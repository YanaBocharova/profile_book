using ProfileBook.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProfileBook.Services.Settings
{
    public class SettingsManager : ISettingsManager
    {
        public int UserId
        {
            get => Preferences.Get(nameof(UserId), 0);
            set => Preferences.Set(nameof(UserId), value);
        }
        public int Sort 
        {
            get => Preferences.Get(nameof(Sort), (int)SortOption.Date);
            set => Preferences.Set(nameof(Sort), value);
        }
        public string Culture
        {
            get => Preferences.Get(nameof(Culture), (LanguageOption.ru).ToString());
            set => Preferences.Set(nameof(Culture), value);
        }
    }
}
