using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Services;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using eBooksDT.Core.Helpers;
using eBooksDT.Models;
using eBooksDT.Services;

namespace eBooksDT.ViewModels
{
	public class MenuPageViewModel : BaseViewModel
	{
		INavigationService _navigationService;

		private DelegateCommand<ItemTappedEventArgs> _goToPage;

        private List<MenuItem> _menuItemsList;
        public List<MenuItem> MenuItemsList
        {
            get { return _menuItemsList; }
            set { SetProperty(ref _menuItemsList, value); }
        }

        private User _activeUser;
        public User ActiveUser
        {
            get { return _activeUser; }
            set { SetProperty(ref _activeUser, value); }
        }

        public MenuPageViewModel(IPageDialogService pageDialogService, INavigationService navigationService)
			: base(pageDialogService, navigationService)
		{
			Task.Run(LoadList).ConfigureAwait(false);
            Task.Run(LoadUserInfo).ConfigureAwait(false);
		}

        private async Task LoadUserInfo()
        {
            var us = new UserService();
            ActiveUser = await us.GetActiveUser();
        }

        private async Task LoadList()
        {
            try
            {
                MenuItemsList = await this.GetMenuItems();
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Getting In Theater Books", ex);
            }
        }

        public DelegateCommand<ItemTappedEventArgs> GoToPage
		{
			get
			{
				if (_goToPage == null)
				{
					_goToPage = new DelegateCommand<ItemTappedEventArgs>(async selected =>
					{
						var menuItem = selected.Item as MenuItem;

						await NavigateToUri(menuItem.Destination);
					});
				}

                return _goToPage;
			}
		}

		async Task<List<MenuItem>> GetMenuItems()
		{
            var menuItems = new List<MenuItem>();
			try
			{
                //add an extra load time to the consumption of the method
                await Task.Delay(10);
                menuItems.Add(new MenuItem()
                {
                    Title = "Home",
                    Icon = "ic_home.png",
                    Destination = Constants.InitialUrl
                });


                menuItems.Add(new MenuItem()
                {
                    Title = "Lists",
                    Icon = "ic_list.png",
					Destination = Constants.ListPage
                });

                menuItems.Add(new MenuItem()
                {
                    Title = "Settings",
                    Icon = "ic_settings.png",
                    Destination = Constants.SettingsPage
                });
            }
			catch (Exception ex)
			{
                ErrorLog.LogError("ERROR: Loading Menu items",ex);
			}
            return menuItems;
		}
	}
}
