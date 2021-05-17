using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using Prism.Unity;
using ProfileBook.Resources;
using ProfileBook.Services.Authorization;
using ProfileBook.Services.Profile;
using ProfileBook.Services.Repository;
using ProfileBook.Services.Settings;
using ProfileBook.Services.Validators;
using ProfileBook.ViewModels;
using ProfileBook.Views;
using System;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProfileBook
{
    public partial class App : PrismApplication
    {
        private Services.Settings.ISettingsManager _settingsManager;
        private Services.Settings.ISettingsManager SettingsManager =>
            _settingsManager ??= Container.Resolve<Services.Settings.ISettingsManager>();
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        #region --- Overrides ---

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterInstance<Services.Settings.ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthorizationService>(Container.Resolve<AuthorizationService>());
            containerRegistry.RegisterInstance<IProfileService>(Container.Resolve<ProfileService>());
            containerRegistry.RegisterInstance<IValidators>(Container.Resolve<Validators>());

            //Navigation
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SignInPage, SignInPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<MainListPage, MainListPageViewModel>();
            containerRegistry.RegisterForNavigation<AddProfilePage, AddProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfileImagePage, ProfileImagePageViewModel>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            var cultureInfo = new CultureInfo(SettingsManager.Culture, false);

            //Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Resource.Culture = cultureInfo;

            if (SettingsManager.UserId == 0)
            {
                NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInPage)}");
            }
            else
            {
                NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListPage)}");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion
    }
}
