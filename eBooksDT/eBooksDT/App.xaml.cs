using Prism.Unity;
using eBooksDT.Views;
using Xamarin.Forms;
using Acr.UserDialogs;
using Microsoft.Practices.Unity;
using eBooksDT.Interfaces;
using eBooksDT.Models;
using eBooksDT.DataAccess;
using eBooksDT.Core.Helpers;
using eBooksDT.Core.Interfaces;
using eBooksDT.Core;
using System.Threading.Tasks;
using eBooksDT.ViewModels;
using System;
using eBooksDT.Services;

namespace eBooksDT
{
    public partial class App : PrismApplication
    {
        private bool _userAuthenticated = false;
        private IRepository<User> _userRepo;
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            try
            {
                UserService us = new UserService();
                _userAuthenticated = us.IsUserAuthenticated();
                InitializeComponent();
                TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
                var urlPage = _userAuthenticated ? Constants.InitialUrl : Constants.LoginPage;
                NavigationService.NavigateAsync(urlPage, animated: true);
            }
            catch (System.Exception ex)
            {
                ErrorLog.LogError("ERROR: OnInitialized Method", ex);
            }
        }

        protected override void RegisterTypes()
        {
            //Services
            Container.RegisterInstance<IBooksService>(new BooksService());
            Container.RegisterInstance<IUserDialogs>(UserDialogs.Instance);

            //Pages
            Container.RegisterTypeForNavigation<MenuPage>();
            Container.RegisterTypeForNavigation<HomePage>();
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<NavPage>();
            Container.RegisterTypeForNavigation<WatchListPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
            Container.RegisterTypeForNavigation<SignUpPage>();
            Container.RegisterTypeForNavigation<SeenBooksPage>();
            Container.RegisterTypeForNavigation<MostPopularPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<BookDetailPage>();
            Container.RegisterTypeForNavigation<SearchBookPage>();
            Container.RegisterTypeForNavigation<AddToListPage>();
            Container.RegisterTypeForNavigation<BookListsPage>();
            Container.RegisterTypeForNavigation<BookListDetailPage>();
            Container.RegisterTypeForNavigation<BookListInfoPage>();
            Container.RegisterTypeForNavigation<SettingsPage>();
        }

        async Task NavigateToMainPage()
        {
            try
            {
                await NavigationService.NavigateAsync(Constants.InitialUrl);
            }
            catch (Exception ex)
            {
                ShowCrashPage(ex);
            }
        }

        void ShowCrashPage(Exception ex = null)
        {
            Device.BeginInvokeOnMainThread(() => this.MainPage = new CrashPage(ex?.Message));

            ErrorLog.LogError("FATAL ERROR: ", ex);
        }

        void TaskScheduler_UnobservedTaskException(Object sender, UnobservedTaskExceptionEventArgs e)
        {
            if (!e.Observed)
            {
                // prevents the app domain from being torn down
                e.SetObserved();

                // show the crash page
                ShowCrashPage(e.Exception.Flatten().GetBaseException());
            }
        }
    }
}