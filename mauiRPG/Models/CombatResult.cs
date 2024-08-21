﻿namespace mauiRPG.Models
{
    public class CombatResult
    {
        public required string Attacker { get; set; }
        public required string Defender { get; set; }
        public int Damage { get; set; }
        public double RemainingHealth { get; set; }
    }

    public enum CombatOutcome
    {
        PlayerVictory,
        EnemyVictory
    }
}