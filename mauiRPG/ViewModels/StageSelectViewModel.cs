using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace mauiRPG.ViewModels
{
    public partial class StageSelectViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;
        private readonly ILogger<PlayerInfoPopup> _logger;
        private readonly Page _currentPage;

        [ObservableProperty]
        private Player? _currentPlayer;

        [ObservableProperty]
        private ObservableCollection<Level> _levels = [];

        public StageSelectViewModel(GameStateService gameStateService, ILogger<PlayerInfoPopup> logger, Page currentPage)
        {
            _gameStateService = gameStateService;
            _logger = logger;
            _currentPage = currentPage;
            _logger.LogInformation("StageSelectViewModel constructor called");
            _logger.LogInformation("GameStateService is {GameStateServiceStatus}", "not null");
            LoadData();
        }

        [RelayCommand]
        public void LoadData()
        {
            CurrentPlayer = _gameStateService.CurrentPlayer;
            _logger.LogInformation("CurrentPlayer is {CurrentPlayerStatus}", $"set to {CurrentPlayer?.Name ?? "null"}");
            InitializeLevels();
        }

        private void InitializeLevels()
        {
            _logger.LogInformation("InitializeLevels called");
            Levels =
            [
                new("The Beginning", "level1.jpg")
                    { Number = 1, IsUnlocked = true, Name = "The Beginning", ImageSource = "level1.jpg" },
                new("Dark Forest", "level2.png")
                    { Number = 2, IsUnlocked = true, Name = "Dark Forest", ImageSource = "level2.jpg" },
                new("Mystic Mountains", "level3.png")
                    { Number = 3, IsUnlocked = false, Name = "Mystic Mountains", ImageSource = "level3.png" }
            ];
            _logger.LogInformation("Levels initialized with {LevelCount} levels", Levels.Count);
        }

        [RelayCommand]
        private async Task ViewPlayerInfo()
        {
            _logger.LogInformation("ViewPlayerInfo called");
            _logger.LogInformation("CurrentPlayer: Name={Name}, Race={Race}",
                CurrentPlayer?.Name ?? "null",
                CurrentPlayer?.Race?.Name ?? "null");
            try
            {
                var popup = new PlayerInfoPopup(CurrentPlayer!, _logger);
                await _currentPage.ShowPopupAsync(popup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating or showing PlayerInfoPopup");
                await ShowErrorPopup("Error", "Unable to display player information");
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
                await ShowErrorPopup("Locked", $"Level {level.Number} is locked!");
            }
        }

        [RelayCommand]
        private async Task GoToQuestBoard()
        {
            try
            {
                await Shell.Current.GoToAsync("///QuestBoard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error navigating to Quest Board");
                await ShowErrorPopup("Error", "Unable to navigate to the Quest Board. Please try again.");
            }
        }

        private async Task ShowErrorPopup(string title, string message)
        {
            var errorPopup = new ErrorPopup($"{title}\n\n{message}");
            await _currentPage.ShowPopupAsync(errorPopup);
        }
    }
}