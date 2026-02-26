using CommunityToolkit.Maui;
using LoLStatsMaui.Repositories;
using LoLStatsMaui.ViewModels;
using LoLStatsMaui.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LoLStatsMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if DEBUG
            builder.Logging.AddDebug();
            builder.Configuration.AddUserSecrets<App>();
#endif
            var app = builder.Build();
            AppConfig.Initialize(app.Services.GetService<IConfiguration>());

            return app;
        }
    }
}
