using mauiRPG.Models;

namespace mauiRPG.Services
{
    public class CombatManagerService(ICombatService combatService)
    {
        private readonly Random _random = new();

        public CombatResult ExecutePlayerTurn(Player player, EnemyModel enemy)
        {
            return combatService.ExecutePlayerAttack(player, enemy);
        }
        public CombatResult ExecuteSpecialAttack(Character attacker, Character defender, double damageMultiplier)
        {
            return combatService.ExecuteSpecialAttack(attacker, defender, damageMultiplier);
        }
        public CombatResult ExecuteEnemyTurn(EnemyModel enemy, Player player)
        {
            int action = _random.Next(100);
            if (action < 20 && !enemy.IsDefending)
            {
                enemy.IsDefending = true;
                return combatService.Defend(enemy);
            }
            else if (action < 40 && enemy.CurrentHealth < enemy.MaxHealth / 2)
            {
                // Assuming a default multiplier for enemy special attacks
                return ExecuteSpecialAttack(enemy, player, 1.5);
            }
            else
            {
                return combatService.ExecuteEnemyAttack(enemy, player);
            }
        }

        public EnemyModel GenerateNewEnemy(int battleCount)
        {
            string[] enemyTypes = ["Goblin", "Orc", "Troll", "Dark Elf", "Dragon"];
            string enemyName = enemyTypes[_random.Next(enemyTypes.Length)];
            int baseHealth = 10 + (battleCount * 10);
            int healthVariation = _random.Next(-10, 11);
            int finalHealth = baseHealth + healthVariation;

            return new EnemyModel
            {
                Name = $"{enemyName} Lvl {battleCount}",
                MaxHealth = finalHealth,
                CurrentHealth = finalHealth,
                Attack = 5 + (battleCount * 2),
                Defense = 3 + battleCount,
                Level = battleCount
            };
        }
        public async Task<EnemyModel> PrepareNextBattle(Player player, int battleCount)
        {
            await Task.Delay(1000); // Reduced delay for better game flow
            var newEnemy = GenerateNewEnemy(battleCount);
            int healAmount = (int)(player.MaxHealth * 0.2);
            player.Heal(healAmount);
            player.IsDefending = false;
            return newEnemy;
        }

        public bool AttemptEscape()
        {
            return combatService.AttemptEscape();
        }
        public CombatResult ExecuteSpecialAttack(Player player, EnemyModel enemy, double damageMultiplier)
        {
            return combatService.ExecuteSpecialAttack(player, enemy, damageMultiplier);
        }
        public static bool IsCombatOver(Player player, EnemyModel enemy)
        {
            return player.CurrentHealth <= 0 || enemy.CurrentHealth <= 0;
        }

        public static string GetCombatResult(Player player, EnemyModel enemy)
        {
            if (player.CurrentHealth <= 0)
                return $"{player.Name} has been defeated. Game Over!";
            else if (enemy.CurrentHealth <= 0)
                return $"{enemy.Name} has been defeated. {player.Name} is victorious!";
            return "Combat is still ongoing.";
        }
    }
}