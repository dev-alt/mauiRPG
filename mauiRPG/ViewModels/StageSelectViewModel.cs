using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;

namespace mauiRPG.ViewModels;

public class StageSelectViewModel
{
    private readonly GameStateService _gameStateService;
    private Player _currentPlayer;
    public event PropertyChangedEventHandler? PropertyChanged;
    private ObservableCollection<Level> _levels;
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
    public ICommand SelectLevelCommand { get; private set; }
    public ICommand ViewPlayerInfoCommand { get; private set; }
    public StageSelectViewModel(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
        CurrentPlayer = _gameStateService.CurrentPlayer;
        _levels = new ObservableCollection<Level>();
        InitializeLevels();
        SelectLevelCommand = new Command<Level>(OnLevelSelected);
        ViewPlayerInfoCommand = new Command(OnViewPlayerInfo);
    }

    private void InitializeLevels()
    {
        Levels = new ObservableCollection<Level>
        {
            new() { Number = 1, Name = "The Beginning", IsUnlocked = true, ImageSource = "level1.png" },
            new() { Number = 2, Name = "Dark Forest", IsUnlocked = true, ImageSource = "level2.png" },
            new() { Number = 3, Name = "Mystic Mountains", IsUnlocked = false, ImageSource = "level3.png" },
        };
    }
    private async void OnViewPlayerInfo()
    {
        Console.WriteLine("OnViewPlayerInfo called");
        if (CurrentPlayer != null)
        {
            try
            {
                var playerJson = System.Text.Json.JsonSerializer.Serialize(CurrentPlayer);
                await Shell.Current.GoToAsync($"{nameof(PlayerInfoView)}?CurrentPlayer={Uri.EscapeDataString(playerJson)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing player: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Unable to view player information", "OK");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No player information available", "OK");
        }
    }

    private async void OnLevelSelected(Level level)
    {
        Console.WriteLine($"OnLevelSelected called with level: {level.Name}");
        if (level.IsUnlocked)
        {
            await Shell.Current.GoToAsync($"{nameof(LevelPage)}?levelNumber={level.Number}");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Locked", $"Level {level.Number} is locked!", "OK");
        }
    }


    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}