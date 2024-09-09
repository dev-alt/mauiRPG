using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using mauiRPG.Converters;


namespace mauiRPG.Models
{
    public class Player : Character
    {
        public Dictionary<EquipmentSlot, Equipment> EquippedItems { get; init; } = [];

        [JsonConverter(typeof(RaceConverter))] public required Race Race { get; init; }

        public ObservableCollection<Item> Inventory { get; set; } = [];

        public int Level { get; set; } 
        public int Health { get; set; } 
        public int Experience { get; set; } = 0;
        public bool IsDefending { get; set; }

        public void UpdateStats()
        {
            Strength = 10;
            Intelligence = 10;
            Dexterity = 10;
            Constitution = 10;

            // Apply equipment bonuses
            foreach (var equipment in EquippedItems.Values)
            {
                Strength += equipment.StrengthBonus;
                Intelligence += equipment.IntelligenceBonus;
                Dexterity += equipment.DexterityBonus;
                Constitution += equipment.ConstitutionBonus;
            }

            // Recalculate max health based on new Constitution
            MaxHealth = 100 + (Constitution * 10); 
        }
    }
}

