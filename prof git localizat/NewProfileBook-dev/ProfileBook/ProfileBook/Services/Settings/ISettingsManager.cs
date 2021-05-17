using ProfileBook.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProfileBook.Services.Settings
{
    public interface ISettingsManager
    {
        int UserId { get; set; }
        int Sort { get; set; }
        string Culture { get; set; }
    }
}
