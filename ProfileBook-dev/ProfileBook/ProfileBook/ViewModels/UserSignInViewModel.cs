using Prism.Navigation;
using Prism.Commands;
using ProfileBook.View;
using ProfileBook.Services.Authorization;
using ProfileBook.Services.Settings;
using Acr.UserDialogs;
using Xamarin.Forms;
using ProfileBook.Services.DbService;
using ProfileBook.Resx;

namespace ProfileBook.ViewModels
{
    public class UserSignInViewModel : ViewModelBase
    {
        public UserSignInViewModel(INavigationService navigationService, IDbService dbService, ISettingsManagerService settingsManager, IAuthorization authorization) : base(navigationService, dbService, settingsManager)
        {
            Title = Resource.SignInTitlePage;
            authorizationService = authorization;
        }


        #region Private fields

        string userLogin;
        string userPassword;
        bool isEnabled;
        IAuthorization authorizationService;

        #endregion


        #region Commands

        public DelegateCommand OnSignUpTapCommand => new DelegateCommand(GoSignUp);
        public DelegateCommand OnSignInTapCommand => new DelegateCommand(AuthorizationUser, CanExecute);

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

        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }

        #endregion


        #region Override

        public override void Initialize(INavigationParameters parameters)
        {
            UserLogin = (string)parameters["login"];
        }

        #endregion


        #region private helpers

        private async void GoSignUp() => await NavigationService.NavigateAsync(nameof(UserSignUp));


        private async void AuthorizationUser()
        {
            bool result = await authorizationService.IsAuthorization(UserLogin, UserPassword);

            if (result)
            {
                SettingsManager.LoggedUser = UserLogin;

                await NavigationService.NavigateAsync(nameof(MainListView));

                // clear navigation stack
                NavigationPage page = (NavigationPage)App.Current.MainPage;
                while(page.Navigation.NavigationStack.Count > 1)
                    page.Navigation.RemovePage(page.Navigation.NavigationStack[page.Navigation.NavigationStack.Count - 2]);

                UserLogin = "";
                UserPassword = "";
            }
            else
            {
                UserDialogs.Instance.Alert(Resource.INVALID_LOGIN_OR_PASSWORD);
                UserPassword = "";
            }
        }


        private bool CanExecute() => IsEnabled;

        #endregion
    }
}