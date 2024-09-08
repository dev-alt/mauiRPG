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
            // Ensure health doesn't exceed max health
            player.CurrentHealth = Math.Min(player.CurrentHealth + HealAmount, player.MaxHealth);
        }
    }
}