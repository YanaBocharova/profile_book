using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Services.DbService;
using ProfileBook.Services.Settings;

namespace ProfileBook.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize
    {
        protected INavigationService NavigationService { get; }
        protected IDbService DbService { get; }
        protected ISettingsManagerService SettingsManager { get; }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public ViewModelBase(INavigationService navigationService, IDbService dbService, ISettingsManagerService settingsManager = null)
        {
            NavigationService = navigationService;
            SettingsManager = settingsManager;
            DbService = dbService;
        }

        public virtual void Initialize(INavigationParameters parameters) { }
    }
}