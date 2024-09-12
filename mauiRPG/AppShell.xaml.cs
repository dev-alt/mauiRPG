using mauiRPG.ViewModels;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;

namespace mauiRPG
{
    public partial class AppShell : Shell
    {
        private readonly ILogger<AppShell> _logger;

        public AppShell(AppShellViewModel viewModel, ILogger<AppShell> logger)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel;
                _logger = logger;

                RegisterRoutes();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing AppShell");
                throw;
            }
        }

        private void RegisterRoutes()
        {
            _logger.LogInformation("Registering routes");

            var routes = new Dictionary<string, Type>
            {
                { nameof(MainMenuView), typeof(MainMenuView) },
                { nameof(LevelSelectView), typeof(LevelSelectView) },
                { nameof(CharacterSelect), typeof(CharacterSelect) },
                { nameof(QuestBoardView), typeof(QuestBoardView) },
                { nameof(LevelPage), typeof(LevelPage) }
            };

            foreach (var route in routes)
            {
                Routing.RegisterRoute(route.Key, route.Value);
                _logger.LogDebug("Registered route: {RouteName}", route.Key);
            }
        }
    }
}