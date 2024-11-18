using System.Windows.Input;

namespace MauiContactManager.ViewModels
{
    public class SettingsViewModel
    {
        public List<string> AvailableThemes { get; } = new List<string> { "Light", "Dark", "System" };
        public string SelectedTheme
        {
            get => Preferences.Get("AppTheme", "System");
            set
            {
                Preferences.Set("AppTheme", value);
                ApplyTheme(value);
            }
        }

        public bool PushNotificationsEnabled
        {
            get => Preferences.Get("PushNotificationsEnabled", true);
            set => Preferences.Set("PushNotificationsEnabled", value);
        }

        public ICommand ExportContactsCommand { get; }
        public ICommand ImportContactsCommand { get; }

        public SettingsViewModel()
        {
            ExportContactsCommand = new Command(ExportContacts);
            ImportContactsCommand = new Command(ImportContacts);
        }

        private void ApplyTheme(string theme)
        {
            Application.Current.UserAppTheme = theme switch
            {
                "Light" => AppTheme.Light,
                "Dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified
            };
        }

        private void ExportContacts()
        {
            //Todo
        }

        private void ImportContacts()
        {
            //Todo
        }
    }
}
