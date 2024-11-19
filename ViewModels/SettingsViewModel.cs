using MauiContactManager.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiContactManager.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly IContactDatabase _contactDatabase;
        private string _selectedTheme;

        public event PropertyChangedEventHandler PropertyChanged;

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    _contactDatabase.SaveSetting("AppTheme", value);
                    ApplyTheme(value);
                    OnPropertyChanged(nameof(SelectedTheme));
                }
            }
        }

        public bool PushNotificationsEnabled
        {
            get => Preferences.Get("PushNotificationsEnabled", true);
            set => Preferences.Set("PushNotificationsEnabled", value);
        }

        public ICommand ExportContactsCommand { get; }
        public ICommand ImportContactsCommand { get; }

        public SettingsViewModel(IContactDatabase contactDatabase)
        {
            _contactDatabase = contactDatabase;

            // Инициализация темы
            _selectedTheme = _contactDatabase.GetSetting("AppTheme") ?? "System";
            ApplyTheme(_selectedTheme);

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

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExportContacts()
        {
            // Todo
        }

        private void ImportContacts()
        {
            // Todo
        }
    }
}
