using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;

namespace mauiRPG.ViewModels
{
    public partial class LevelPageViewModel(
        GameStateService gameStateService,
        CombatService combatService,
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
            var levelName = $"Level {levelNumber}";
            var imageSource = $"level{levelNumber}.jpg";
            CurrentLevel = new Level(name: levelName, imageSource: imageSource)
            {
                Number = levelNumber,
                IsUnlocked = true,
                Name = levelName,
                ImageSource = imageSource
            };

            SimulateEnemyEncounter();
        }

        private void SimulateEnemyEncounter()
        {
            if (CurrentLevel == null) return;

            var enemy = new EnemyWizard($"Evil Wizard {CurrentLevel.Number}", CurrentLevel.Number)
            {
                Name = "Dark Wizard",
                Description = "A powerful dark wizard",
                Health = 50
            };

            InitiateCombat(gameStateService.CurrentPlayer, enemy);
        }

        private void InitiateCombat(Player player, Enemy enemy)
        {
            CombatViewModel = new CombatViewModel(player, enemy, combatService, inventoryService);
            CombatViewModel.CombatEnded += OnCombatEnded;
            IsCombatViewVisible = true;
        }

        private void OnCombatEnded(object? sender, CombatOutcome result)
        {
            IsCombatViewVisible = false;
            // Handle combat result (e.g., show alert, update player stats, etc.)
            if (CombatViewModel != null)
            {
                CombatViewModel.CombatEnded -= OnCombatEnded;
            }
        }
    }
}