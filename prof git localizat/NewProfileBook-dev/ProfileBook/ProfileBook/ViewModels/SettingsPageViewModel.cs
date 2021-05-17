using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Enums;
using ProfileBook.Resources;
using ProfileBook.Services.Settings;
using ProfileBook.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    class SettingsPageViewModel : BindableBase, IInitialize
    {
        private INavigationService navigationService;
        private ISettingsManager settingsManager;

        public SettingsPageViewModel(INavigationService navigationService,
                                     ISettingsManager settingsManager)
        {
            Title = "Settings";
            this.navigationService = navigationService;
            this.settingsManager = settingsManager;
            
        }

        #region --- Public Properties ---

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private bool isName;
        public bool IsName
        {
            get => isName;
            set => SetProperty(ref isName, value);
        }

        private bool isNickName;
        public bool IsNickName
        {
            get => isNickName;
            set => SetProperty(ref isNickName, value);
        }

        private bool isDate;
        public bool IsDate
        {
            get => isDate;
            set => SetProperty(ref isDate, value);
        }

        private bool isLight;
        public bool IsLight
        {
            get => isLight;
            set => SetProperty(ref isLight, value);
        }

        private bool isDark;
        public bool IsDark
        {
            get => isDark;
            set => SetProperty(ref isDark, value);
        }

        private bool isEnglish;
        public bool IsEnglish
        {
            get => isEnglish;
            set => SetProperty(ref isEnglish, value);
        }

        private bool isRussian;
        public bool IsRussian
        {
            get => isRussian;
            set => SetProperty(ref isRussian, value);
        }

        public ICommand SaveSettingsTapCommand => new Command(OnSaveSettings);

        #endregion

        #region --- Public Methods ---

        public void Initialize(INavigationParameters parameters)
        {
            ActivateSortControls();
            ActivateLanguageControls();
        }

        #endregion

        #region --- Private Methods ---

        private void ActivateSortControls()
        {
            var sortOption = (SortOption)settingsManager.Sort;
            switch (sortOption)
            {
                case SortOption.NickName:
                    IsNickName = true;
                    break;
                case SortOption.Name:
                    IsName = true;
                    break;
                case SortOption.Date:
                    IsDate = true;
                    break;
            }
        }

        private void ActivateLanguageControls()
        {
            LanguageOption language = (LanguageOption)Enum.Parse(typeof(LanguageOption), settingsManager.Culture);

            switch (language)
            {
                case LanguageOption.en:
                    IsEnglish = true;
                    break;
                case LanguageOption.ru:
                    IsRussian = true;
                    break;
            }
        }

        private void SaveSortSettings()
        {
            if (IsNickName == true)
            {
                settingsManager.Sort = (int)SortOption.NickName;
            }
            if (IsName == true)
            {
                settingsManager.Sort = (int)SortOption.Name;
            }
            if (IsDate == true)
            {
                settingsManager.Sort = (int)SortOption.Date;
            }
        }

        private void SaveLanguageSettings()
        {
            if (IsEnglish == true)
            {
                settingsManager.Culture = (LanguageOption.en).ToString();
            }
            if (IsRussian == true)
            {
                settingsManager.Culture = (LanguageOption.ru).ToString();
            }

            var cultureInfo = new CultureInfo(settingsManager.Culture, false);
            Resource.Culture = cultureInfo;
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnSaveSettings()
        {
            SaveSortSettings();
            SaveLanguageSettings();

            await navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListPage)}");
        }

        #endregion

    }
}
