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
	public class SearchBookPageViewModel : BaseViewModel
	{
		private readonly IBooksService _BookService;
        private readonly IRepository<eBooksDT.Models.Book> _BookRepo;

        public bool IsBusy { get; set; }

		private string _searchField;
		public string SearchField
		{
			get { return _searchField; }
			set { SetProperty(ref _searchField, value); }
		}

		private List<eBooksDT.Core.Models.DetailedBook> _searchList;
		public List<eBooksDT.Core.Models.DetailedBook> SearchList
		{
			get { return _searchList; }
			set { SetProperty(ref _searchList, value); }
		}

		private DelegateCommand<TextChangedEventArgs> _searchBookCommand;
        public DelegateCommand<DetailedBook> AddWatchListCommand { get; set; }
        public DelegateCommand<DetailedBook> AddSeenCommand { get; set; }
        public DelegateCommand<DetailedBook> AddListCommand { get; set; }

        private DelegateCommand<ItemTappedEventArgs> _goToDetailPage;

		public SearchBookPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService, IBooksService BookService)
			: base(pageDialogService, navigationService)
		{
			_BookService = BookService;

            var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
			if (connectionService != null)
			{
				_BookRepo = new Repository<eBooksDT.Models.Book>(connectionService);
			}

            AddListCommand = new DelegateCommand<DetailedBook>(async (DetailedBook arg) => await AddToList(arg));
        }

		public DelegateCommand<TextChangedEventArgs> SearchBookCommand
		{
			get
			{
				if (_searchBookCommand == null)
				{
					_searchBookCommand = new DelegateCommand<TextChangedEventArgs>(async textchanged =>
					{
						SearchField = textchanged.NewTextValue;

						if (!IsBusy && !string.IsNullOrEmpty(SearchField))
						{
							IsBusy = true;
							await SearchBook();
						}
					});
				}

				return _searchBookCommand;
			}
		}

		public async Task SearchBook()
		{
			try
			{
				SearchList = await _BookService.SearchBooks(SearchField);
			}
			catch (Exception ex)
			{
				ErrorLog.LogError("ERROR: Searching Books", ex);
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
						var Book = selected.Item as eBooksDT.Core.Models.DetailedBook;
						param.Add("Book", Book);

						await NavigateToUri(Constants.BookDetailPageNoNav, param);
					});
				}

				return _goToDetailPage;
			}
		}

       

        private async Task AddToList(DetailedBook detailedBook)
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