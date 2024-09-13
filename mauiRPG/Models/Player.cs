using mauiRPG.Converters;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace mauiRPG.Models
{
    public class Player : Character
    {
        public new int Id { get; set; }
        public Dictionary<EquipmentSlot, Equipment> EquippedItems { get; init; } = [];
        [JsonConverter(typeof(RaceConverter))] public required Race Race { get; init; }
        public ObservableCollection<Item> Inventory { get; set; } = [];
        public int Experience { get; set; } = 0;
        public bool IsDefending { get; set; }
        public List<Quest> ActiveQuests { get; set; } = [];
        public List<Quest> CompletedQuests { get; set; } = [];
        public int Gold { get; set; }
        public List<SpecialAbility> SpecialAbilities { get; set; }
        public Player()
        {
            SpecialAbilities =
            [
                new SpecialAbility
                {
                    Name = "Power Strike",
                    Description = "A powerful attack that deals 2x damage",
                    CooldownTurns = 3,
                    DamageMultiplier = 2,
                    CurrentCooldown = 0
                },

                new SpecialAbility
                {
                    Name = "Quick Slash",
                    Description = "A fast attack that deals 1.5x damage and has a shorter cooldown",
                    CooldownTurns = 2,
                    DamageMultiplier = 1.5,
                    CurrentCooldown = 0
                }
            ];
        }

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