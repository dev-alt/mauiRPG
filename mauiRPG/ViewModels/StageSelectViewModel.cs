using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;

namespace mauiRPG.ViewModels
{
    public class StageSelectViewModel : INotifyPropertyChanged
    {
        private readonly GameStateService _gameStateService;
        private readonly ILogger<PlayerInfoPopup> _logger;
        private Player _currentPlayer = null!;
        private ObservableCollection<Level> _levels;
        public ICommand SelectLevelCommand { get; private set; }
        public ICommand ViewPlayerInfoCommand { get; private set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Level> Levels
        {
            get => _levels;
            set
            {
                _levels = value;
                OnPropertyChanged(nameof(Levels));
            }
        }
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }

        public StageSelectViewModel(GameStateService gameStateService, ILogger<PlayerInfoPopup> logger)
        {
            _gameStateService = gameStateService;
            _logger = logger;

            _logger.LogInformation("StageSelectViewModel constructor called");
            _logger.LogInformation("GameStateService is {GameStateServiceStatus}", "not null");

            CurrentPlayer = _gameStateService.CurrentPlayer;
            _logger.LogInformation("CurrentPlayer is {CurrentPlayerStatus}", $"set to {CurrentPlayer.Name}");

            _levels = new ObservableCollection<Level>();
            InitializeLevels();
            SelectLevelCommand = new Command<Level>(OnLevelSelected);
            ViewPlayerInfoCommand = new Command(OnViewPlayerInfo);
        }

        private void InitializeLevels()
        {
            _logger.LogInformation("InitializeLevels called");
            Levels = new ObservableCollection<Level>
            {
                new() { Number = 1, Name = "The Beginning", IsUnlocked = true, ImageSource = "level1.jpg" },
                new() { Number = 2, Name = "Dark Forest", IsUnlocked = true, ImageSource = "level2.png" },
                new() { Number = 3, Name = "Mystic Mountains", IsUnlocked = false, ImageSource = "level3.png" },
            };
            _logger.LogInformation("Levels initialized with {LevelCount} levels", Levels.Count);
        }

        private async void OnViewPlayerInfo()
        {
            _logger.LogInformation("OnViewPlayerInfo called");

            if (_gameStateService.CurrentPlayer != null)
            {
                _logger.LogInformation("CurrentPlayer: Name={Name}, Race={Race}, Class={Class}",
                    _gameStateService.CurrentPlayer.Name,
                    _gameStateService.CurrentPlayer.Race?.Name,
                    _gameStateService.CurrentPlayer.Class?.Name);

                try
                {
                    var popup = new PlayerInfoPopup(_gameStateService.CurrentPlayer, _logger);
                    if (Application.Current == null) return;
                    if (Application.Current.MainPage != null)
                        await Application.Current.MainPage.ShowPopupAsync(popup);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating or showing PlayerInfoPopup");
                    if (Application.Current != null)
                        if (Application.Current.MainPage != null)
                            await Application.Current.MainPage.DisplayAlert("Error",
                                "Unable to display player information",
                                "OK");
                }
            }
            else
            {
                _logger.LogWarning("CurrentPlayer is null");
                if (Application.Current == null) return;
                if (Application.Current.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Error", "No player information available",
                        "OK");
            }
        }


        private async void OnLevelSelected(Level level)
        {
            _logger.LogInformation("OnLevelSelected called with level: {LevelName}", level.Name);
            if (level.IsUnlocked)
            {
                await Shell.Current.GoToAsync($"{nameof(LevelPage)}?levelNumber={level.Number}");
            }
            else
            {
                await Application.Current?.MainPage?.DisplayAlert("Locked", $"Level {level.Number} is locked!", "OK")!;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}