using System;
using System.ComponentModel;
using System.Windows.Input;
using mauiRPG.Models;

namespace mauiRPG.ViewModels
{
    public class LevelDetailsViewModel : INotifyPropertyChanged
    {
        private Level _level = null!;
        private string _enemyName = string.Empty;
        private double _enemyHealthPercentage;
        private string _enemyHealth = string.Empty;
        private string _playerName = string.Empty;
        private double _playerHealthPercentage;
        private string _playerHealth = string.Empty;

        public Level Level
        {
            get => _level;
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }

        public string Name => Level?.Name ?? string.Empty;
        public string ImageSource => Level?.ImageSource ?? string.Empty;

        public string EnemyName
        {
            get => _enemyName;
            set
            {
                _enemyName = value;
                OnPropertyChanged(nameof(EnemyName));
            }
        }

        public double EnemyHealthPercentage
        {
            get => _enemyHealthPercentage;
            set
            {
                _enemyHealthPercentage = value;
                OnPropertyChanged(nameof(EnemyHealthPercentage));
            }
        }

        public string EnemyHealth
        {
            get => _enemyHealth;
            set
            {
                _enemyHealth = value;
                OnPropertyChanged(nameof(EnemyHealth));
            }
        }

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                OnPropertyChanged(nameof(PlayerName));
            }
        }

        public double PlayerHealthPercentage
        {
            get => _playerHealthPercentage;
            set
            {
                _playerHealthPercentage = value;
                OnPropertyChanged(nameof(PlayerHealthPercentage));
            }
        }

        public string PlayerHealth
        {
            get => _playerHealth;
            set
            {
                _playerHealth = value;
                OnPropertyChanged(nameof(PlayerHealth));
            }
        }

        public ICommand AttackCommand { get; }
        public ICommand UseItemCommand { get; }
        public ICommand DefendCommand { get; }
        public ICommand RunCommand { get; }

        public LevelDetailsViewModel(Level level)
        {
            Level = level;

            // Initialize commands
            AttackCommand = new Command(Attack);
            UseItemCommand = new Command(UseItem);
            DefendCommand = new Command(Defend);
            RunCommand = new Command(Run);
        }

        private void Attack()
        {
            // Implement attack logic
        }

        private void UseItem()
        {
            // Implement use item logic
        }

        private void Defend()
        {
            // Implement defend logic
        }

        private void Run()
        {
            // Implement run logic
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}