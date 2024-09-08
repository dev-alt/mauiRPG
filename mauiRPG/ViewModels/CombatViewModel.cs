using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;

namespace mauiRPG.ViewModels;

public partial class CombatViewModel : ObservableObject
{
    private readonly CombatManagerService _combatManager;
    private readonly GameStateService _gameStateService;
    private readonly InventoryService _inventoryService;

    [ObservableProperty]
    private int _battleCount;

    [ObservableProperty]
    private Player _player;

    [ObservableProperty]
    private CombatantModel _enemy;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _combatResult = string.Empty;

    [ObservableProperty]
    private ObservableCollection<CombatLogEntryModel> _combatLog;

    [ObservableProperty]
    private ObservableCollection<Item> _inventoryItems;

    public event EventHandler<CombatOutcome>? CombatEnded;

    public CombatViewModel(CombatManagerService combatManager, InventoryService inventoryService, Player player, CombatantModel enemy, GameStateService gameStateService)
    {
        _combatManager = combatManager;
        _inventoryService = inventoryService;
        _gameStateService = gameStateService;
        Player = player;
        Enemy = enemy;
        CombatLog = [];
        InventoryItems = [];
        BattleCount = 1;
        LoadPlayerInventory();
    }

    private void LoadPlayerInventory()
    {
        var items = _inventoryService.GetPlayerItems(Player.Id);
        foreach (var item in items)
        {
            InventoryItems.Add(item);
        }
    }

    [RelayCommand]
    private async Task OpenInventoryAsync()
    {
        var inventoryViewModel = new InventoryViewModel(_gameStateService);
        var inventoryPopup = new InventoryPopup(inventoryViewModel);
        await Shell.Current.ShowPopupAsync(inventoryPopup);
    }

    [RelayCommand]
    private void Attack() => ExecuteTurn();

    [RelayCommand]
    private void Defend() => ExecuteDefend();

    [RelayCommand]
    private void UseItem(Item item)
    {
        item.Use(Player);
        UpdateCombatLog(new CombatResult
        {
            Attacker = Player.Name,
            Defender = Player.Name,
            Message = $"{Player.Name} used {item.Name}.",
            Damage = 0,
            RemainingHealth = Player.CurrentHealth
        });

        if (item.Type == ItemType.Consumable)
        {
            InventoryItems.Remove(item);
        }

        CheckCombatEnd();
    }

    [RelayCommand]
    private void Run() => ExecuteRun();

    private void ExecuteTurn()
    {
        var playerResult = _combatManager.ExecutePlayerTurn(Player, Enemy);
        UpdateCombatLog(playerResult);

        if (!CombatManagerService.IsCombatOver(Player, Enemy))
        {
            var enemyResult = _combatManager.ExecuteEnemyTurn(Enemy, Player);
            UpdateCombatLog(enemyResult);
        }

        CheckCombatEnd();
    }

    private void ExecuteDefend()
    {
        var result = _combatManager.ExecutePlayerDefend(Player, Enemy);
        UpdateCombatLog(result);
        CheckCombatEnd();
    }

    private void ExecuteRun()
    {
        bool escaped = _combatManager.AttemptEscape();
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
            var enemyResult = _combatManager.ExecuteEnemyTurn(Enemy, Player);
            UpdateCombatLog(enemyResult);
            CheckCombatEnd();
        }
    }

    private void UpdateCombatLog(CombatResult result)
    {
        CombatLog.Add(new CombatLogEntryModel
        {
            Message = result.Message ?? $"{result.Attacker} dealt {result.Damage} damage to {result.Defender}. {result.Defender}'s remaining health: {result.RemainingHealth}",
            IsPlayerAction = result.Attacker == Player.Name
        });
    }

    private async Task PrepareNextBattle()
    {
        IsLoading = true;
        BattleCount++;
        Enemy = await _combatManager.PrepareNextBattle(Player, BattleCount);

        CombatLog.Clear();
        CombatLog.Add(new CombatLogEntryModel
        {
            Message = $"A new enemy appears: {Enemy.Name}!",
            IsPlayerAction = false
        });
        CombatLog.Add(new CombatLogEntryModel
        {
            Message = $"{Player.Name} recovers some HP!",
            IsPlayerAction = true
        });

        IsLoading = false;
    }

    private async void CheckCombatEnd()
    {
        if (CombatManagerService.IsCombatOver(Player, Enemy))
        {
            CombatResult = CombatManagerService.GetCombatResult(Player, Enemy);
            if (Player.CurrentHealth > 0)
            {
                await PrepareNextBattle();
            }
            else
            {
                CombatLog.Add(new CombatLogEntryModel
                {
                    Message = "Game Over! You have been defeated.",
                    IsPlayerAction = false
                });
            }
            CombatEnded?.Invoke(this, Player.CurrentHealth > 0 ? CombatOutcome.PlayerVictory : CombatOutcome.EnemyVictory);
        }
    }

    [RelayCommand]
    private void ResetCombat()
    {
        Player.CurrentHealth = Player.MaxHealth;
        Enemy.CurrentHealth = Enemy.MaxHealth;
        CombatLog.Clear();
        CombatResult = string.Empty;
    }

    public enum CombatOutcome
    {
        PlayerVictory,
        EnemyVictory,
        PlayerEscaped
    }
}