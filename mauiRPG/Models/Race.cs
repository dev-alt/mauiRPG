namespace mauiRPG.Models
{
    public abstract class Race
    {
        public abstract string Name { get; }
        public int StrengthBonus { get; set; }
        public int IntelligenceBonus { get; set; }
        public int DexterityBonus { get; set; }
        public int ConstitutionBonus { get; set; }
    }

    public class Orc : Race
    {
        public override string Name => "Orc";

        public Orc()
        {
            StrengthBonus = 6;
            ConstitutionBonus = 3;
        }
    }

    public class Human : Race
    {
        public override string Name => "Human";

        public Human()
        {
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
            StrengthBonus = 4;
        }
    }

    public class Elf : Race
    {
        public override string Name => "Elf";

        public Elf()
        {
            DexterityBonus = 4;
            IntelligenceBonus = 1;
        }
    }
}