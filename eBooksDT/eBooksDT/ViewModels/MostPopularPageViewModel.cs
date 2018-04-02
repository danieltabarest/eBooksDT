﻿using System;
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
		private readonly IMovieService _movieService;
        private readonly IBooksService _ibooksservice;
        private readonly IRepository<eBooksDT.Models.Movie> _movieRepo;

        public DelegateCommand<DetailedMovie> AddWatchListCommand { get; set; }
        public DelegateCommand<DetailedMovie> AddSeenCommand { get; set; }
        public DelegateCommand<DetailedMovie> AddListCommand { get; set; }
        private DelegateCommand<ItemTappedEventArgs> _goToDetailPage;


        private List<eBooksDT.Core.Models.DetailedBook> _mostPopularList;
		public List<eBooksDT.Core.Models.DetailedBook> MostPopularList
		{
			get { return _mostPopularList; }
			set { SetProperty(ref _mostPopularList, value); }
		}

		private bool _isActive;

		public MostPopularPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService, IMovieService movieService)
            : base(pageDialogService, navigationService)
        {
			_movieService = movieService;

			var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
			_movieRepo = new Repository<eBooksDT.Models.Movie>(connectionService);

            AddWatchListCommand = new DelegateCommand<DetailedMovie>(async (DetailedMovie arg) => await AddToWatchList(arg));
            AddSeenCommand = new DelegateCommand<DetailedMovie>(async (DetailedMovie arg) => await AddToSeenList(arg));
            AddListCommand = new DelegateCommand<DetailedMovie>(async (DetailedMovie arg) => await AddToList(arg));

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
                        var movie = selected.Item as DetailedMovie;
                        param.Add("movie", movie);

                        await NavigateToUriWithModalOption(Constants.MovieDetailPageNoNav, param, false);
                    });
                }

                return _goToDetailPage;
            }
        }

        private async Task AddToWatchList(DetailedMovie detailedMovie)
        {
            try
            {

                var booksGet = await _movieRepo.Get(x => x.ToWatch == true);
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
