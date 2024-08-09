using mauiRPG.Services;
using mauiRPG.ViewModels;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;

namespace mauiRPG
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
            builder.Services.AddSingleton<GameStateService>();
            builder.Services.AddSingleton<CharacterService>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();

            builder.Services.AddTransient<CharacterSelect>();
            builder.Services.AddTransient<CharacterCreationViewModel>();
            builder.Services.AddTransient<LevelSelectView>();
            builder.Services.AddTransient<LevelPage>();
            builder.Services.AddTransient<StageSelectViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
        #endif

            return builder.Build();
        }
    }
}
