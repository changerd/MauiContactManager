using MauiContactManager.Data;
using MauiContactManager.Interfaces;
using MauiContactManager.ViewModels;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace MauiContactManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Batteries_V2.Init();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "contacts.db");
            builder.Services.AddSingleton<IContactDatabase, ContactDatabase>(sp => new ContactDatabase(dbPath));

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<ContactFormViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<ContactDetailsViewModel>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ContactFormPage>();
            builder.Services.AddTransient<ContactDetailsPage>();
            builder.Services.AddTransient<SettingsPage>();

            return builder.Build();
        }
    }
}
