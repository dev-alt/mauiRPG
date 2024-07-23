using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    abstract class Class : IClass
    {
        public string ClassName { get; }

        public void LevelUp(ICharacter character)
        {

        }

        public void PerformSpecialAbility(ICharacter character)
        {

        }
    }

    internal class Mage : Class
    {

    }
}
