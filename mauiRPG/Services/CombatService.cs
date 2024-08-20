using mauiRPG.Models;

namespace mauiRPG.Services
{
    public class CombatService
    {
        private readonly Random _random = new();

        public int CalculateDamage(int attackStat, int defenseStat)
        {
            int baseDamage = _random.Next(attackStat / 2, attackStat);
            return Math.Max(0, baseDamage - defenseStat);
        }

        public CombatResult ExecutePlayerAttack(Player player, Enemy enemy)
        {
            int damage = CalculateDamage(player.Strength, enemy.Defense);
            enemy.Health -= damage;
            return new CombatResult
            {
                Attacker = player.Name,
                Defender = enemy.Name,
                Damage = damage,
                RemainingHealth = enemy.Health
            };
        }

        public CombatResult ExecuteEnemyAttack(Enemy enemy, Player player)
        {
            int damage = CalculateDamage(enemy.Attack, player.Constitution);
            player.Health -= damage;
            return new CombatResult
            {
                Attacker = enemy.Name,
                Defender = player.Name,
                Damage = damage,
                RemainingHealth = player.Health
            };
        }
    }
}
