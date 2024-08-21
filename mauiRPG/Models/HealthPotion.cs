using mauiRPG.Models;

namespace mauiRPG.Models
{
    public class HealthPotion : Item
    {
        public int HealAmount { get; set; } = 50;

        public HealthPotion()
        {
            Type = ItemType.Consumable;
        }

        public override void Use(Player player)
        {
            player.Health += HealAmount;
            // Ensure health doesn't exceed max health
            if (player.Health > player.MaxHealth)
            {
                player.Health = player.MaxHealth;
            }
        }
    }
}