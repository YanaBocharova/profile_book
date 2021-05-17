using Acr.UserDialogs;
using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Services.Authorization;
using ProfileBook.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    class SignInPageViewModel : BindableBase
    {
        INavigationService navigationService;
        IAuthorizationService authorizationService;

        public SignInPageViewModel(INavigationService navigationService,
                                   IAuthorizationService authorizationService)
        {
            Title = "Users SignIn";
            this.navigationService = navigationService;
            this.authorizationService = authorizationService;
        }

        #region --- Public Properties ---

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string login;
        public string Login
        {
            get => login;
            set
            {
                SetProperty(ref login, value);
                CheckEntries();
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                CheckEntries();
            }
        }

        private bool enabledButton = false;
        public bool EnabledButton
        {
            get => enabledButton;
            set => SetProperty(ref enabledButton, value);
        }

        public ICommand SignInTapCommand => new Command(OnSignInTap);
        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        #endregion

        #region --- Private Methods ---

        private void CheckEntries()
        {
            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password))
            {
                EnabledButton = false;
            }
            else
            {
                EnabledButton = true;
            }
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnSignInTap()
        {
            var isAutorize = await authorizationService.SignIn(login, password);

            if (isAutorize)
            {
                await navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListPage)}");
            }
            else
            {
                Password = string.Empty;
                UserDialogs.Instance.Alert("Invalid login or password!!", "Alert", "OK");
            }
        }

        private async void OnSignUpTap()
        {
            await navigationService.NavigateAsync($"{nameof(SignUpPage)}");
        }

        #endregion

    }
}
