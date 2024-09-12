using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;

namespace mauiRPG.ViewModels
{

    public partial class LevelPageViewModel(
        GameStateService gameStateService,
        CombatManagerService combatManagerService,
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
            var currentPlayer = gameStateService.CurrentPlayer;
            if (currentPlayer != null)
            {
                var enemy = combatManagerService.GenerateNewEnemy(CurrentLevel.Number);
                InitiateCombat(currentPlayer, enemy);
            }
        }


        private void InitiateCombat(Player player, EnemyModel enemy)
        {
            CombatViewModel = new CombatViewModel(combatManagerService, inventoryService, player, enemy, gameStateService);
            CombatViewModel.CombatEnded += OnCombatEnded;
            IsCombatViewVisible = true;
        }

        private async void OnCombatEnded(object? sender, CombatViewModel.CombatOutcome result)
        {
            IsCombatViewVisible = false;
            await Task.Delay(1000);

            if (result == CombatViewModel.CombatOutcome.PlayerVictory)
            {
                SimulateEnemyEncounter();
            }
            else if (result == CombatViewModel.CombatOutcome.EnemyVictory)
            {
                // Handle player defeat
                IsCombatViewVisible = false;
            }
        }
    }
}
