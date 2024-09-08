using System;

namespace mauiRPG.Models
{
    public abstract class Item
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ItemType Type { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }

        public abstract void Use(Player player);
    }

    public enum ItemType
    {
        Consumable,
        Equipment,
        QuestItem
    }
}