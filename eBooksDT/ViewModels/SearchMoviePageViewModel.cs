using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Services;
using Prism.Navigation;
using Xamarin.Forms;
using System.Threading.Tasks;
using eBooksDT.Core.Interfaces;
using eBooksDT.Core.Helpers;
using eBooksDT.Core.Models;
using eBooksDT.Interfaces;
using eBooksDT.DataAccess;

namespace eBooksDT.ViewModels
{
	public class SearchMoviePageViewModel : BaseViewModel
	{
		private readonly IMovieService _movieService;
        private readonly IRepository<eBooksDT.Models.Movie> _movieRepo;

        public bool IsBusy { get; set; }

		private string _searchField;
		public string SearchField
		{
			get { return _searchField; }
			set { SetProperty(ref _searchField, value); }
		}

		private List<eBooksDT.Core.Models.DetailedMovie> _searchList;
		public List<eBooksDT.Core.Models.DetailedMovie> SearchList
		{
			get { return _searchList; }
			set { SetProperty(ref _searchList, value); }
		}

		private DelegateCommand<TextChangedEventArgs> _searchMovieCommand;
        public DelegateCommand<DetailedMovie> AddWatchListCommand { get; set; }
        public DelegateCommand<DetailedMovie> AddSeenCommand { get; set; }
        public DelegateCommand<DetailedMovie> AddListCommand { get; set; }

        private DelegateCommand<ItemTappedEventArgs> _goToDetailPage;

		public SearchMoviePageViewModel(IPageDialogService pageDialogService, INavigationService navigationService, IMovieService movieService)
			: base(pageDialogService, navigationService)
		{
			_movieService = movieService;

            var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
			if (connectionService != null)
			{
				_movieRepo = new Repository<eBooksDT.Models.Movie>(connectionService);
			}

            AddWatchListCommand = new DelegateCommand<DetailedMovie>(async (DetailedMovie arg) => await AddToWatchList(arg));
            AddSeenCommand = new DelegateCommand<DetailedMovie>(async (DetailedMovie arg) => await AddToSeenList(arg));
            AddListCommand = new DelegateCommand<DetailedMovie>(async (DetailedMovie arg) => await AddToList(arg));
        }

		public DelegateCommand<TextChangedEventArgs> SearchMovieCommand
		{
			get
			{
				if (_searchMovieCommand == null)
				{
					_searchMovieCommand = new DelegateCommand<TextChangedEventArgs>(async textchanged =>
					{
						SearchField = textchanged.NewTextValue;

						if (!IsBusy && !string.IsNullOrEmpty(SearchField))
						{
							IsBusy = true;
							await SearchMovie();
						}
					});
				}

				return _searchMovieCommand;
			}
		}

		public async Task SearchMovie()
		{
			try
			{
				SearchList = await _movieService.SearchMovie(SearchField);
			}
			catch (Exception ex)
			{
				ErrorLog.LogError("ERROR: Searching movies", ex);
			}
			finally
			{ 
				IsBusy = false;
			}
		}

		public DelegateCommand<ItemTappedEventArgs> GoToDetailPage
		{
			get
			{
				if (_goToDetailPage == null)
				{
					_goToDetailPage = new DelegateCommand<ItemTappedEventArgs>(async selected =>
					{
						var param = new NavigationParameters();
						var movie = selected.Item as eBooksDT.Core.Models.DetailedMovie;
						param.Add("movie", movie);

						await NavigateToUri(Constants.MovieDetailPageNoNav, param);
					});
				}

				return _goToDetailPage;
			}
		}

        private async Task AddToWatchList(DetailedMovie detailedMovie)
        {
            try
            {
                var movieGet = await _movieRepo.Get(x => x.MovieId == detailedMovie.Id && x.ToWatch == true);

                if (movieGet == null)
                {
                    await _movieRepo.Insert(new eBooksDT.Models.Movie
                    {
                        MovieName = detailedMovie.OriginalTitle,
                        ToWatch = true,
                        PosterURL = detailedMovie.PosterUrl,
                        MovieRate = detailedMovie?.Score == null ? "N/A" : detailedMovie?.Score.ToString(),
                        MovieDescription = detailedMovie.Overview,
                        MovieId = detailedMovie.Id,
                        DateAdded = DateTime.Now
                    });

                    await DisplayDialog("Info", "Added to WatchList", "Ok");
                }
                else
                {
                    await DisplayDialog("Info", "This Movie already exist in your watchlist", "Ok");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Adding to Watchlist", ex);
            }
        }

        private async Task AddToSeenList(DetailedMovie detailedMovie)
        {
            try
            {
                await _movieRepo.Insert(new eBooksDT.Models.Movie
                {
                    MovieName = detailedMovie.OriginalTitle,
                    AlreadySeen = true,
                    PosterURL = detailedMovie.PosterUrl,
                    MovieRate = detailedMovie?.Score == null ? "N/A" : detailedMovie?.Score.ToString(),
                    MovieDescription = detailedMovie.Overview,
                    MovieId = detailedMovie.Id,
                    DateAdded = DateTime.Now
                });

                await DisplayDialog("Info", "Added to Seen Movies", "Ok");
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Adding to Seen list", ex);
            }
        }

        private async Task AddToList(DetailedMovie detailedMovie)
        {
            try
            {
                await DisplayDialog("Info", "Added to List", "Ok");
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Adding to list", ex);
            }
        }
    }
}