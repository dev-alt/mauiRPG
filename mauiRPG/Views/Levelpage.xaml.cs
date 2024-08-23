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
    private readonly InventoryService _inventoryService;

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

    public LevelPage(GameStateService gameStateService, CombatService combatService, InventoryService inventoryService)
    {
        InitializeComponent();
        _gameStateService = gameStateService;
        _combatService = combatService;
        _inventoryService = inventoryService;
        _combatView = new CombatView
        {
            IsVisible = false
        };
        MainLayout.Children.Add(_combatView);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine("LevelPage OnAppearing");
        if (_player == null)
        {
            _player = _gameStateService.CurrentPlayer;
            Debug.WriteLine($"Player loaded: {_player?.Name}");
        }
    }

    private void LoadLevel(int levelNumber)
    {
        Debug.WriteLine($"LoadLevel called with level number: {levelNumber}");
        _player = _gameStateService.CurrentPlayer;
        var levelName = $"Level {levelNumber}";
        var imageSource = $"level{levelNumber}.jpg";
        Level level = new(name: levelName, imageSource: imageSource)
        {
            Number = levelNumber,
            IsUnlocked = true,
            Name = levelName,
            ImageSource = imageSource
        };
        BindingContext = new LevelDetailsViewModel(level);

        Dispatcher.DispatchAsync(async () =>
        {
            await Task.Delay(2000);
            SimulateEnemyEncounter();
        });
    }

    private void SimulateEnemyEncounter()
    {
        Debug.WriteLine("SimulateEnemyEncounter called");
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
        Debug.WriteLine("InitiateCombat called");
        var combatViewModel = new CombatViewModel(player, enemy, _combatService, _inventoryService);
        _combatView.SetCombatViewModel(combatViewModel);
        Dispatcher.Dispatch(() =>
        {
            _combatView.IsVisible = true;
            Debug.WriteLine($"CombatView made visible. BindingContext: {_combatView.BindingContext}");
        });

        combatViewModel.CombatEnded += OnCombatEnded;
    }

    private void OnCombatEnded(object? sender, CombatOutcome result)
    {
        Debug.WriteLine($"Combat ended with result: {result}");
        _combatView.IsVisible = false;
        if (result == CombatOutcome.PlayerVictory)
        {
            DisplayAlert("Victory!", "You've defeated the enemy!", "Continue");
        }
        else
        {
            DisplayAlert("Defeat", "You've been defeated. Try again?", "OK");
        }
        // Unsubscribe from the event
        if (sender is CombatViewModel viewModel)
        {
            viewModel.CombatEnded -= OnCombatEnded;
        }
    }
}