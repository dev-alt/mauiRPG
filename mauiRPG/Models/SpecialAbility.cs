﻿namespace mauiRPG.Models
{
    public class SpecialAbility
    {
        public required string Name { get; init; }
        public required string Description { get; set; }
        public int CooldownTurns { get; init; }
        public double DamageMultiplier { get; init; }
        public int CurrentCooldown { get; set; }
    }
}
