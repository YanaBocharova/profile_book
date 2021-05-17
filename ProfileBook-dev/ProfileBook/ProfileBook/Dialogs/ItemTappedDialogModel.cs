using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace ProfileBook.Dialogs
{
    public class ItemTappedDialogModel : BindableBase, IDialogAware
    {
        public event Action<IDialogParameters> RequestClose;

        public ItemTappedDialogModel()
        {
            CloseCommand = new DelegateCommand(() => RequestClose(null));
        }

        #region Commands

        public DelegateCommand CloseCommand { get; }

        #endregion


        #region Properties

        private string pathToSourceImageProfile;
        public string PathToSourceImageProfile 
        {
            get => pathToSourceImageProfile;
            private set => SetProperty(ref pathToSourceImageProfile, value); 
        }

        #endregion


        #region Public methods

        public bool CanCloseDialog() => true;     

        public void OnDialogClosed() { }


        public void OnDialogOpened(IDialogParameters parameters) => PathToSourceImageProfile = parameters.GetValue<string>("source");

        #endregion

    }
}