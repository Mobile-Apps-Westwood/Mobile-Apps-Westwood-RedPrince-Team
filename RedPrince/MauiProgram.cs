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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
