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
        private int _levelNumber;

        [ObservableProperty]
        private LevelDetailsViewModel _levelDetailsViewModel;

        [ObservableProperty]
        private bool _isCombatViewVisible;

        [ObservableProperty]
        private CombatViewModel _combatViewModel;

        partial void OnLevelNumberChanged(int value)
        {
            LoadLevel(value);
        }

        private void LoadLevel(int levelNumber)
        {
            var levelName = $"Level {levelNumber}";
            var imageSource = $"level{levelNumber}.jpg";
            Level level = new(name: levelName, imageSource: imageSource)
            {
                Number = levelNumber,
                IsUnlocked = true,
                Name = levelName,
                ImageSource = imageSource
            };
            LevelDetailsViewModel = new LevelDetailsViewModel(level);

            SimulateEnemyEncounter();
        }

        private void SimulateEnemyEncounter()
        {
            var enemy = new EnemyWizard($"Evil Wizard {LevelNumber}", LevelNumber)
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
            IsCombatViewVisible = true;
            CombatViewModel.CombatEnded += OnCombatEnded;
        }

        private void OnCombatEnded(object? sender, CombatOutcome result)
        {
            IsCombatViewVisible = false;
            // Handle combat result (e.g., show alert, update player stats, etc.)
        }
    }
}