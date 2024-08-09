using System.ComponentModel;
using System.Windows.Input;
using mauiRPG.Models;
using mauiRPG.Services;

namespace mauiRPG.ViewModels
{
    public class CombatViewModel : INotifyPropertyChanged
    {
        private readonly CombatService _combatService;
        private readonly Player _player;
        private readonly Enemy _enemy;
        public event EventHandler<CombatResult>? CombatEnded;

        public string PlayerName => _player.Name;
        public string EnemyName => _enemy.Name;

        public int PlayerHealth
        {
            get => _player.Health;
            set
            {
                _player.Health = value;
                OnPropertyChanged(nameof(PlayerHealth));
                OnPropertyChanged(nameof(PlayerHealthPercentage));
            }
        }

        public int EnemyHealth
        {
            get => (int)_enemy.Health;
            set
            {
                _enemy.Health = value;
                OnPropertyChanged(nameof(EnemyHealth));
                OnPropertyChanged(nameof(EnemyHealthPercentage));
            }
        }

        public double PlayerHealthPercentage => (double)PlayerHealth / _player.MaxHealth;
        public double EnemyHealthPercentage => _enemy.Health / _enemy.MaxHealth;

        public ICommand AttackCommand { get; }
        public ICommand DefendCommand { get; }
        public ICommand UseItemCommand { get; }

        public CombatViewModel(Player player, Enemy enemy, CombatService combatService)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _enemy = enemy ?? throw new ArgumentNullException(nameof(enemy));
            _combatService = combatService ?? throw new ArgumentNullException(nameof(combatService));

            AttackCommand = new Command(ExecuteAttack);
            DefendCommand = new Command(ExecuteDefend);
            UseItemCommand = new Command(ExecuteUseItem);
        }

        private void ExecuteAttack()
        {
            // Implement attack logic
            // Update health values
            // Check for battle end conditions
            CheckCombatEnd();
        }

        private void ExecuteDefend()
        {
            // Implement defend logic
        }

        private void ExecuteUseItem()
        {
            // Implement item usage logic
        }
        private void CheckCombatEnd()
        {
            if (PlayerHealth <= 0)
            {
                CombatEnded?.Invoke(this, CombatResult.EnemyVictory);
            }
            else if (EnemyHealth <= 0)
            {
                CombatEnded?.Invoke(this, CombatResult.PlayerVictory);
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public enum CombatResult
        {
            PlayerVictory,
            EnemyVictory
        }
    }
}