namespace ProfileBook.Services.Settings
{
    public interface ISettingsManagerService
    {
        string LoggedUser { get; set; }
        string SortListBy { get; set; }
        bool ChangeSort { get; set; }
        bool DarkTheme { get; set; }
    }
}