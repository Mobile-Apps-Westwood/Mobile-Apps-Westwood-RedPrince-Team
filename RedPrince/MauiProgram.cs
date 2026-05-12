using Microsoft.Extensions.Logging;
using RedPrince.Services;

namespace RedPrince
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register the database service
            builder.Services.AddSingleton<DatabaseService>();

            builder.Services.AddTransient<Views.MainPage>();
            builder.Services.AddTransient<ViewModels.MainViewModel>();

            builder.Services.AddTransient<Views.CreateAccountPage>();
            builder.Services.AddTransient<ViewModels.CreateAccountViewModel>();

            builder.Services.AddTransient<Views.StorePage>();
            builder.Services.AddTransient<ViewModels.StoreViewModel>();

            builder.Services.AddTransient<Views.BlackJackPage>();
            builder.Services.AddTransient<ViewModels.GameViewModel>();

            builder.Services.AddTransient<Views.BaccaratPage>();
            builder.Services.AddTransient<ViewModels.BaccaratViewModel>();

            builder.Services.AddTransient<Views.LeaderboardPage>();
            builder.Services.AddTransient<ViewModels.LeaderboardViewModel>();

            builder.Services.AddTransient<Views.SettingsPage>();
            builder.Services.AddTransient<ViewModels.SettingsViewModel>();

            builder.Services.AddTransient<Views.SettingsChangeUserPage>();
            builder.Services.AddTransient<ViewModels.SettingsChangeUserViewModel>();

            builder.Services.AddTransient<Views.SettingsChangePassPage>();
            builder.Services.AddTransient<ViewModels.SettingsChangePassViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
