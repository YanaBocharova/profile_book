using Prism.Commands;
using Prism.Navigation;
using ProfileBook.Services.DbService;
using ProfileBook.Services.Settings;
using ProfileBook.Themes;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using ProfileBook.Resx;

namespace ProfileBook.ViewModels
{
    public class SettingsViewModel : ViewModelBase, INavigatedAware
    {
        public SettingsViewModel(INavigationService navigationService, IDbService dbService, ISettingsManagerService settingsManager) : base(navigationService, dbService, settingsManager)
        {
            Title = Resource.SettingsTitlePage;
        }


        #region Private fields

        private object sortSelection;
        private string oldSortSelection;
        private bool isToogled;

        #endregion


        #region Properties

        public object SortSelection
        {
            get => sortSelection;
            set => SetProperty(ref sortSelection, value);
        }

        public bool IsToogled
        {
            get => isToogled;
            set => SetProperty(ref isToogled, value);
        }

        #endregion


        #region Commands

        public DelegateCommand SwitchTapCommand => new DelegateCommand(OnChangeTheme);

        #endregion


        #region Overrides

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            string sortList = SettingsManager.SortListBy;
            if (!string.IsNullOrEmpty(sortList))
            {
                switch (sortList)
                {
                    case "Name":
                        SortSelection = "Name";
                        break;
                    case "NickName":
                        SortSelection = "NickName";
                        break;
                    case "Date creation":
                        SortSelection = "Date creation";
                        break;
                }
                oldSortSelection = sortList;
            }

            IsToogled = SettingsManager.DarkTheme;
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsToogled))
                OnChangeTheme();
        }

        #endregion


        #region Implement interface

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            if(oldSortSelection != SortSelection.ToString())
            {
                SettingsManager.SortListBy = SortSelection.ToString();
                SettingsManager.ChangeSort = true;
            }

            SettingsManager.DarkTheme = IsToogled;
        }

        public void OnNavigatedTo(INavigationParameters parameters) { }

        #endregion


        #region Private helpers

        private void OnChangeTheme()
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if(mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                switch (IsToogled)
                {
                    case true:
                        mergedDictionaries.Add(new DarkTheme());
                        break;
                    case false:
                        mergedDictionaries.Add(new LightTheme());
                        break;
                }
            }
        }

        #endregion
    }
}