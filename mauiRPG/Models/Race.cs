using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public abstract class Race
    {
        public abstract string Name { get; }
    }
    internal class Orc : Race
    {

        public override string Name => "Orc";
    }

    internal class Human : Race
    {
        public override string Name => "Human";
    }
    internal class Dwarf : Race
    {
        public override string Name => "Dwarf";

    }
    internal class Elf : Race
    {
        public override string Name => "Elf";

    }


}
