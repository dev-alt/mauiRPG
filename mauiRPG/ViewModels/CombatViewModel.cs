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
    private readonly ICombatService _combatService;
    private readonly GameStateService _gameStateService;
    private readonly InventoryService _inventoryService;
    private readonly Random _random = new();

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

    public CombatViewModel(ICombatService combatService, InventoryService inventoryService, Player player, CombatantModel enemy, GameStateService gameStateService)
    {
        _combatService = combatService;
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
        CombatLog.Add(new CombatLogEntryModel
        {
            Message = $"{Player.Name} used {item.Name}.",
            IsPlayerAction = true
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
        CombatLog.Add(new CombatLogEntryModel
        {
            Message = result.Message ?? $"{result.Attacker} dealt {result.Damage} damage to {result.Defender}. {result.Defender}'s remaining health: {result.RemainingHealth}",
            IsPlayerAction = result.Attacker == Player.Name
        });
    }
    private async Task PrepareNextBattle()
    {
        IsLoading = true;
        await Task.Delay(3000); // Simulating preparation time

        BattleCount++;

        // Generate a new enemy with increasing difficulty
        Enemy = GenerateNewEnemy();

        // Reset combat log
        CombatLog.Clear();
        CombatLog.Add(new CombatLogEntryModel
        {
            Message = $"A new enemy appears: {Enemy.Name}!",
            IsPlayerAction = false
        });

        // Heal the player a bit between battles
        int healAmount = (int)(Player.MaxHealth * 0.2); // Heal 20% of max health
        Player.Heal(healAmount);
        CombatLog.Add(new CombatLogEntryModel
        {
            Message = $"{Player.Name} recovers {healAmount} HP!",
            IsPlayerAction = true
        });

        // Reset player's defending state
        Player.IsDefending = false;

        IsLoading = false;
    }
    private CombatantModel GenerateNewEnemy()
    {
        string[] enemyTypes = ["Goblin", "Orc", "Troll", "Dark Elf", "Dragon"];
        string enemyName = enemyTypes[_random.Next(enemyTypes.Length)];

        int baseHealth = 50 + (BattleCount * 10); // Health increases with each battle
        int healthVariation = _random.Next(-10, 11); // Add some randomness

        return new CombatantModel
        {
            Name = $"{enemyName} Lvl {BattleCount}",
            MaxHealth = baseHealth + healthVariation,
            CurrentHealth = baseHealth + healthVariation,
            Attack = 5 + (BattleCount * 2), // Attack increases with each battle
            Defense = 3 + BattleCount // Defense increases with each battle
        };
    }
    private bool IsCombatOver() => Player.CurrentHealth <= 0 || Enemy.CurrentHealth <= 0;

    private async void CheckCombatEnd()
    {
        if (IsCombatOver())
        {
            CombatResult = GetCombatResult();
            if (Player.CurrentHealth > 0)
            {
                await PrepareNextBattle();
            }
            else
            {
                // Game over logic here
                CombatLog.Add(new CombatLogEntryModel
                {
                    Message = "Game Over! You have been defeated.",
                    IsPlayerAction = false
                });
            }
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