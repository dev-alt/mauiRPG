using mauiRPG.Models;
using System.Collections.ObjectModel;

namespace mauiRPG.Services
{
    public class InventoryService
    {
        private readonly Dictionary<int, ObservableCollection<Item>> _playerInventories = [];

        public ObservableCollection<Item> GetPlayerItems(int playerId)
        {
            if (_playerInventories.TryGetValue(playerId, out var items))
            {
                return items;
            }
            var newInventory = new ObservableCollection<Item>();
            _playerInventories[playerId] = newInventory;
            return newInventory;
        }

        public void AddItem(int playerId, Item item)
        {
            var items = GetPlayerItems(playerId);
            items.Add(item);
        }

        public void RemoveItem(int playerId, int itemId)
        {
            var items = GetPlayerItems(playerId);
            var itemToRemove = items.FirstOrDefault(item => item.Id == itemId);
            if (itemToRemove != null)
            {
                items.Remove(itemToRemove);
            }
        }
    }
}