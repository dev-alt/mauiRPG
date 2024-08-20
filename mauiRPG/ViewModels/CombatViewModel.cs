using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using System.Collections.ObjectModel;

namespace mauiRPG.ViewModels
{
    public partial class CombatViewModel : ObservableObject
    {
        private readonly CombatService _combatService;
        private readonly Player _player;
        private readonly Enemy _enemy;

        [ObservableProperty]
        private string playerName;

        [ObservableProperty]
        private string enemyName;

        [ObservableProperty]
        private int playerHealth;

        [ObservableProperty]
        private int enemyHealth;

        [ObservableProperty]
        private string combatLog = string.Empty;

        public double PlayerHealthPercentage => (double)PlayerHealth / _player.MaxHealth;
        public double EnemyHealthPercentage => (double)EnemyHealth / _enemy.MaxHealth;

        public event EventHandler<CombatOutcome>? CombatEnded;

        public CombatViewModel(Player player, Enemy enemy, CombatService combatService)
        {
            _player = player;
            _enemy = enemy;
            _combatService = combatService;
            PlayerName = player.Name;
            EnemyName = enemy.Name;
            PlayerHealth = player.Health;
            EnemyHealth = (int)enemy.Health;
        }


        [RelayCommand]
        private void Attack()
        {
            var playerResult = _combatService.ExecutePlayerAttack(_player, _enemy);
            UpdateCombatLog(playerResult);
            EnemyHealth = (int)_enemy.Health;

            if (_enemy.Health <= 0)
            {
                CombatEnded?.Invoke(this, CombatOutcome.PlayerVictory);
                return;
            }

            var enemyResult = _combatService.ExecuteEnemyAttack(_enemy, _player);
            UpdateCombatLog(enemyResult);
            PlayerHealth = _player.Health;

            if (_player.Health <= 0)
            {
                CombatEnded?.Invoke(this, CombatOutcome.EnemyVictory);
            }
        }
        [RelayCommand]
        private void Defend()
        {
            // Implement defend logic
            CombatLog += "You took a defensive stance.\n";
        }

        [RelayCommand]
        private void UseItem()
        {
            // Implement item usage logic
            CombatLog += "You used an item.\n";
        }

        [RelayCommand]
        private void Run()
        {
            // Implement run logic
            CombatLog += "You attempted to run away.\n";
        }

        private void UpdateCombatLog(CombatResult result)
        {
            CombatLog += $"{result.Attacker} dealt {result.Damage} damage to {result.Defender}. {result.Defender}'s remaining health: {result.RemainingHealth}\n";
        }
    }
}