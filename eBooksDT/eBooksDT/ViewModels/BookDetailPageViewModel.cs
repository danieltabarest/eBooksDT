using Acr.UserDialogs;
using eBooksDT.Core.Helpers;
using eBooksDT.Core.Interfaces;
using eBooksDT.Core.Models;
using eBooksDT.DataAccess;
using eBooksDT.Interfaces;
using eBooksDT.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Prism.Services;

namespace eBooksDT.ViewModels
{
	public class BookDetailPageViewModel : BaseViewModel
    {
        private readonly IBooksService _BookService;
        private readonly IRepository<eBooksDT.Models.Book> _BookRepo;

        private Books _BookItem;
        public Books BookItem
        {
            get { return _BookItem; }
            set { SetProperty(ref _BookItem, value); }
        }

        //Delegates
        public DelegateCommand AddWatchListCommand { get; set; }
        public DelegateCommand AddSeenCommand { get; set; }
        public DelegateCommand AddListCommand { get; set; }

		public BookDetailPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService, 
		                                IBooksService BookService)
			: base(pageDialogService, navigationService)
		{
			try
			{
				_BookService = BookService;

				var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
				_BookRepo = new Repository<eBooksDT.Models.Book>(connectionService);

				AddListCommand = new DelegateCommand(async () => await AddToList());
			}
			catch (Exception ex)
			{
				ErrorLog.LogError("ERROR: Loading Book detail", ex);
			}
        }

		public override void OnNavigatedTo(NavigationParameters parameters)
		{
			BookItem = (Books)parameters["books"];
		}

		public override void OnNavigatedFrom(NavigationParameters parameters)
		{
			BookItem = (Books)parameters["books"];
		}

       

        private async Task AddToList()
        {
            try
            {
				await DisplayDialog("Info", "This is not implemented", "OK");
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Adding to list", ex);
            }
        }
    }
}
