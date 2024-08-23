using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public class Player : Character
    {
        public Dictionary<EquipmentSlot, Equipment> EquippedItems { get; set; } = [];

        [JsonConverter(typeof(RaceConverter))] public required Race Race { get; set; }

        [JsonConverter(typeof(ClassConverter))]
        public required Class Class { get; set; }

        public int Experience { get; set; } = 0;
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
            MaxHealth = 100 + (Constitution * 10); // Example calculation
        }
    }
}

