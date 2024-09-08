using CommunityToolkit.Maui.Views;
using mauiRPG.Models;
using mauiRPG.ViewModels;
using Microsoft.Extensions.Logging;

public class CombatViewModel : BaseViewModel
{
    private readonly ICombatService _combatService;
    private readonly IGameStateService _gameStateService;
    private readonly ILogger<CombatViewModel> _logger;

    public Player Player => _gameStateService.CurrentPlayer;
    public Enemy CurrentEnemy { get; private set; }

    private string _combatLog;
    public string CombatLog
    {
        get => _combatLog;
        set => SetProperty(ref _combatLog, value);
    }

    public Command AttackCommand { get; }
    public Command DefendCommand { get; }
    public Command RunCommand { get; }

    public CombatViewModel(ICombatService combatService, IGameStateService gameStateService, ILogger<CombatViewModel> logger)
    {
        _combatService = combatService;
        _gameStateService = gameStateService;
        _logger = logger;

        AttackCommand = new Command(ExecuteAttack);
        DefendCommand = new Command(ExecuteDefend);
        RunCommand = new Command(ExecuteRun);

        InitializeCombat();
    }

    private void InitializeCombat()
    {
        CurrentEnemy = _combatService.GenerateEnemy(Player.Level);
        CombatLog = $"A wild {CurrentEnemy.Name} appears!";
    }

    private void ExecuteAttack()
    {
        try
        {
            var (playerDamage, enemyDamage) = _combatService.ExecutePlayerAttack(Player, CurrentEnemy);
            
            UpdateCombatLog($"You attack {CurrentEnemy.Name} for {playerDamage} damage!");
            
            if (CurrentEnemy.CurrentHP <= 0)
            {
                HandleEnemyDefeat();
            }
            else
            {
                UpdateCombatLog($"{CurrentEnemy.Name} counterattacks for {enemyDamage} damage!");
                
                if (Player.CurrentHP <= 0)
                {
                    HandlePlayerDefeat();
                }
            }

            OnPropertyChanged(nameof(Player));
            OnPropertyChanged(nameof(CurrentEnemy));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during attack execution");
            UpdateCombatLog("An error occurred during the attack.");
        }
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