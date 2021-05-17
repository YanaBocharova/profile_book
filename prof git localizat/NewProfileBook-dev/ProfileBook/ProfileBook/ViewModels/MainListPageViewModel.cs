using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Enums;
using ProfileBook.Models;
using ProfileBook.Services.Profile;
using ProfileBook.Services.Repository;
using ProfileBook.Services.Settings;
using ProfileBook.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    class MainListPageViewModel : BindableBase, IInitializeAsync
    {
        private INavigationService navigationService;
        private IProfileService profileService;
        private Services.Settings.ISettingsManager settingsManager;

        public MainListPageViewModel(INavigationService navigationService,
                                     IProfileService profileService,
                                     Services.Settings.ISettingsManager settingsManager)
        {
            Title = "Main List";
            this.navigationService = navigationService;
            this.profileService = profileService;
            this.settingsManager = settingsManager;
        }

        #region --- Public Properties ---

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private ObservableCollection<ProfileModel> profileList;
        public ObservableCollection<ProfileModel> ProfileList
        {
            get => profileList;
            set
            {
                SetProperty(ref profileList, value);
            }
        }

        private bool haveNoProfiles;
        public bool HaveNoProfiles
        {
            get => haveNoProfiles;
            set
            {
                SetProperty(ref haveNoProfiles, value);
            }
        }

        public ICommand AddProfileTapCommand => new Command(OnAddProfileTap);
        public ICommand EditProfileTapCommand => new Command<ProfileModel>(OnEditProfileTap);
        public ICommand DeleteProfileTapCommand => new Command<ProfileModel>(OnDeleteProfileTap);
        public ICommand OpenImageTapCommand => new Command<ProfileModel>(OnOpenImageTap);
        public ICommand SettingsTapCommand => new Command(OnSettingsTap);
        public ICommand SignOutTapCommand => new Command(OnSignOutTap);

        #endregion

        #region --- Public Methods ---

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            var profileList = await profileService.SortProfiles((SortOption)settingsManager.Sort);

            ProfileList = new ObservableCollection<ProfileModel>(profileList);

            CheckProfiles();
        }

        #endregion

        #region --- Private Methods ---

        private void CheckProfiles()
        {
            if (profileList.Count > 0)
            {
                HaveNoProfiles = false;
            }
            else
            {
                HaveNoProfiles = true;
            }
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnAddProfileTap()
        {
            await navigationService.NavigateAsync($"{nameof(AddProfilePage)}");
        }

        private async void OnEditProfileTap(ProfileModel profileModel)
        {
            var parameters = new NavigationParameters();
            parameters.Add("profile", profileModel);

            await navigationService.NavigateAsync($"{nameof(AddProfilePage)}", parameters);
        }

        private void OnDeleteProfileTap(ProfileModel profileModel)
        {
            profileService.DeleteProfile(profileModel);
            ProfileList.Remove(profileModel);

            CheckProfiles();
        }

        private async void OnOpenImageTap(ProfileModel profileModel)
        {
            var parameters = new NavigationParameters();
            parameters.Add("profile", profileModel);

            await navigationService.NavigateAsync($"{nameof(ProfileImagePage)}", 
                                                  parameters,
                                                  useModalNavigation: true,
                                                  animated: true);
        }

        private async void OnSettingsTap(object obj)
        {
            await navigationService.NavigateAsync($"{nameof(SettingsPage)}");
        }

        private async void OnSignOutTap()
        {
            settingsManager.UserId = 0;
            await navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInPage)}");
        }

        #endregion

    }
}
