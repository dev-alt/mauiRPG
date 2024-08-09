using mauiRPG.Models;

namespace mauiRPG.Services
{
    public class CombatService
    {
        public void ExecutePlayerAttack(Player player, Enemy enemy)
        {
            int damage = CalculateDamage(player.Strength, enemy.Defense);
            enemy.Health -= damage;
        }

        public void ExecuteEnemyAttack(Enemy enemy, Player player)
        {
            int damage = CalculateDamage(enemy.Attack, player.Constitution);
            player.Health -= damage;
        }

        private int CalculateDamage(int attackStat, int defenseStat)
        {
            // Simple damage calculation, can be made more complex
            return Math.Max(0, attackStat - defenseStat);
        }
    }
}