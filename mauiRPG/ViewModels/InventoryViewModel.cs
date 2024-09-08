using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using System.Collections.ObjectModel;

namespace mauiRPG.ViewModels
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;

        [ObservableProperty]
        private  ObservableCollection<Item> _inventoryItems;

        [ObservableProperty]
        private Item? _selectedItem;

        public event EventHandler? CloseRequested;

        public InventoryViewModel(GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            _inventoryItems = _gameStateService.CurrentPlayer.Inventory;
        }

        [RelayCommand]
        private void UseItem(Item item)
        {
            if (_gameStateService.CurrentPlayer == null || item == null) return;

            item.Use(_gameStateService.CurrentPlayer);
            if (item.Type == ItemType.Consumable)
            {
                InventoryItems.Remove(item);
            }
        }


        [RelayCommand]
        private void DropItem()
        {
            if (SelectedItem != null)
            {
                InventoryItems.Remove(SelectedItem);
                SelectedItem = null;
            }
        }

        [RelayCommand]
        private void Close()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}