using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;

namespace mauiRPG.ViewModels
{
    public partial class LevelPageViewModel(
        GameStateService gameStateService, CombatManagerService combatManagerService,
        InventoryService inventoryService)
        : ObservableObject
    {
        [ObservableProperty]
        private Level? _currentLevel;

        [ObservableProperty]
        private bool _isCombatViewVisible;

        [ObservableProperty]
        private CombatViewModel? _combatViewModel;
        
        [RelayCommand]
        private void LoadLevel(int levelNumber)
        {
            // Load the level data based on the level number
            CurrentLevel = new Level($"Level {levelNumber}", $"level{levelNumber}.jpg")
            {
                Number = levelNumber,
                IsUnlocked = true,
                Name = $"Level {levelNumber}",
                ImageSource = $"level{levelNumber}.jpg" 
            };

            SimulateEnemyEncounter();
        }
        private void SimulateEnemyEncounter()
        {
            if (CurrentLevel == null) return;

            var enemy = new CombatantModel
            {
                Name = $"Dark Wizard {CurrentLevel.Number}",
                MaxHealth = 50,
            };

            InitiateCombat(gameStateService.CurrentPlayer, enemy);
        }

        private void InitiateCombat(Player player, CombatantModel enemy)
        {
            CombatViewModel = new CombatViewModel(combatManagerService, inventoryService, player, enemy, gameStateService);
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
