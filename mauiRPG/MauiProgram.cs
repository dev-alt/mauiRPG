using mauiRPG.Services;
using mauiRPG.ViewModels;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace mauiRPG
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
            builder.Services.AddSingleton<GameStateService>();
            builder.Services.AddSingleton<CharacterService>();
            builder.Services.AddSingleton<CombatService>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<InventoryService>();
            builder.Services.AddSingleton<LevelUpService>();

            builder.Services.AddTransient<MainMenuView>();
            builder.Services.AddTransient<CharacterSelect>();
            builder.Services.AddTransient<CharacterCreationViewModel>();
            builder.Services.AddTransient<LevelSelectView>();
            builder.Services.AddTransient<LevelPage>();
            builder.Services.AddTransient<StageSelectViewModel>();
            builder.Services.AddTransient<InventoryView>();
            builder.Services.AddTransient<InventoryViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
        #endif

            return builder.Build();
        }
    }
}
