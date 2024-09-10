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
                    fonts.AddFont("MedievalSharp-Regular.ttf", "MedievalSharp");
                });

            // Register services
            builder.Services.AddSingleton<GameStateService>();
            builder.Services.AddSingleton<CharacterService>();
            builder.Services.AddTransient<ICombatService, CombatService>();
            builder.Services.AddSingleton<InventoryService>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();

            // Add CombatManagerService
            builder.Services.AddTransient<CombatManagerService>();

            // Register ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<CharacterCreationViewModel>();
            builder.Services.AddTransient<StageSelectViewModel>();
            builder.Services.AddTransient<LevelPageViewModel>();
            builder.Services.AddTransient<CombatViewModel>();
            builder.Services.AddTransient<InventoryViewModel>();

            // Register Views
            builder.Services.AddTransient<MainMenuView>();
            builder.Services.AddTransient<CharacterSelect>();
            builder.Services.AddTransient<LevelSelectView>();
            builder.Services.AddTransient<LevelPage>();

            builder.Logging.AddDebug();

            return builder.Build();
        }
    }
}
