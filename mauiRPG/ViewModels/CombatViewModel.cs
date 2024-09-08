using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;

namespace mauiRPG.ViewModels;

public partial class CombatViewModel : ObservableObject
{
    private readonly ICombatService _combatService;
    private readonly GameStateService _gameStateService;
    private readonly InventoryService _inventoryService;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private Player _player;

    [ObservableProperty]
    private CombatantModel _enemy;

    [ObservableProperty]
    private string _combatResult = string.Empty;

    private readonly ObservableCollection<CombatLogEntryModel> _combatLog = [];
    public ReadOnlyObservableCollection<CombatLogEntryModel> CombatLog { get; }

    private readonly ObservableCollection<Item> _inventoryItems = [];
    public ReadOnlyObservableCollection<Item> InventoryItems { get; }

    public event EventHandler<CombatOutcome>? CombatEnded;

    public CombatViewModel(ICombatService combatService, InventoryService inventoryService,
        INavigationService _navigation, Player player, CombatantModel enemy)
    {
        _combatService = combatService;
        _inventoryService = inventoryService;
        _navigation = _navigation;
        Player = player;
        Enemy = enemy;
        CombatLog = new ReadOnlyObservableCollection<CombatLogEntryModel>(_combatLog);
        InventoryItems = new ReadOnlyObservableCollection<Item>(_inventoryItems);

        LoadPlayerInventory();
    }


    private void LoadPlayerInventory()
    {
        var items = _inventoryService.GetPlayerItems(Player.Id);  // Use InventoryService
        foreach (var item in items)
        {
            _inventoryItems.Add(item);
        }
    }


    [RelayCommand]
    private async Task OpenInventoryAsync()
    {
        var inventoryViewModel = new InventoryViewModel(_gameStateService);
        var inventoryPopup = new InventoryPopup(inventoryViewModel);

    }


    [RelayCommand]
    private void Attack() => ExecuteTurn();

    [RelayCommand]
    private void Defend() => ExecuteDefend();

    [RelayCommand]
    private void UseItem(Item item)
    {
        item.Use(Player);
        _combatLog.Add(new CombatLogEntryModel
        {
            Message = $"{Player.Name} used {item.Name}.",
            IsPlayerAction = true
        });

        if (item.Type == ItemType.Consumable)
        {
            _inventoryItems.Remove(item);
        }

        CheckCombatEnd();
    }

    [RelayCommand]
    private void Run() => ExecuteRun();

    private void ExecuteTurn()
    {
        var playerResult = _combatService.ExecutePlayerAttack(Player, Enemy);
        UpdateCombatLog(playerResult);

        if (!IsCombatOver())
        {
            var enemyResult = _combatService.ExecuteEnemyAttack(Enemy, Player);
            UpdateCombatLog(enemyResult);
        }

        CheckCombatEnd();
    }

    private void ExecuteDefend()
    {
        Player.IsDefending = true;
        UpdateCombatLog(new CombatResult
        {
            Attacker = Player.Name,
            Defender = Player.Name,
            Message = $"{Player.Name} takes a defensive stance.",
            Damage = 0,
            RemainingHealth = Player.CurrentHealth
        });

        var enemyResult = _combatService.ExecuteEnemyAttack(Enemy, Player);
        UpdateCombatLog(enemyResult);

        Player.IsDefending = false;
        CheckCombatEnd();
    }

    private void ExecuteRun()
    {
        bool escaped = _combatService.AttemptEscape();
        if (escaped)
        {
            UpdateCombatLog(new CombatResult
            {
                Attacker = Player.Name,
                Defender = Enemy.Name,
                Message = $"{Player.Name} successfully escaped from the battle!",
                Damage = 0,
                RemainingHealth = Player.CurrentHealth
            });
            CombatEnded?.Invoke(this, CombatOutcome.PlayerEscaped);
        }
        else
        {
            UpdateCombatLog(new CombatResult
            {
                Attacker = Player.Name,
                Defender = Enemy.Name,
                Message = $"{Player.Name} failed to escape!",
                Damage = 0,
                RemainingHealth = Player.CurrentHealth
            });
            var enemyResult = _combatService.ExecuteEnemyAttack(Enemy, Player);
            UpdateCombatLog(enemyResult);
            CheckCombatEnd();
        }
    }

    private void UpdateCombatLog(CombatResult result)
    {
        _combatLog.Add(new CombatLogEntryModel
        {
            Message = result.Message ?? $"{result.Attacker} dealt {result.Damage} damage to {result.Defender}. {result.Defender}'s remaining health: {result.RemainingHealth}",
            IsPlayerAction = result.Attacker == Player.Name
        });
    }

    private bool IsCombatOver() => Player.CurrentHealth <= 0 || Enemy.CurrentHealth <= 0;

    private void CheckCombatEnd()
    {
        if (IsCombatOver())
        {
            CombatResult = GetCombatResult();
            CombatEnded?.Invoke(this, Player.CurrentHealth > 0 ? CombatOutcome.PlayerVictory : CombatOutcome.EnemyVictory);
        }
    }

    private string GetCombatResult()
    {
        if (Player.CurrentHealth <= 0)
            return $"{Player.Name} has been defeated. Game Over!";
        else if (Enemy.CurrentHealth <= 0)
            return $"{Enemy.Name} has been defeated. {Player.Name} is victorious!";
        return "Combat is still ongoing.";
    }

    [RelayCommand]
    private void ResetCombat()
    {
        Player.CurrentHealth = Player.MaxHealth;
        Enemy.CurrentHealth = Enemy.MaxHealth;
        _combatLog.Clear();
        CombatResult = string.Empty;
    }

    public enum CombatOutcome
    {
        PlayerVictory,
        EnemyVictory,
        PlayerEscaped
    }
}