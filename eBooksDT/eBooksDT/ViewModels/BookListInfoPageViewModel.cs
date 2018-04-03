using eBooksDT.Core.Helpers;
using eBooksDT.Core.Interfaces;
using eBooksDT.DataAccess;
using eBooksDT.Interfaces;
using eBooksDT.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBooksDT.ViewModels
{
    public class BookListInfoPageViewModel : BaseViewModel
    {
        private readonly IRepository<Book> _BookRepo;
        private CustomList _customList { get; set; }

		private string _listTitle;
		public string ListTitle
		{
			get { return _listTitle; }
			set { SetProperty(ref _listTitle, value); }
		}

        private List<eBooksDT.Models.Book> _BooksList;
        public List<eBooksDT.Models.Book> BooksList
        {
            get { return _BooksList; }
            set { SetProperty(ref _BooksList, value); }
        }
        public BookListInfoPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService, IBooksService BookService)
            : base(pageDialogService, navigationService)
        {
            var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
            _BookRepo = new Repository<Book>(connectionService);
        }

        private async Task LoadList()
        {
            try
            {
                var query = _BookRepo.AsQueryable();

                var BookList = await query.Where(x => x.ListId == _customList.id).ToListAsync();
                BooksList = BookList;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Getting In Theater Books", ex);
            }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
			try
			{
				_customList = (CustomList)parameters["list"];
				ListTitle = _customList.Name;
				Task.Run(LoadList).ConfigureAwait(true);
			}
			catch (Exception ex)
			{
				ErrorLog.LogError("ERROR: Loading list Books", ex);
			}
        }
    }
}