using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using eBooksDT.Core.Helpers;
using eBooksDT.Core.Interfaces;
using eBooksDT.DataAccess;
using eBooksDT.Interfaces;
using eBooksDT.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using Prism.Commands;
using eBooksDT.Core.Models;
using Prism.Services;

namespace eBooksDT.ViewModels
{
	public class MostPopularPageViewModel : BaseViewModel
	{
        private readonly IBooksService _ibooksservice;

        public DelegateCommand<Books> AddWatchListCommand { get; set; }
        public DelegateCommand<Books> AddSeenCommand { get; set; }
        public DelegateCommand<Books> AddListCommand { get; set; }
        private DelegateCommand<ItemTappedEventArgs> _goToDetailPage;


        private List<eBooksDT.Core.Models.Books> _mostPopularList;
		public List<eBooksDT.Core.Models.Books> MostPopularList
		{
			get { return _mostPopularList; }
			set { SetProperty(ref _mostPopularList, value); }
		}

		private bool _isActive;

		public MostPopularPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService, IMovieService movieService, IBooksService ibooksservice)
            : base(pageDialogService, navigationService)
        {
            _ibooksservice = ibooksservice;

            var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();

            AddWatchListCommand = new DelegateCommand<Books>(async (Books arg) => await AddToWatchList(arg));
            AddSeenCommand = new DelegateCommand<Books>(async (Books arg) => await AddToSeenList(arg));
            AddListCommand = new DelegateCommand<Books>(async (Books arg) => await AddToList(arg));

            Task.Run(LoadList).ConfigureAwait(true);
		}

		private async Task LoadList()
		{
			try
			{
				//MostPopularList = await _movieService.DiscoverMovie(Core.Constants.DiscoverOption.Popular);
                MostPopularList = await _ibooksservice.DiscoverBooks(Core.Constants.DiscoverOption.Popular);
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
                if (_goToDetailPage == null)
                {
                    _goToDetailPage = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        var param = new NavigationParameters();
                        var movie = selected.Item as Books;
                        param.Add("books", movie);

                        await NavigateToUriWithModalOption(Constants.MovieDetailPageNoNav, param, false);
                    });
                }

                return _goToDetailPage;
            }
        }

        private async Task AddToWatchList(Books DetailedBook)
        {
            try
            {

                /*var booksGet = await _movieRepo.Get(x => x.ToWatch == true);
                var movieGet = await _movieRepo.Get(x => x.MovieId == DetailedBook.Id && x.ToWatch == true);

                if (movieGet == null)
                {
                    await _movieRepo.Insert(new eBooksDT.Models.Movie
                    {
                        MovieName = DetailedBook.OriginalTitle,
                        ToWatch = true,
                        PosterURL = DetailedBook.PosterUrl,
                        MovieRate = DetailedBook?.Score == null ? "N/A" : DetailedBook?.Score.ToString(),
                        MovieDescription = DetailedBook.Overview,
                        MovieId = DetailedBook.Id,
                        DateAdded = DateTime.Now
                    });

                    await DisplayDialog("Info", "Added to WatchList", "Ok");
                }
                else
                {
                    await DisplayDialog("Info", "This Movie already exist in your watchlist", "Ok");
                }*/
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Adding to Watchlist", ex);
            }
        }

        private async Task AddToSeenList(Books DetailedBook)
        {
            try
            {
              /*  await _movieRepo.Insert(new eBooksDT.Models.Movie
                {
                    MovieName = DetailedBook.OriginalTitle,
                    AlreadySeen = true,
                    PosterURL = DetailedBook.PosterUrl,
                    MovieRate = DetailedBook?.Score == null ? "N/A" : DetailedBook?.Score.ToString(),
                    MovieDescription = DetailedBook.Overview,
                    MovieId = DetailedBook.Id,
                    DateAdded = DateTime.Now
                });

                await DisplayDialog("Info", "Added to Seen Movies", "Ok");*/
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Adding to Seen list", ex);
            }
        }

        private async Task AddToList(Books DetailedBook)
        {
            try
			{
				var param = new NavigationParameters();
				param.Add("movie", DetailedBook);

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
