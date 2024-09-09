using mauiRPG.Models;
using System;
using System.Threading.Tasks;

namespace mauiRPG.Services
{
    /// <summary>
    /// Manages combat-related operations and enemy generation.
    /// </summary>
    public class CombatManagerService(ICombatService combatService)
    {
        private readonly ICombatService _combatService = combatService ?? throw new ArgumentNullException(nameof(combatService));
        private readonly Random _random = new();

        /// <summary>
        /// Executes the player's turn in combat.
        /// </summary>
        public CombatResult ExecutePlayerTurn(Player player, CombatantModel enemy)
        {
            return _combatService.ExecutePlayerAttack(player, enemy);
        }

        /// <summary>
        /// Executes the enemy's turn in combat.
        /// </summary>
        public CombatResult ExecuteEnemyTurn(CombatantModel enemy, Player player)
        {
            return _combatService.ExecuteEnemyAttack(enemy, player);
        }

        /// <summary>
        /// Executes the player's defend action.
        /// </summary>
        public static CombatResult ExecutePlayerDefend(Player player, CombatantModel enemy)
        {
            player.IsDefending = true;
            int damage = CalculateDamage(enemy.Attack, player.Defense * 2); // Double defense when defending
            player.CurrentHealth = Math.Max(0, player.CurrentHealth - damage);
            player.IsDefending = false;

            return new CombatResult
            {
                Attacker = enemy.Name,
                Defender = player.Name,
                Damage = damage,
                RemainingHealth = player.CurrentHealth,
                Message = $"{player.Name} defends. {enemy.Name} attacks for {damage} damage!"
            };
        }

        /// <summary>
        /// Attempts to escape from combat.
        /// </summary>
        public bool AttemptEscape()
        {
            return _combatService.AttemptEscape();
        }

        /// <summary>
        /// Generates a new enemy based on the current battle count.
        /// </summary>
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

        /// <summary>
        /// Prepares for the next battle, healing the player and generating a new enemy.
        /// </summary>
        public async Task<CombatantModel> PrepareNextBattle(Player player, int battleCount)
        {
            await Task.Delay(3000); // Simulating preparation time

            var newEnemy = GenerateNewEnemy(battleCount);
            int healAmount = (int)(player.MaxHealth * 0.2);
            player.Heal(healAmount);
            player.IsDefending = false;

            return newEnemy;
        }

        /// <summary>
        /// Checks if the combat is over.
        /// </summary>
        public static bool IsCombatOver(Player player, CombatantModel enemy)
        {
            return player.CurrentHealth <= 0 || enemy.CurrentHealth <= 0;
        }

        /// <summary>
        /// Gets the result of the combat.
        /// </summary>
        public static string GetCombatResult(Player player, CombatantModel enemy)
        {
            if (player.CurrentHealth <= 0)
                return $"{player.Name} has been defeated. Game Over!";
            else if (enemy.CurrentHealth <= 0)
                return $"{enemy.Name} has been defeated. {player.Name} is victorious!";
            return "Combat is still ongoing.";
        }

        /// <summary>
        /// Calculates the damage dealt in an attack.
        /// </summary>
        private static int CalculateDamage(int attack, int defense)
        {
            return Math.Max(1, attack - defense);
        }
    }
}