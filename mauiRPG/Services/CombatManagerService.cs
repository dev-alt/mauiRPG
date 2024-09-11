﻿using mauiRPG.Models;
using System;
using System.Threading.Tasks;

namespace mauiRPG.Services
{
    public class CombatManagerService(ICombatService combatService)
    {
        private readonly Random _random = new();

        public CombatResult ExecutePlayerTurn(Player player, CombatantModel enemy)
        {
            return combatService.ExecutePlayerAttack(player, enemy);
        }

        public CombatResult ExecuteEnemyTurn(CombatantModel enemy, Player player)
        {
            int action = _random.Next(100);
            if (action < 20 && !enemy.IsDefending)
            {
                enemy.IsDefending = true;
                return combatService.Defend(enemy);
            }
            else if (action < 40 && enemy.CurrentHealth < enemy.MaxHealth / 2)
            {
                return combatService.ExecuteSpecialAttack(enemy, player);
            }
            else
            {
                return combatService.ExecuteEnemyAttack(enemy, player);
            }
        }

        private CombatantModel GenerateNewEnemy(int battleCount)
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

        public bool AttemptEscape()
        {
            return combatService.AttemptEscape();
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