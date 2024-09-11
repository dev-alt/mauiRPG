using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mauiRPG.Models
{
    public partial class Character : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private int _maxHealth;

        [ObservableProperty]
        private int _currentHealth;

        [ObservableProperty]
        private int _attack;

        [ObservableProperty]
        private int _defense;

        [ObservableProperty]
        private double _healthPercentage;

        [ObservableProperty]
        private int _strength;

        [ObservableProperty]
        private int _intelligence;

        [ObservableProperty]
        private int _dexterity;

        [ObservableProperty]
        private int _constitution;

        [ObservableProperty]
        private int _level = 1;

        partial void OnCurrentHealthChanged(int value)
        {
            HealthPercentage = (double)value / MaxHealth;
        }

        public void TakeDamage(int amount)
        {
            CurrentHealth = Math.Max(0, CurrentHealth - amount);
        }

        public void Heal(int amount)
        {
            CurrentHealth = Math.Min(MaxHealth, CurrentHealth + amount);
        }
    }
}