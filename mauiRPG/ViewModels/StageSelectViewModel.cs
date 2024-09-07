using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;

namespace mauiRPG.ViewModels
{
    public partial class StageSelectViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;
        private readonly ILogger<PlayerInfoPopup> _logger;

        [ObservableProperty]
        private Player _currentPlayer = null!;

        [ObservableProperty]
        private ObservableCollection<Level> _levels = [];

        public StageSelectViewModel(GameStateService gameStateService, ILogger<PlayerInfoPopup> logger)
        {
            _gameStateService = gameStateService;
            _logger = logger;

            _logger.LogInformation("StageSelectViewModel constructor called");
            _logger.LogInformation("GameStateService is {GameStateServiceStatus}", "not null");

            LoadData();
        }

        [RelayCommand]
        public void LoadData()
        {
            CurrentPlayer = _gameStateService.CurrentPlayer;
            _logger.LogInformation("CurrentPlayer is {CurrentPlayerStatus}", $"set to {CurrentPlayer.Name}");
            InitializeLevels();
        }

        private void InitializeLevels()
        {
            _logger.LogInformation("InitializeLevels called");
            Levels =
            [
                new Level("The Beginning", "level1.jpg")
                {
                    Number = 1,
                    IsUnlocked = true,
                    Name = "The Beginning",
                    ImageSource = "level1.jpg"
                },
                new Level("Dark Forest", "level2.png")
                {
                    Number = 2,
                    IsUnlocked = true,
                    Name = "Dark Forest",
                    ImageSource = "level2.jpg"
                },
                new Level("Mystic Mountains", "level3.png")
                {
                    Number = 3,
                    IsUnlocked = false,
                    Name = "Mystic Mountains",
                    ImageSource = "level3.png"
                }
            ];
            _logger.LogInformation("Levels initialized with {LevelCount} levels", Levels.Count);
        }

        [RelayCommand]
        private async Task ViewPlayerInfo()
        {
            _logger.LogInformation("ViewPlayerInfo called");

            _logger.LogInformation("CurrentPlayer: Name={Name}, Race={Race}, Class={Class}",
                CurrentPlayer.Name,
                CurrentPlayer.Race?.Name,
                CurrentPlayer.Class?.Name);

            try
            {
                var popup = new PlayerInfoPopup(CurrentPlayer, _logger);
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.ShowPopupAsync(popup);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating or showing PlayerInfoPopup");
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        "Unable to display player information",
                        "OK");
                }
            }
        }

        [RelayCommand]
        private async Task SelectLevel(Level level)
        {
            _logger.LogInformation("SelectLevel called with level: {LevelName}", level.Name);
            if (level.IsUnlocked)
            {
                await Shell.Current.GoToAsync($"{nameof(LevelPage)}?levelNumber={level.Number}");
            }
            else
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Locked", $"Level {level.Number} is locked!", "OK");
                }
            }
        }
    }
}