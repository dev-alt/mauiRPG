using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    internal class Orc : IRace
    {
        public string RaceName { get; init; } 
        public int StrengthBonus { get; } = 0;
        public int IntelligenceBonus { get; } = 0;
    }

    internal class Human : IRace
    {
        public string RaceName { get; init; }
        public int StrengthBonus { get; } = 0;
        public int IntelligenceBonus { get; } = 0;
    }
    internal class Dwarf : IRace
    {
        public string RaceName { get; init; }
        public int StrengthBonus { get; } = 0;
        public int IntelligenceBonus { get; } = 0;
    }
    internal class Elf : IRace
    {
        public string RaceName { get; init; }
        public int StrengthBonus { get; } = 0;
        public int IntelligenceBonus { get; } = 0;
    }


}
