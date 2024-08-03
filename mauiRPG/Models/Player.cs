using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public class Player : Character, ICharacter
    {
        public required IRace Race { get; set; }
        public required IClass Class { get; set; }

    }
}
