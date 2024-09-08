using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public abstract class Class
    {
        public abstract string Name { get; }
        public abstract void LevelUp(Character character);
    }

    public class Warrior : Class
    {
        public override string Name => "Warrior";
        public override void LevelUp(Character character)
        {
        }

    }

    public class Mage : Class
    {
        public override string Name => "Mage";

        public override void LevelUp(Character character)
        {
            // Implement level up logic for Mage
        }
    }

    public class Rogue : Class
    {
        public override string Name => "Rogue";

        public override void LevelUp(Character character)
        {

        }
    }
}
