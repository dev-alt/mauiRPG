using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace mauiRPG.ViewModels
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;
        private readonly InventoryService _inventoryService;

        [ObservableProperty]
        private ObservableCollection<Item> _inventoryItems = new();

        [ObservableProperty]
        private Item? _selectedItem;

        public event EventHandler? CloseRequested;

        public InventoryViewModel(GameStateService gameStateService, InventoryService inventoryService)
        {
            _gameStateService = gameStateService;
            _inventoryService = inventoryService;
            LoadInventory();
            Debug.WriteLine("InventoryViewModel initialized. Inventory contains: " + string.Join(", ", _inventoryItems.Select(item => item.Name)));
        }

        private void LoadInventory()
        {
            if (_gameStateService.CurrentPlayer != null)
            {
                InventoryItems = _inventoryService.GetPlayerItems(_gameStateService.CurrentPlayer.Id);
            }
            else
            {
                InventoryItems = new ObservableCollection<Item>();
            }
        }

        [RelayCommand]
        private void UseItem(Item item)
        {
            if (_gameStateService.CurrentPlayer == null || item == null) return;

            item.Use(_gameStateService.CurrentPlayer);
            if (item.Type == ItemType.Consumable)
            {
                _inventoryService.RemoveItem(_gameStateService.CurrentPlayer.Id, item.Id);
                InventoryItems.Remove(item);
            }
        }

        [RelayCommand]
        private void DropItem()
        {
            if (_gameStateService.CurrentPlayer == null || SelectedItem == null) return;

            _inventoryService.RemoveItem(_gameStateService.CurrentPlayer.Id, SelectedItem.Id);
            InventoryItems.Remove(SelectedItem);
            SelectedItem = null;
        }

        [RelayCommand]
        private void Close()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}