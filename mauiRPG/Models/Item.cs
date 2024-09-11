namespace mauiRPG.Models
{
    public abstract class Item
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string IconSource { get; set; }
        public ItemType Type { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
        public bool IsEquippable { get; set; }

        public abstract void Use(Player player);
    }

    public enum ItemType
    {
        Consumable,
        Equipment,
        QuestItem
    }
}