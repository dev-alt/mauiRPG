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
        private readonly InventoryService _inventoryService;

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

        [ObservableProperty]
        private bool isInventoryOpen;

        public ObservableCollection<Item> InventoryItems { get; } = [];

        public double PlayerHealthPercentage => (double)PlayerHealth / _player.MaxHealth;
        public double EnemyHealthPercentage => (double)EnemyHealth / _enemy.MaxHealth;

        public event EventHandler<CombatOutcome>? CombatEnded;
        public event EventHandler<string>? AnimationRequested;

        public CombatViewModel(Player player, Enemy enemy, CombatService combatService, InventoryService inventoryService)
        {
            _player = player;
            _enemy = enemy;
            _combatService = combatService;
            _inventoryService = inventoryService;
            PlayerName = player.Name;
            EnemyName = enemy.Name;
            PlayerHealth = player.Health;
            EnemyHealth = (int)enemy.Health;

            LoadInventory();
        }

        private void LoadInventory()
        {
            var items = _inventoryService.GetPlayerItems(_player.Id);
            foreach (var item in items)
            {
                InventoryItems.Add(item);
            }
        }

        [RelayCommand]
        private async Task Attack()
        {
            AnimationRequested?.Invoke(this, "PlayerAttack");

            var playerResult = _combatService.ExecutePlayerAttack(_player, _enemy);
            UpdateCombatLog(playerResult);
            EnemyHealth = (int)_enemy.Health;

            OnPropertyChanged(nameof(EnemyHealthPercentage));

            if (_enemy.Health <= 0)
            {
                CombatEnded?.Invoke(this, CombatOutcome.PlayerVictory);
                return;
            }

            await Task.Delay(1000);

            AnimationRequested?.Invoke(this, "EnemyAttack");

            var enemyResult = _combatService.ExecuteEnemyAttack(_enemy, _player);
            UpdateCombatLog(enemyResult);
            PlayerHealth = _player.Health;

            OnPropertyChanged(nameof(PlayerHealthPercentage));

            if (_player.Health <= 0)
            {
                CombatEnded?.Invoke(this, CombatOutcome.EnemyVictory);
            }
        }

        [RelayCommand]
        private void Defend()
        {
            CombatLog += "You took a defensive stance.\n";
        }

        [RelayCommand]
        private void OpenInventory()
        {
            IsInventoryOpen = true;
        }

        [RelayCommand]
        private void CloseInventory()
        {
            IsInventoryOpen = false;
        }

        [RelayCommand]
        private void UseItem(Item item)
        {
            item.Use(_player);
            InventoryItems.Remove(item);
            _inventoryService.RemoveItem(_player.Id, item.Id);
            PlayerHealth = _player.Health;
            UpdateCombatLog(new CombatResult { Attacker = _player.Name, Defender = _player.Name, Damage = 0, RemainingHealth = _player.Health });
            CloseInventory();
        }

        [RelayCommand]
        private void Run()
        {
            CombatLog += "You attempted to run away.\n";
        }

        private void UpdateCombatLog(CombatResult result)
        {
            CombatLog += $"{result.Attacker} dealt {result.Damage} damage to {result.Defender}. {result.Defender}'s remaining health: {result.RemainingHealth}\n";
        }
    }
}