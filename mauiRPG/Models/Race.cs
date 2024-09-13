namespace mauiRPG.Models
{
    public abstract class Race(string name, string description)
    {
        public string Name { get; init; } = name;
        public string Description { get; init; } = description;
        public int StrengthBonus { get; init; }
        public int IntelligenceBonus { get; init; }
        public int DexterityBonus { get; init; }
        public int ConstitutionBonus { get; init; }

        public string GetBonusSummary()
        {
            var bonuses = new List<string>();
            if (StrengthBonus != 0) bonuses.Add($"Strength +{StrengthBonus}");
            if (IntelligenceBonus != 0) bonuses.Add($"Intelligence +{IntelligenceBonus}");
            if (DexterityBonus != 0) bonuses.Add($"Dexterity +{DexterityBonus}");
            if (ConstitutionBonus != 0) bonuses.Add($"Constitution +{ConstitutionBonus}");
            return string.Join(", ", bonuses);
        }
    }

    public class Human : Race
    {
        public Human() : base("Human", "Versatile and adaptable, humans excel in various roles.")
        {
            StrengthBonus = 1;
            IntelligenceBonus = 1;
            DexterityBonus = 1;
            ConstitutionBonus = 1;
        }
    }

    public class Elf : Race
    {
        public Elf() : base("Elf", "Graceful and long-lived, elves excel in magic and archery.")
        {
            DexterityBonus = 4;
            IntelligenceBonus = 1;
        }
    }

    public class Dwarf : Race
    {
        public Dwarf() : base("Dwarf", "Stout and hardy, dwarves are known for their craftsmanship and resilience.")
        {
            ConstitutionBonus = 2;
            StrengthBonus = 4;
        }
    }

    public class Orc : Race
    {
        public Orc() : base("Orc", "Fierce warriors with unmatched strength and endurance.")
        {
            StrengthBonus = 6;
            ConstitutionBonus = 3;
        }
    }
}