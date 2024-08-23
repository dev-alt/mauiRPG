using mauiRPG.Models;

namespace mauiRPG.Services
{
    public class LevelUpService
    {
        public static void GainExperience(Player player, int experiencePoints)
        {
            player.Experience += experiencePoints;
            CheckLevelUp(player);
        }

        private static void CheckLevelUp(Player player)
        {
            int experienceNeeded = player.Level * 100;
            while (player.Experience >= experienceNeeded)
            {
                player.Level++;
                player.Experience -= experienceNeeded;
                ApplyLevelUpBonuses(player);
                experienceNeeded = player.Level * 100;
            }
        }

        private static void ApplyLevelUpBonuses(Player player)
        {
            player.MaxHealth += 10;
            player.Health = player.MaxHealth;
            player.Strength += 2;
            player.Intelligence += 2;
            player.Dexterity += 2;
            player.Constitution += 2;
        }
    }
}