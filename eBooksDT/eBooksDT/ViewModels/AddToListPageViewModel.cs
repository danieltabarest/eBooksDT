using eBooksDT.Core.Helpers;
using eBooksDT.Core.Models;
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
using Xamarin.Forms;

namespace eBooksDT.ViewModels
{
    public class AddToListPageViewModel : BaseViewModel
    {
        private readonly IRepository<CustomList> _listRepo;
        private readonly IRepository<eBooksDT.Models.Book> _BookRepo;
        private DelegateCommand<ItemTappedEventArgs> _addToListCommand;
        private Command _createListCommand;
        public bool HasData { get; set; } = false;
        private DetailedBook _selectedBook;

        private string _newList;
        public string NewList
        {
            get { return _newList; }
            set { SetProperty(ref _newList, value); }
        }

        private List<CustomList> _BookList;
        public List<CustomList> BookList
        {
            get { return _BookList; }
            set { SetProperty(ref _BookList, value); }
        }

        public AddToListPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService)
            : base(pageDialogService, navigationService)
        {
            var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
            _listRepo = new Repository<CustomList>(connectionService);
            _BookRepo = new Repository<eBooksDT.Models.Book>(connectionService);
            Task.Run(LoadList).ConfigureAwait(true);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            _selectedBook = (DetailedBook)parameters["Book"];
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            _selectedBook = (DetailedBook)parameters["Book"];
        }

        public DelegateCommand<ItemTappedEventArgs> AddToListCommand
        {
            get
            {
                if (_addToListCommand == null)
                {
                    _addToListCommand = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        var list = selected.Item as CustomList;
                        //await AddBookToList(list);
                    });
                }
                return _addToListCommand;
            }
        }

        public Command CreateListCommand
        {
            get
            {
                return this._createListCommand ?? (this._createListCommand = new  Command(
                 async () =>
                  {
                      await CreateList();
                  },
                  () =>
                  {
                      return true;
                  }));
            }
        }

        private async Task LoadList()
        {
            try
            {
                BookList = await _listRepo.GetAllAsync();
                HasData = BookList.Count() > 0 ? false : true;
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Getting In Theater Books", ex);
            }
        }

      
        private async Task CreateList()
        {
            var list = new CustomList() {
                Name = NewList,
                DateAdded = DateTime.Today
            };
            await _listRepo.Insert(list);
            //refresh
            await LoadList();
        }
    }
}
