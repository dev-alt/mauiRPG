using mauiRPG.Models;
using System.Collections.Generic;

namespace mauiRPG.Services
{
    public class InventoryService
    {
        private readonly Dictionary<int, List<Item>> _playerInventories = [];

        public List<Item> GetPlayerItems(int playerId)
        {
            if (_playerInventories.TryGetValue(playerId, out var items))
            {
                return items;
            }
            return [];
        }

        public void AddItem(int playerId, Item item)
        {
            if (!_playerInventories.TryGetValue(playerId, out List<Item>? value))
            {
                value = [];
                _playerInventories[playerId] = value;
            }

            value.Add(item);
        }

        public void RemoveItem(int playerId, int itemId)
        {
            if (_playerInventories.TryGetValue(playerId, out var items))
            {
                items.RemoveAll(item => item.Id == itemId);
            }
        }

    }
}