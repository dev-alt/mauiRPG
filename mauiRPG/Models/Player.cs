using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    internal class Player : ICharacter
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }

    }
}
