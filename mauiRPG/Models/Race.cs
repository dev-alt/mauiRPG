namespace mauiRPG.Models
{
    public abstract class Race
    {
        public abstract string Name { get; }
        public int StrengthBonus { get; set; }
        public int IntelligenceBonus { get; set; }
        public int DexterityBonus { get; set; }
        public int ConstitutionBonus { get; set; }

        // Add any other common properties or methods here
    }

    public class Orc : Race
    {
        public override string Name => "Orc";

        public Orc()
        {
            StrengthBonus = 2;
            ConstitutionBonus = 1;
            // Set other Orc-specific bonuses or properties
        }
    }

    public class Human : Race
    {
        public override string Name => "Human";

        public Human()
        {
            // Humans could have balanced stats
            StrengthBonus = 1;
            IntelligenceBonus = 1;
            DexterityBonus = 1;
            ConstitutionBonus = 1;
        }
    }

    public class Dwarf : Race
    {
        public override string Name => "Dwarf";

        public Dwarf()
        {
            ConstitutionBonus = 2;
            StrengthBonus = 1;
            // Set other Dwarf-specific bonuses or properties
        }
    }

    public class Elf : Race
    {
        public override string Name => "Elf";

        public Elf()
        {
            DexterityBonus = 2;
            IntelligenceBonus = 1;
            // Set other Elf-specific bonuses or properties
        }
    }
}