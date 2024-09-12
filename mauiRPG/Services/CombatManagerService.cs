using mauiRPG.Models;
using System;
using System.Threading.Tasks;

namespace mauiRPG.Services
{
    public class CombatManagerService(ICombatService combatService)
    {
        private readonly ICombatService _combatService = combatService ?? throw new ArgumentNullException(nameof(combatService));
        private readonly Random _random = new();

        public CombatResult ExecutePlayerTurn(Player player, EnemyModel enemy)
        {
            try
            {
                return _combatService.ExecutePlayerAttack(player, enemy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing player turn: {ex.Message}");
                return new CombatResult
                {
                    Attacker = player.Name,
                    Defender = enemy.Name,
                    Message = "An error occurred during the player's turn."
                };
            }
        }

        public CombatResult ExecuteSpecialAttack(Character attacker, Character defender, double damageMultiplier)
        {
            try
            {
                return _combatService.ExecuteSpecialAttack(attacker, defender, damageMultiplier);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing special attack: {ex.Message}");
                return new CombatResult
                {
                    Attacker = attacker.Name,
                    Defender = defender.Name,
                    Message = "An error occurred during the special attack."
                };
            }
        }

        public CombatResult ExecuteEnemyTurn(EnemyModel enemy, Player player)
        {
            try
            {
                int action = _random.Next(100);
                if (action < 20 && !enemy.IsDefending)
                {
                    enemy.IsDefending = true;
                    return _combatService.Defend(enemy);
                }
                else if (action < 40 && enemy.CurrentHealth < enemy.MaxHealth / 2)
                {
                    return ExecuteSpecialAttack(enemy, player, 1.5);
                }
                else
                {
                    return _combatService.ExecuteEnemyAttack(enemy, player);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing enemy turn: {ex.Message}");
                return new CombatResult
                {
                    Attacker = enemy.Name,
                    Defender = player.Name,
                    Message = "An error occurred during the enemy's turn."
                };
            }
        }

        public EnemyModel GenerateNewEnemy(int battleCount)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating new enemy: {ex.Message}");
                return new EnemyModel { Name = "Default Enemy" };
            }
        }

        public async Task<EnemyModel> PrepareNextBattle(Player player, int battleCount)
        {
            try
            {
                await Task.Delay(1000);
                var newEnemy = GenerateNewEnemy(battleCount);
                int healAmount = (int)(player.MaxHealth * 0.2);
                player.Heal(healAmount);
                player.IsDefending = false;
                return newEnemy;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error preparing next battle: {ex.Message}");
                return new EnemyModel { Name = "Default Enemy" };
            }
        }

        public bool AttemptEscape()
        {
            try
            {
                return _combatService.AttemptEscape();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error attempting escape: {ex.Message}");
                return false;
            }
        }

        public CombatResult ExecuteSpecialAttack(Player player, EnemyModel enemy, double damageMultiplier)
        {
            try
            {
                return _combatService.ExecuteSpecialAttack(player, enemy, damageMultiplier);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing special attack: {ex.Message}");
                return new CombatResult
                {
                    Attacker = player.Name,
                    Defender = enemy.Name,
                    Message = "An error occurred during the special attack."
                };
            }
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