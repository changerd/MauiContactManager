using MauiContactManager.Interfaces;

namespace MauiContactManager
{
    public partial class App : Application
    {
        private readonly IContactDatabase _contactDatabase;
        private readonly INavigationService _navigationService;

        public App(IServiceProvider serviceProvider, IContactDatabase contactDatabase, INavigationService navigationService)
        {
            InitializeComponent();

            _contactDatabase = contactDatabase;
            _navigationService = navigationService;

            ApplySavedTheme();

            var mainPage = serviceProvider.GetService<MainPage>();
            MainPage = new NavigationPage(mainPage);
        }

        private void ApplySavedTheme()
        {            
            string savedTheme = _contactDatabase.GetSetting("AppTheme") ?? "System";
            ApplyTheme(savedTheme);
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
    }
}
