using mauiRPG.Models;
using mauiRPG.Services;

namespace mauiRPG.Controllers
{
    public class CombatManager
    {
        private readonly Player _player;
        private readonly Enemy _enemy;
        private readonly CombatService _combatService;

        public CombatManager(Player player, Enemy enemy, CombatService combatService)
        {
            _player = player;
            _enemy = enemy;
            _combatService = combatService;
        }

        public string ExecuteTurn()
        {
            string result = "";

            // Player's turn
            int playerDamage = _combatService.CalculateDamage(_player.Strength, _enemy.Defense);
            _enemy.Health -= playerDamage;
            result += $"{_player.Name} deals {playerDamage} damage to {_enemy.Name}.\n";

            if (_enemy.Health <= 0)
            {
                result += $"{_enemy.Name} has been defeated!\n";
                return result;
            }

            // Enemy's turn
            int enemyDamage = _combatService.CalculateDamage(_enemy.Attack, _player.Constitution);
            _player.Health -= enemyDamage;
            result += $"{_enemy.Name} deals {enemyDamage} damage to {_player.Name}.\n";

            if (_player.Health <= 0)
            {
                result += $"{_player.Name} has been defeated!\n";
            }

            return result;
        }
    }
}