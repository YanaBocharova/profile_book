using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using ProfileBook.Models;
using ProfileBook.Services.DbService;
using ProfileBook.Services.Settings;
using ProfileBook.View;
using ProfileBook.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Prism.Services.Dialogs;
using ProfileBook.Resx;

namespace ProfileBook.ViewModels
{
    public class MainListViewModel : ViewModelBase, IInitializeAsync, INavigatedAware
    {
        public MainListViewModel(INavigationService navigationService, IDbService dbService, ISettingsManagerService settingsManager, IDialogService dialogService) : base(navigationService, dbService, settingsManager)
        {
            Title = Resource.MainListTitlePage;
            DialogService = dialogService;
        }


        #region Private fields

        private ObservableCollection<ProfileModel> profileList;
        private bool isVisible = false;

        #endregion


        #region Implement interfaces

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            List<ProfileModel> profiles = await DbService.GetOwnersProfilesAsync(SettingsManager.LoggedUser);
            profiles.Sort(new ProfileModelComparer(SettingsManager.SortListBy));
            ProfileList = new ObservableCollection<ProfileModel>(profiles);

            IsVisible = ProfileList.Count > 0;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if(SettingsManager.ChangeSort && parameters.GetNavigationMode() == NavigationMode.Back)
            {
                List<ProfileModel> profiles = ProfileList.ToList();
                profiles.Sort(new ProfileModelComparer(SettingsManager.SortListBy));
                ProfileList = new ObservableCollection<ProfileModel>(profiles);
                SettingsManager.ChangeSort = false;
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters) { }

        #endregion


        #region Properties

        public ObservableCollection<ProfileModel> ProfileList
        {
            get => profileList;
            set => SetProperty(ref profileList, value);
        }

        public IDialogService DialogService { get; }

        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        #endregion


        #region Commands

        public DelegateCommand LogOutTapCommand => new DelegateCommand(GoLogOutAsync);
        public DelegateCommand AddEditProfileTapCommand => new DelegateCommand(GoAddEditProfileAsync);
        public DelegateCommand SettingsTapCommand => new DelegateCommand(GoSettingsPageAsync);

        public ICommand DeleteTapCommand => new Command(OnDeleteAsync);
        public ICommand UpdateTapCommand => new Command(GoUpdateProfileAsync);
        public ICommand ItemTapCommand => new Command(GoShowItemTapped);

        #endregion


        #region Private helpers

        private async void GoLogOutAsync()
        {
            SettingsManager.LoggedUser = "";
            await NavigationService.NavigateAsync(nameof(UserSignIn));

            NavigationPage page = (NavigationPage)App.Current.MainPage;
            while (page.Navigation.NavigationStack.Count > 1)
                page.Navigation.RemovePage(page.Navigation.NavigationStack[page.Navigation.NavigationStack.Count - 2]);
        }


        private async void GoAddEditProfileAsync() => await NavigationService.NavigateAsync(nameof(AddEditProfileView));


        private async void GoUpdateProfileAsync(object selectedProfile)
        {
            ProfileModel profileModel = selectedProfile as ProfileModel;

            if(profileModel != null)
            {
                NavigationParameters parameter = new NavigationParameters
                {
                    {"profile", profileModel }
                };

                await NavigationService.NavigateAsync(nameof(AddEditProfileView), parameter);
            }
        }


        private async void OnDeleteAsync(object selectedProfile)
        {
            ProfileModel profileModel = selectedProfile as ProfileModel;

            if (profileModel != null)
            {
                ConfirmConfig confirmConfig = new ConfirmConfig
                {
                    Message = Resource.DELETE_PROFILE_QUESTION,
                    OkText = Resource.CONFIRM_OK,
                    CancelText = Resource.CONFIRM_CANCEL
                };

                if (await UserDialogs.Instance.ConfirmAsync(confirmConfig))
                {
                    ProfileList.Remove(ProfileList.First(p => p.Id == profileModel.Id));
                    IsVisible = ProfileList.Count > 0;
                    await DbService.DeleteDataAsync(profileModel);
                }
            }
        }


        private void GoShowItemTapped(object profile)
        {
            ProfileModel profileModel = profile as ProfileModel;

            if (profileModel != null)
                DialogService.ShowDialog("ItemTappedDialog", new DialogParameters
                {
                    {"source", profileModel.ImagePath }
                });
        }


        private async void GoSettingsPageAsync() => await NavigationService.NavigateAsync(nameof(SettingsView));


        #endregion
    }
}