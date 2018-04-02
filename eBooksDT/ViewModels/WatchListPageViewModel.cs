using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Services;
using Prism.Navigation;
using eBooksDT.Core.Interfaces;
using eBooksDT.Interfaces;
using eBooksDT.Models;
using System.Threading.Tasks;
using eBooksDT.DataAccess;
using eBooksDT.Core.Helpers;

namespace eBooksDT.ViewModels
{
	public class WatchListPageViewModel : BaseViewModel
	{
		private readonly IMovieService _movieService;
		private readonly IRepository<Movie> _movieRepo;

		private List<eBooksDT.Models.Movie> _toWatchList;
		public List<eBooksDT.Models.Movie> ToWatchList
		{
			get { return _toWatchList; }
			set { SetProperty(ref _toWatchList, value); }
		}

		public WatchListPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService)
			: base(pageDialogService, navigationService)
		{
			var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
			_movieRepo = new Repository<Movie>(connectionService);

			Task.Run(LoadList).ConfigureAwait(true);
		}

		private async Task LoadList()
		{
			try
			{
				var query = _movieRepo.AsQueryable();

				ToWatchList = await query.Where(x => x.ToWatch == true).ToListAsync();
			}
			catch (Exception ex)
			{
				ErrorLog.LogError("Getting In Theater movies", ex);
			}
		}
	}
}