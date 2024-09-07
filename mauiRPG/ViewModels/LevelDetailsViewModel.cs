using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;

namespace mauiRPG.ViewModels
{
    public partial class LevelDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private Level _level = null!;

        [ObservableProperty]
        private string _enemyName = string.Empty;

        [ObservableProperty]
        private double _enemyHealthPercentage;

        [ObservableProperty]
        private string _enemyHealth = string.Empty;

        [ObservableProperty]
        private string _playerName = string.Empty;

        [ObservableProperty]
        private double _playerHealthPercentage;

        [ObservableProperty]
        private string _playerHealth = string.Empty;

        public string Name => Level?.Name ?? string.Empty;
        public string ImageSource => Level?.ImageSource ?? string.Empty;

        public LevelDetailsViewModel(Level level)
        {
            Level = level;
        }

        [RelayCommand]
        private static void Attack()
        {
            // Implement attack logic
        }

        [RelayCommand]
        private static void UseItem()
        {
            // Implement use item logic
        }

        [RelayCommand]
        private static void Defend()
        {
            // Implement defend logic
        }

        [RelayCommand]
        private static void Run()
        {
            // Implement run logic
        }
    }
}