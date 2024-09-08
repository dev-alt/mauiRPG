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