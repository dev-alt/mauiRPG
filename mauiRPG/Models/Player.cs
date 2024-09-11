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
        public int Experience { get; set; } = 0;
        public bool IsDefending { get; set; }
        public List<Quest> ActiveQuests { get; set; } = [];
        public List<Quest> CompletedQuests { get; set; } = [];
        public int Gold { get; set; }

        public void UpdateStats()
        {
            Strength = 10;
            Intelligence = 10;
            Dexterity = 10;
            Constitution = 10;

            foreach (var equipment in EquippedItems.Values)
            {
                Strength += equipment.StrengthBonus;
                Intelligence += equipment.IntelligenceBonus;
                Dexterity += equipment.DexterityBonus;
                Constitution += equipment.ConstitutionBonus;
            }

            MaxHealth = 100 + (Constitution * 10);
        }

        public void GainExperience(int amount)
        {
            Experience += amount;
            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            int experienceNeeded = Level * 100;
            while (Experience >= experienceNeeded)
            {
                Level++;
                Experience -= experienceNeeded;
                // Apply level up bonuses
                MaxHealth += 10;
                Strength += 2;
                Intelligence += 2;
                Dexterity += 2;
                Constitution += 2;
                experienceNeeded = Level * 100;
            }
        }
    }
}