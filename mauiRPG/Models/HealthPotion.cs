namespace mauiRPG.Models
{
    public class HealthPotion : Item
    {
        public int HealAmount { get; set; }

        public HealthPotion()
        {
            Name = "Health Potion";
            Description = "Restores health when consumed";
            Type = ItemType.Consumable;
            Value = 50;
            HealAmount = 50;
        }

        public override void Use(Player player)
        {
            player.Health = Math.Min(player.Health + HealAmount, player.MaxHealth);
        }
    }
}
