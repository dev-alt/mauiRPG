namespace mauiRPG.Models
{
    public class CombatResult
    {
        public required string Attacker { get; set; }
        public required string Defender { get; set; }
        public int Damage { get; set; }
        public int RemainingHealth { get; set; }
        public string? Message { get; set; }
    }

}