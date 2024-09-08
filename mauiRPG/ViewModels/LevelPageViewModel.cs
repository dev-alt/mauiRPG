using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;

namespace mauiRPG.ViewModels
{
    public partial class LevelPageViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;
        private readonly ICombatService _combatService;
        private readonly InventoryService _inventoryService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private Level? _currentLevel;

        [ObservableProperty]
        private bool _isCombatViewVisible;

        [ObservableProperty]
        private CombatViewModel? _combatViewModel;

        public LevelPageViewModel(GameStateService gameStateService, ICombatService combatService,
            InventoryService inventoryService, INavigationService navigationService)
        {
            _gameStateService = gameStateService;
            _combatService = combatService;
            _inventoryService = inventoryService;
            _navigationService = navigationService;
        }

        private void SimulateEnemyEncounter()
        {
            if (CurrentLevel == null) return;

            var enemy = new CombatantModel
            {
                Name = $"Dark Wizard {CurrentLevel.Number}",
                MaxHealth = 50,
            };

            InitiateCombat(_gameStateService.CurrentPlayer, enemy);
        }

        private void InitiateCombat(Player player, CombatantModel enemy)
        {
            CombatViewModel = new CombatViewModel(_combatService, _inventoryService, _navigationService, player, enemy);
            CombatViewModel.CombatEnded += OnCombatEnded;
            IsCombatViewVisible = true;
        }

        private void OnCombatEnded(object? sender, CombatViewModel.CombatOutcome result)
        {
            IsCombatViewVisible = false;

            if (CombatViewModel != null)
            {
                CombatViewModel.CombatEnded -= OnCombatEnded;
            }
        }
    }
}
