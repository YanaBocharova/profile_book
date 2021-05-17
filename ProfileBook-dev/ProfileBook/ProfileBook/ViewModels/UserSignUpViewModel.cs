using Prism.Navigation;
using Prism.Commands;
using ProfileBook.ServiceData.Enums;
using ProfileBook.Services.Registration;
using Acr.UserDialogs;
using ProfileBook.View;
using ProfileBook.Services.DbService;
using ProfileBook.Models;
using ProfileBook.Resx;

namespace ProfileBook.ViewModels
{
    public class UserSignUpViewModel : ViewModelBase
    {
        public UserSignUpViewModel(INavigationService navigationService, IDbService _dbService, IRegistration registration) : base(navigationService, _dbService)
        {
            Title = Resource.SignUpTitlePage;
            registrationService = registration;
        }


        #region Private fields

        private string userLogin;
        private string userPassword;
        private string confirmUserPassword;
        private bool isEnabled;
        private IRegistration registrationService;

        #endregion


        #region Commands

        public DelegateCommand SignUpButtonTapCommand => new DelegateCommand(RegistrationNewUser, CanExecute);

        #endregion


        #region Properties

        public string UserLogin
        {
            get => userLogin;
            set => SetProperty(ref userLogin, value);
        }

        public string UserPassword
        {
            get => userPassword;
            set => SetProperty(ref userPassword, value);
        }

        public string UserConfirmPassword
        {
            get => confirmUserPassword;
            set => SetProperty(ref confirmUserPassword, value);
        }


        public bool IsEnabled 
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }

        #endregion


        #region Private helpers

        private async void RegistrationNewUser()
        {
            CodeUserAuthResult result = await registrationService.IsRegistration(UserLogin, UserPassword, UserConfirmPassword);

            switch (result)
            {
                case CodeUserAuthResult.InvalidLogin:
                    UserDialogs.Instance.Alert(Resource.INVALID_LOGIN);
                    UserLogin = "";
                    UserPassword = "";
                    UserConfirmPassword = "";
                    break;
                case CodeUserAuthResult.InvalidPassword:
                    UserDialogs.Instance.Alert(Resource.INVALID_PASSWORD);
                    UserPassword = "";
                    UserConfirmPassword = "";
                    break;
                case CodeUserAuthResult.PasswordMismatch:
                    UserDialogs.Instance.Alert(Resource.PASSWORD_MISMATCH);
                    UserPassword = "";
                    UserConfirmPassword = "";
                    break;
                case CodeUserAuthResult.LoginTaken:
                    UserDialogs.Instance.Alert(Resource.LOGIN_TAKEN);
                    UserLogin = "";
                    UserPassword = "";
                    UserConfirmPassword = "";
                    break;
                case CodeUserAuthResult.Passed:
                    UserDialogs.Instance.Alert(Resource.AUTHETICATION_SUCCESS);
                    await DbService.InsertDataAsync(new UserModel
                    {
                        Login = UserLogin,
                        Password = UserPassword
                    });

                    NavigationParameters parameter = new NavigationParameters
                    {
                        { "login", UserLogin }
                    };
                    await NavigationService.NavigateAsync(nameof(UserSignIn), parameter);
                    break;
            }
        }

        private bool CanExecute() => IsEnabled;

        #endregion
    }
}