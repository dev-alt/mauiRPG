using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    internal class Player : ICharacter
    {
        public required string Name { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public required IRace Race { get; set; }
        public required IClass Class { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }

    }
}
