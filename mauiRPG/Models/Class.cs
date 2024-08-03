using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public class Class : IClass
    {
        public void LevelUp(ICharacter character)
        {
        }
    }
    public class Warrior : IClass
    {
        public string ClassName => "Warrior";
        public void LevelUp(ICharacter character)
        {
        }

    }
    public class Mage : IClass
    {
        public string ClassName => "Mage";
        public void LevelUp(ICharacter character)
        {
        }
    }


}
