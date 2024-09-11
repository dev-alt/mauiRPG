using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using System.Collections.ObjectModel;

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

    [ObservableProperty]
    private bool _isDefeated;

    [ObservableProperty]
    private string _defeatMessage = string.Empty;

    [ObservableProperty]
    private bool _isBattleActionVisible;

    [ObservableProperty]
    private string _currentBattleAction = string.Empty;

    public event EventHandler<CombatOutcome>? CombatEnded;

    private CombatView? _combatView;

    public void SetCombatView(CombatView view)
    {
        _combatView = view;
    }

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

        Player.CurrentHealth = Player.MaxHealth;
        Enemy.CurrentHealth = Enemy.MaxHealth;

        LoadPlayerInventory();
        InitializeCombatLog();
    }

    private void LoadPlayerInventory()
    {
        var items = _inventoryService.GetPlayerItems(Player.Id);
        foreach (var item in items)
        {
            InventoryItems.Add(item);
        }
    }

    private void InitializeCombatLog()
    {
        CombatLog.Add(new CombatLogEntryModel { Message = $"Combat begins! {Player.Name} vs {Enemy.Name}", IsPlayerAction = false });
        CombatLog.Add(new CombatLogEntryModel { Message = $"{Player.Name} HP: {Player.CurrentHealth}/{Player.MaxHealth}", IsPlayerAction = true });
        CombatLog.Add(new CombatLogEntryModel { Message = $"{Enemy.Name} HP: {Enemy.CurrentHealth}/{Enemy.MaxHealth}", IsPlayerAction = false });
    }

    [RelayCommand]
    private async Task OpenInventoryAsync()
    {
        var inventoryViewModel = new InventoryViewModel(_gameStateService);
        var inventoryPopup = new InventoryPopup(inventoryViewModel);
        await Shell.Current.ShowPopupAsync(inventoryPopup);
    }

    [RelayCommand]
    private async Task AttackAsync()
    {
        if (!IsCombatOver())
        {
            await ShowBattleAction("Attack");
            var playerResult = _combatManager.ExecutePlayerTurn(Player, Enemy);
            await UpdateCombatLog(playerResult);

            if (!IsCombatOver())
            {
                await ShowBattleAction("Enemy Attack");
                var enemyResult = _combatManager.ExecuteEnemyTurn(Enemy, Player);
                await UpdateCombatLog(enemyResult);
            }

            await CheckCombatEndAsync();
        }
    }

    [RelayCommand]
    private async Task DefendAsync()
    {
        if (!IsCombatOver())
        {
            await ShowBattleAction("Defend");
            Player.IsDefending = true;
            await UpdateCombatLog(new CombatResult
            {
                Attacker = Player.Name,
                Defender = Player.Name,
                Message = $"{Player.Name} takes a defensive stance.",
                Damage = 0,
                RemainingHealth = Player.CurrentHealth
            });

            await ShowBattleAction("Enemy Attack");
            var enemyResult = _combatManager.ExecuteEnemyTurn(Enemy, Player);
            await UpdateCombatLog(enemyResult);

            Player.IsDefending = false;
            await CheckCombatEndAsync();
        }
    }

    [RelayCommand]
    private async Task UseItemAsync(Item item)
    {
        if (!IsCombatOver())
        {
            await ShowBattleAction($"Use {item.Name}");
            int previousHealth = Player.CurrentHealth;
            item.Use(Player);
            int healthRestored = Player.CurrentHealth - previousHealth;

            await UpdateCombatLog(new CombatResult
            {
                Attacker = Player.Name,
                Defender = Player.Name,
                Message = $"{Player.Name} used {item.Name} and restored {healthRestored} HP.",
                Damage = -healthRestored,
                RemainingHealth = Player.CurrentHealth
            });

            if (item.Type == ItemType.Consumable)
            {
                InventoryItems.Remove(item);
            }

            await ShowBattleAction("Enemy Attack");
            var enemyResult = _combatManager.ExecuteEnemyTurn(Enemy, Player);
            await UpdateCombatLog(enemyResult);
            await CheckCombatEndAsync();
        }
    }

    [RelayCommand]
    private async Task RunAsync()
    {
        if (!IsCombatOver())
        {
            await ShowBattleAction("Attempt Escape");
            bool escaped = _combatManager.AttemptEscape();
            if (escaped)
            {
                await UpdateCombatLog(new CombatResult
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
                await UpdateCombatLog(new CombatResult
                {
                    Attacker = Player.Name,
                    Defender = Enemy.Name,
                    Message = $"{Player.Name} failed to escape!",
                    Damage = 0,
                    RemainingHealth = Player.CurrentHealth
                });
                await ShowBattleAction("Enemy Attack");
                var enemyResult = _combatManager.ExecuteEnemyTurn(Enemy, Player);
                await UpdateCombatLog(enemyResult);
                await CheckCombatEndAsync();
            }
        }
    }

    private async Task UpdateCombatLog(CombatResult result)
    {
        CombatLog.Add(new CombatLogEntryModel
        {
            Message = result.Message ?? $"{result.Attacker} dealt {result.Damage} damage to {result.Defender}. {result.Defender}'s remaining health: {result.RemainingHealth}",
            IsPlayerAction = result.Attacker == Player.Name
        });

        // Show splash damage
        if (_combatView != null)
        {
            if (result.Defender == Player.Name)
            {
                await _combatView.ShowPlayerDamage(result.Damage);
                await _combatView.ShowPlayerBattleZoneDamage(result.Damage);
            }
            else
            {
                await _combatView.ShowEnemyDamage(result.Damage);
                await _combatView.ShowEnemyBattleZoneDamage(result.Damage);
            }
        }

        OnPropertyChanged(nameof(Player));
        OnPropertyChanged(nameof(Enemy));
    }

    private async Task ShowBattleAction(string action)
    {
        CurrentBattleAction = action;
        IsBattleActionVisible = true;
        await Task.Delay(1000);
        IsBattleActionVisible = false;
    }

    private async Task PrepareNextBattle()
    {
        IsLoading = true;
        BattleCount++;

        Enemy = await _combatManager.PrepareNextBattle(Player, BattleCount);

        CombatLog.Clear();
        InitializeCombatLog();

        IsLoading = false;

        CombatLog.Add(new CombatLogEntryModel { Message = $"Preparing for battle {BattleCount}!", IsPlayerAction = false });

        CombatEnded?.Invoke(this, CombatOutcome.PlayerVictory);

        CombatResult = string.Empty;

        OnPropertyChanged(nameof(Enemy));
    }

    private bool IsCombatOver()
    {
        return CombatManagerService.IsCombatOver(Player, Enemy);
    }

    private async Task CheckCombatEndAsync()
    {
        if (CombatManagerService.IsCombatOver(Player, Enemy))
        {
            CombatResult = CombatManagerService.GetCombatResult(Player, Enemy);
            CombatLog.Add(new CombatLogEntryModel { Message = CombatResult, IsPlayerAction = false });

            if (Player.CurrentHealth > 0)
            {
                int experienceGained = Enemy.Level * 10;
                Player.GainExperience(experienceGained);
                CombatLog.Add(new CombatLogEntryModel { Message = $"{Player.Name} gained {experienceGained} experience!", IsPlayerAction = true });

                await PrepareNextBattle();
            }
            else
            {
                IsDefeated = true;
                DefeatMessage = $"{Player.Name} has been defeated. Game Over!";
                CombatEnded?.Invoke(this, CombatOutcome.EnemyVictory);
            }
        }
    }

    [RelayCommand]
    private static async Task ReturnToHomeScreen()
    {
        await Shell.Current.GoToAsync("//MainMenu");
    }

    [RelayCommand]
    private void ResetCombat()
    {
        Player.CurrentHealth = Player.MaxHealth;
        Enemy.CurrentHealth = Enemy.MaxHealth;
        CombatLog.Clear();
        InitializeCombatLog();
        CombatResult = string.Empty;
    }

    public enum CombatOutcome
    {
        PlayerVictory,
        EnemyVictory,
        PlayerEscaped,
        ContinueToNextBattle
    }

    private void ExecuteDefend()
    {
        // TODO: Implement defend logic
    }

    private void ExecuteRun()
    {
        // TODO: Implement run logic
    }

    private void HandleEnemyDefeat()
    {
        UpdateCombatLog($"You defeated {CurrentEnemy.Name}!");
        // TODO: Award experience and loot
        // TODO: End combat or generate new enemy
    }

    private void HandlePlayerDefeat()
    {
        UpdateCombatLog("You have been defeated!");
        // TODO: Handle player defeat (game over, respawn, etc.)
    }

    private void UpdateCombatLog(string message)
    {
        CombatLog += $"\n{message}";
    }
}