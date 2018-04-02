using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eBooksDT.Core.Helpers;
using eBooksDT.Core.Interfaces;
using eBooksDT.DataAccess;
using eBooksDT.Interfaces;
using eBooksDT.Models;
using Prism.Navigation;
using Prism.Services;
using Prism.Commands;
using eBooksDT.Core.Models;
using Xamarin.Forms;

namespace eBooksDT.ViewModels
{
	public class ComingSoonPageViewModel : BaseViewModel
	{
		private readonly IMovieService _movieService;
		private readonly IRepository<eBooksDT.Models.Movie> _movieRepo;
        public DelegateCommand<DetailedMovie> AddWatchListCommand { get; set; }
		private DelegateCommand<ItemTappedEventArgs> _goToDetailPage;

        private List<eBooksDT.Core.Models.DetailedMovie> _comingSoonList;
		public List<eBooksDT.Core.Models.DetailedMovie> ComingSoonList
		{
			get { return _comingSoonList; }
			set { SetProperty(ref _comingSoonList, value); }
		}

		private bool _isActive;

		public ComingSoonPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService,
										IMovieService movieService)
			: base(pageDialogService, navigationService)
		{
			_movieService = movieService;

			var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
			_movieRepo = new Repository<eBooksDT.Models.Movie>(connectionService);

            AddWatchListCommand = new DelegateCommand<DetailedMovie>(async (DetailedMovie arg) => await AddToWatchList(arg));

            Task.Run(LoadList).ConfigureAwait(true);
		}

		private async Task LoadList()
		{
			try
			{
				ComingSoonList = await _movieService.DiscoverMovie(Core.Constants.DiscoverOption.Upcoming);
			}
			catch (Exception ex)
			{
				ErrorLog.LogError("Getting Highest rated movies", ex);
			}
		}

		public DelegateCommand<ItemTappedEventArgs> GoToDetailPage
		{
			get
			{
                try
                {
                    if (_goToDetailPage == null)
                    {
                        _goToDetailPage = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                        {
                            var param = new NavigationParameters();
                            var movie = selected.Item as DetailedMovie;
                            param.Add("movie", movie);

                            await NavigateToUriWithModalOption(Constants.MovieDetailPageNoNav, param, false);
                        });
                    }

                    return _goToDetailPage;
                }
                catch (Exception ex )
                {
                    ErrorLog.LogError("Go To Detail Page", ex); ;
                    return _goToDetailPage;
                }
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
				var param = new NavigationParameters();
				param.Add("movie", detailedMovie);
				await NavigateToUri(Constants.AddToListPage, param);
			}
			catch (Exception ex)
			{
				ErrorLog.LogError("Adding to list", ex);
			}
		}

        public bool IsActive
		{
			get
			{
				return _isActive;
			}

			set
			{
				_isActive = value;
			}
		}
	}
}
