using Acr.UserDialogs;
using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Resources;
using ProfileBook.Services.Authorization;
using ProfileBook.Services.Repository;
using ProfileBook.Services.Settings;
using ProfileBook.Services.Validators;
using ProfileBook.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    class SignUpPageViewModel : BindableBase
    {
        INavigationService navigationService;
        IAuthorizationService authorizationService;
        IValidators validators;

        public SignUpPageViewModel(INavigationService navigationService,
                                   IAuthorizationService authorizationService,
                                   IValidators validators)
        {
            Title = "Users SignUp";
            this.navigationService = navigationService;
            this.authorizationService = authorizationService;
            this.validators = validators;
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

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
                CheckEntries();
            }
        }

        private bool enabledButton = false;
        public bool EnabledButton
        {
            get => enabledButton;
            set => SetProperty(ref enabledButton, value);
        }

        public ICommand SignUpTapCommand => new Command(OnSignUpTap);

        #endregion

        #region --- Private Methods ---

        private void CheckEntries()
        {
            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                EnabledButton = false;
            }
            else
            {
                EnabledButton = true;
            }
        }

        private void ClearEntries()
        {
            Login = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        private void ShowAlert(string msg)
        {
            UserDialogs.Instance.Alert(msg, Resource.Alert, "OK");
        }

        private bool IsLoginValidate()
        {
            if (validators.IsFirstSymbolDigit(Login))
            {
                ShowAlert(Resource.IsFirstSymbolDigit);
                ClearEntries();
                return false;
            }
            if (!validators.IsCorrectLength(Login, 4))
            {
                ShowAlert(Resource.HasLoginCorrectLength);
                ClearEntries();
                return false;
            }
            return true;
        }

        private bool IsPassValidate()
        {
            if (!validators.IsPassAvailable(Password))
            {
                ShowAlert(Resource.IsPassAvailable);
                ClearEntries();
                return false;
            }
            if (!validators.IsCorrectLength(Password, 8))
            {
                ShowAlert(Resource.HasPassCorrectLength);
                ClearEntries();
                return false;
            }
            if (!validators.ArePasswordsEquals(Password, ConfirmPassword))
            {
                ShowAlert(Resource.ArePasswordsEquals);
                ClearEntries();
                return false;
            }
            return true;
        }

        private UserModel CreateUser()
        {
            UserModel userModel = null;

            if (Login != Password)
            {
                userModel = new UserModel()
                {
                    Login = Login,
                    Password = Password
                };
            }
            else
            {
                ShowAlert(Resource.AreLogAndPassEquals);
                ClearEntries();
            }

            return userModel;
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnSignUpTap()
        {
            if (IsLoginValidate() && IsPassValidate())
            {
                var isLoginBusy = await authorizationService.IsLoginBusy(Login);
                if (isLoginBusy)
                {
                    ShowAlert(Resource.IsLoginBusy);
                    ClearEntries();
                }
                else
                {
                    var userModel = CreateUser();
                    if (userModel != null)
                    {
                        authorizationService.SignUp(userModel);
                        await navigationService.GoBackAsync();
                    }
                }
            }
        }

        #endregion

    }
}
