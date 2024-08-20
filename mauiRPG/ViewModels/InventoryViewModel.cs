using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;

namespace mauiRPG.ViewModels
{
    public partial class InventoryViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Item> items;

        [ObservableProperty]
        private Item selectedItem;

        public InventoryViewModel()
        {
            Items = [new HealthPotion()];
        }
        [RelayCommand]
        private void UseItem()
        {
            if (SelectedItem != null)
            {
                // Implement logic to use the item
                // This might involve updating the player's stats, removing the item from inventory, etc.
                Items.Remove(SelectedItem);
            }
        }
        [RelayCommand]
        private void DropItem()
        {
            if (SelectedItem != null)
            {
                Items.Remove(SelectedItem);
            }
        }
    }
}