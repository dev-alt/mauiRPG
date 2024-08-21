using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;

namespace mauiRPG.ViewModels
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;

        [ObservableProperty]
        private ObservableCollection<Item> items;

        [ObservableProperty]
        private Item? selectedItem;
        public InventoryViewModel(GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            Items =
            [
                new HealthPotion
                {
                    Name = "Health Potion",
                    Description = "Restores 50 HP",
                    Value = 30
                }
            ];
        }
        [RelayCommand]
        private void UseItem()
        {
            if (SelectedItem == null || _gameStateService.CurrentPlayer == null)
            {
                return;
            }

            Player player = _gameStateService.CurrentPlayer;

            switch (SelectedItem)
            {
                case HealthPotion healthPotion:
                    InventoryViewModel.UseHealthPotion(player, healthPotion);
                    break;
                case Equipment equipment:
                    EquipItem(player, equipment);
                    break;
                default:
                    // For other item types or quest items
                    SelectedItem.Use(player);
                    break;
            }

            // Remove the item from inventory after use
            Items.Remove(SelectedItem);

            // Notify of changes
            OnPropertyChanged(nameof(Items));
            SelectedItem = null;
        }
        private static void UseHealthPotion(Player player, HealthPotion potion)
        {
            int healAmount = Math.Min(potion.HealAmount, player.MaxHealth - player.Health);
            player.Health += healAmount;
        }

        private void EquipItem(Player player, Equipment equipment)
        {
            if (player.EquippedItems.Remove(equipment.Slot, out var existingItem))
            {
                Items.Add(existingItem);
            }

            player.EquippedItems[equipment.Slot] = equipment;

            player.UpdateStats();
        }

        [RelayCommand]
        private void DropItem()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
                SelectedItem = null;
            }
        }
    }
}