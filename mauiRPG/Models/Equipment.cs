namespace mauiRPG.Models
{
    public abstract class Equipment : Item
    {
        public EquipmentSlot Slot { get; set; }
        public int StrengthBonus { get; set; }
        public int IntelligenceBonus { get; set; }
        public int DexterityBonus { get; set; }
        public int ConstitutionBonus { get; set; }

        public Equipment()
        {
            Type = ItemType.Equipment;
        }

        public override void Use(Player player)
        {
            // Equip the item
            if (player.EquippedItems.ContainsKey(Slot))
            {
                player.EquippedItems[Slot] = this;
            }
            else
            {
                player.EquippedItems.Add(Slot, this);
            }
            player.UpdateStats();
        }
    }

    public enum EquipmentSlot
    {
        Head,
        Body,
        Hands,
        Feet,
        Weapon,
        Shield
    }
}