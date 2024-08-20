using System.Diagnostics;
using mauiRPG.Models;
using mauiRPG.ViewModels;
using mauiRPG.Services;

namespace mauiRPG.Views;
[QueryProperty(nameof(LevelNumber), "levelNumber")]
public partial class LevelPage : ContentPage
{
    private int _levelNumber;
    private readonly CombatView _combatView;
    private readonly CombatService _combatService;
    private readonly GameStateService _gameStateService;
    private Player _player = null!;

    public int LevelNumber
    {
        get => _levelNumber;
        set
        {
            _levelNumber = value;
            OnPropertyChanged();
            LoadLevel(_levelNumber);
        }
    }

    public LevelPage(GameStateService gameStateService)
    {
        InitializeComponent();
        _gameStateService = gameStateService;
        _combatService = new CombatService();
        _combatView = new CombatView();
        _combatView.IsVisible = false;
        Debug.WriteLine($"LevelPage constructor: Player: {_player}, CombatService: {_combatService}");
        MainLayout.Children.Add(_combatView);
    }

    private void LoadLevel(int levelNumber)
    {
        _player = _gameStateService.CurrentPlayer;
        var levelName = $"Level {levelNumber}";
        var imageSource = $"level{levelNumber}.jpg";
        Level level = new Level(name: levelName, imageSource: imageSource)
        {
            Number = levelNumber,
            IsUnlocked = true,
            Name = levelName,
            ImageSource = imageSource
        };
        BindingContext = new LevelDetailsViewModel(level);

        // Simulate enemy encounter after a short delay
        Dispatcher.DispatchAsync(async () =>
        {
            await Task.Delay(2000);
            SimulateEnemyEncounter();
        });
    }

    private void SimulateEnemyEncounter()
    {
        Debug.WriteLine($"Player: {_player}");
        var enemy = new EnemyWizard($"Evil Wizard {LevelNumber}", LevelNumber)
        {
            Name = "Dark Wizard",
            Description = "A powerful dark wizard",
            Health = 50
        };
        Debug.WriteLine($"Enemy: {enemy}");
        Debug.WriteLine($"CombatService: {_combatService}");
        InitiateCombat(_player, enemy);
    }

    private void InitiateCombat(Player player, Enemy enemy)
    {
        var combatViewModel = new CombatViewModel(player, enemy, _combatService);
        _combatView.BindingContext = combatViewModel;
        _combatView.IsVisible = true;

        // Subscribe to combat end event
        combatViewModel.CombatEnded += OnCombatEnded;
    }

    private void OnCombatEnded(object sender, CombatOutcome result)
    {
        _combatView.IsVisible = false;

        if (result == CombatOutcome.PlayerVictory)
        {
            // Handle player victory (e.g., show rewards, progress to next level)
            DisplayAlert("Victory!", "You've defeated the enemy!", "Continue");
        }
        else
        {
            // Handle player defeat
            DisplayAlert("Defeat", "You've been defeated. Try again?", "OK");
            // Optionally, navigate back to level select or restart the level
        }

        // Unsubscribe from the event
        if (sender is CombatViewModel viewModel)
        {
            viewModel.CombatEnded -= OnCombatEnded;
        }
    }
}
