using mauiRPG.Models;
using mauiRPG.Services;
using System.Collections.ObjectModel;

namespace mauiRPG.Services
{
    public class CombatManagerService(ICombatService combatService)
    {
        private readonly ICombatService _combatService = combatService;
        private readonly Random _random = new();

        public CombatResult ExecutePlayerTurn(Player player, CombatantModel enemy)
        {
            return _combatService.ExecutePlayerAttack(player, enemy);
        }

        public CombatResult ExecuteEnemyTurn(CombatantModel enemy, Player player)
        {
            return _combatService.ExecuteEnemyAttack(enemy, player);
        }

        public CombatResult ExecutePlayerDefend(Player player, CombatantModel enemy)
        {
            player.IsDefending = true;
            var result = new CombatResult
            {
                Attacker = player.Name,
                Defender = player.Name,
                Message = $"{player.Name} takes a defensive stance.",
                Damage = 0,
                RemainingHealth = player.CurrentHealth
            };

            var enemyResult = _combatService.ExecuteEnemyAttack(enemy, player);
            player.IsDefending = false;

            return enemyResult;
        }

        public bool AttemptEscape()
        {
            return _combatService.AttemptEscape();
        }

        public CombatantModel GenerateNewEnemy(int battleCount)
        {
            string[] enemyTypes = ["Goblin", "Orc", "Troll", "Dark Elf", "Dragon"];
            string enemyName = enemyTypes[_random.Next(enemyTypes.Length)];

            int baseHealth = 50 + (battleCount * 10);
            int healthVariation = _random.Next(-10, 11);

            return new CombatantModel
            {
                Name = $"{enemyName} Lvl {battleCount}",
                MaxHealth = baseHealth + healthVariation,
                CurrentHealth = baseHealth + healthVariation,
                Attack = 5 + (battleCount * 2),
                Defense = 3 + battleCount
            };
        }

        public async Task<CombatantModel> PrepareNextBattle(Player player, int battleCount)
        {
            await Task.Delay(3000); // Simulating preparation time

            var newEnemy = GenerateNewEnemy(battleCount);

            int healAmount = (int)(player.MaxHealth * 0.2);
            player.Heal(healAmount);

            player.IsDefending = false;

            return newEnemy;
        }

        public static bool IsCombatOver(Player player, CombatantModel enemy)
        {
            return player.CurrentHealth <= 0 || enemy.CurrentHealth <= 0;
        }

        public static string GetCombatResult(Player player, CombatantModel enemy)
        {
            if (player.CurrentHealth <= 0)
                return $"{player.Name} has been defeated. Game Over!";
            else if (enemy.CurrentHealth <= 0)
                return $"{enemy.Name} has been defeated. {player.Name} is victorious!";
            return "Combat is still ongoing.";
        }
    }
}