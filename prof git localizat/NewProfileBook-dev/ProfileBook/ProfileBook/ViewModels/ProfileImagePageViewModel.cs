using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    class ProfileImagePageViewModel : BindableBase, INavigationAware
    {
        private INavigationService navigationService;

        public ProfileImagePageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        #region --- Public Properties ---

        private string image;
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        #endregion

        #region --- Public Methods ---

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var profile = parameters.GetValue<ProfileModel>("profile");
            if (profile != null)
            {
                Image = profile.Image;
            }
        }

        #endregion

    }
}
