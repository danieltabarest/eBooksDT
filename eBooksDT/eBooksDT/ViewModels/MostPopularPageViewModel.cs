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

		public MostPopularPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService, IBooksService BookService, IBooksService ibooksservice)
            : base(pageDialogService, navigationService)
        {
            _ibooksservice = ibooksservice;

            var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();

            AddListCommand = new DelegateCommand<Books>(async (Books arg) => await AddToList(arg));

            Task.Run(LoadList).ConfigureAwait(true);
		}

		private async Task LoadList()
		{
			try
			{
				//MostPopularList = await _BookService.DiscoverBook(Core.Constants.DiscoverOption.Popular);
                MostPopularList = await _ibooksservice.DiscoverBooks(Core.Constants.DiscoverOption.Popular);
            }
			catch (Exception ex)
			{
				ErrorLog.LogError("Getting Highest rated Books", ex);
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
                        var Book = selected.Item as Books;
                        param.Add("books", Book);

                        await NavigateToUriWithModalOption(Constants.BookDetailPageNoNav, param, false);
                    });
                }

                return _goToDetailPage;
            }
        }

        private async Task AddToList(Books DetailedBook)
        {
            try
			{
				var param = new NavigationParameters();
				param.Add("Book", DetailedBook);

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
