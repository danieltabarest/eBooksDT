using eBooksDT.Core.Helpers;
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
    public class BookListsPageViewModel : BaseViewModel
    {
        public bool HasData { get; set; } = false;
        private readonly IRepository<CustomList> _listRepo;

        private DelegateCommand<ItemTappedEventArgs> _selectListCommand;

        private List<CustomList> _BookList;
        public List<CustomList> BookList
        {
            get { return _BookList; }
            set { SetProperty(ref _BookList, value); }
        }

        public BookListsPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService)
            : base(pageDialogService, navigationService)
        {
            var connectionService = Xamarin.Forms.DependencyService.Get<ISQLite>();
            _listRepo = new Repository<CustomList>(connectionService);

            Task.Run(LoadList).ConfigureAwait(true);
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
                ErrorLog.LogError("ERROR: Getting Lists", ex);
            }
        }

        public DelegateCommand<ItemTappedEventArgs> SelectListCommand
        {
            get
            {
                if (_selectListCommand == null)
                {
                    _selectListCommand = new DelegateCommand<ItemTappedEventArgs>(async selected =>
                    {
                        var list = selected.Item as CustomList;
                        await SelectList(list);
                    });
                }
                return _selectListCommand;
            }
        }

        private async Task SelectList(CustomList customList)
        {
            try
            {
                var param = new NavigationParameters();
                param.Add("list", customList);

				await NavigateToUriWithModalOption(Constants.BookListInfoPage, param, false);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("ERROR: selecting list", ex);
            }
        }
    }
}
