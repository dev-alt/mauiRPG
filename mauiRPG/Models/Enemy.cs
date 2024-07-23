using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    internal abstract class Enemy
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double Health { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Level { get; set; }
        public int ExperienceValue { get; set; }
        protected Enemy(string name, int health, int attack, int defense, int level, int experienceValue)
        {
            Name = name;
            MaxHealth = health;
            Health = MaxHealth;
            Attack = attack;
            Defense = defense;
            Level = level;
            ExperienceValue = experienceValue;
        }

    }

    internal class EnemyWizard : Enemy
    {
        public EnemyWizard(string name, int level) : base(name, 50 + (level * 10), 5 + (level * 2), 2 + level, level, 10 + (level * 5))
        {
        }

    }
}
