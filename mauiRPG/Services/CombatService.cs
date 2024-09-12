using mauiRPG.Models;
using System;

namespace mauiRPG.Services
{
    public interface ICombatService
    {
        CombatResult ExecutePlayerAttack(Player player, EnemyModel enemy);
        CombatResult ExecuteEnemyAttack(EnemyModel enemy, Player player);
        CombatResult ExecuteAttack(Character attacker, Character defender, bool isDefending);
        CombatResult ExecuteSpecialAttack(Character attacker, Character defender, double damageMultiplier);
        CombatResult Defend(Character character);
        bool AttemptEscape();
    }

    public class CombatService : ICombatService
    {
        private readonly Random _random = new();

        public CombatResult ExecutePlayerAttack(Player player, EnemyModel enemy)
        {
            try
            {
                return ExecuteAttack(player, enemy, enemy.IsDefending);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing player attack: {ex.Message}");
                return new CombatResult
                {
                    Attacker = player.Name,
                    Defender = enemy.Name,
                    Message = "An error occurred during the player's attack."
                };
            }
        }

        public CombatResult ExecuteEnemyAttack(EnemyModel enemy, Player player)
        {
            try
            {
                return ExecuteAttack(enemy, player, player.IsDefending);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing enemy attack: {ex.Message}");
                return new CombatResult
                {
                    Attacker = enemy.Name,
                    Defender = player.Name,
                    Message = "An error occurred during the enemy's attack."
                };
            }
        }

        public CombatResult ExecuteAttack(Character attacker, Character defender, bool isDefending = false)
        {
            try
            {
                bool isCritical = _random.Next(100) < 10; // 10% chance of critical hit
                int damage = CalculateDamage(attacker.Attack, defender.Defense, isDefending);
                if (isCritical)
                {
                    damage = (int)(damage * 1.5);
                }
                defender.TakeDamage(damage);
                return new CombatResult
                {
                    Attacker = attacker.Name,
                    Defender = defender.Name,
                    Damage = damage,
                    RemainingHealth = defender.CurrentHealth,
                    Message = GenerateAttackMessage(attacker.Name, defender.Name, damage, isCritical)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing attack: {ex.Message}");
                return new CombatResult
                {
                    Attacker = attacker.Name,
                    Defender = defender.Name,
                    Message = "An error occurred during the attack."
                };
            }
        }

        private string GenerateAttackMessage(string attacker, string defender, int damage, bool isCritical)
        {
            string[] attackVerbs = ["strikes", "hits", "slashes at", "bashes"];
            string verb = attackVerbs[_random.Next(attackVerbs.Length)];
            string criticalText = isCritical ? " Critical hit!" : "";
            return $"{attacker} {verb} {defender} for {damage} damage!{criticalText}";
        }

        public CombatResult ExecuteSpecialAttack(Character attacker, Character defender, double damageMultiplier)
        {
            try
            {
                int damage = CalculateDamage(attacker.Attack * damageMultiplier, defender.Defense, false);
                defender.TakeDamage(damage);
                return new CombatResult
                {
                    Attacker = attacker.Name,
                    Defender = defender.Name,
                    Damage = damage,
                    RemainingHealth = defender.CurrentHealth,
                    Message = $"{attacker.Name} uses a special attack against {defender.Name} for {damage} damage!"
                };
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

        public CombatResult Defend(Character character)
        {
            try
            {
                return new CombatResult
                {
                    Attacker = character.Name,
                    Defender = character.Name,
                    Damage = 0,
                    RemainingHealth = character.CurrentHealth,
                    Message = $"{character.Name} takes a defensive stance."
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing defend action: {ex.Message}");
                return new CombatResult
                {
                    Attacker = character.Name,
                    Defender = character.Name,
                    Message = "An error occurred while taking a defensive stance."
                };
            }
        }

        public bool AttemptEscape()
        {
            try
            {
                return _random.Next(100) < 50;  // 50% chance of escape
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error attempting escape: {ex.Message}");
                return false;
            }
        }

        private int CalculateDamage(double attack, int defense, bool isDefending)
        {
            try
            {
                double damageReduction = defense / (defense + 100.0);
                int baseDamage = (int)Math.Max(5, attack * (1 - damageReduction) * 1.5);
                int variability = _random.Next(-3, 4);
                int finalDamage = Math.Max(0, baseDamage + variability);
                return isDefending ? finalDamage / 2 : finalDamage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating damage: {ex.Message}");
                return 0;
            }
        }
    }
}