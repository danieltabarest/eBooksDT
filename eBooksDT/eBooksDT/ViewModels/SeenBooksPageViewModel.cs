using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Services;
using Prism.Navigation;
using eBooksDT.Core.Interfaces;
using eBooksDT.Interfaces;
using eBooksDT.DataAccess;
using System.Threading.Tasks;
using eBooksDT.Models;
using eBooksDT.Core.Helpers;

namespace eBooksDT.ViewModels
{
	public class SeenBooksPageViewModel : BaseViewModel
	{
		private readonly IBooksService _BookService;
		private readonly IRepository<Book> _BookRepo;

		private List<eBooksDT.Models.Book> _seenBooksList;
		public List<eBooksDT.Models.Book> SeenBooksList
		{
			get { return _seenBooksList; }
			set { SetProperty(ref _seenBooksList, value); }
		}

		public SeenBooksPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService)
			: base(pageDialogService, navigationService)
		{
			var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
			_BookRepo = new Repository<Book>(connectionService);

			Task.Run(LoadList).ConfigureAwait(true);
		}

		private async Task LoadList()
		{
			try
			{
				var query = _BookRepo.AsQueryable();

				SeenBooksList = await query.Where(x => x.AlreadySeen == true).ToListAsync();
			}
			catch (Exception ex)
			{
				ErrorLog.LogError("Getting In Theater Books", ex);
			}
		}
	}
}