﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public interface ICharacter
    {
        string Name { get; set; }
        int Health { get; set; }
        int Level { get; set; }
    }

    public interface IRace
    {
        string RaceName { get; }
        int StrengthBonus { get; }
        int IntelligenceBonus { get; }
    }

    public interface IClass
    {
        string ClassName { get; }
        void LevelUp(ICharacter character);
        void PerformSpecialAbility(ICharacter character);
    }
}
