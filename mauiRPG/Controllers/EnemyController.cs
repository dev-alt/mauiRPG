using mauiRPG.Models;
using mauiRPG.Services;
using System.Collections.ObjectModel;

namespace mauiRPG.Controllers
{
    public class EnemyController
    {
        private readonly EnemyService _enemyService;
        private ObservableCollection<Enemy> _enemies;

        public EnemyController(EnemyService enemyService)
        {
            _enemyService = enemyService;
            _enemies = new ObservableCollection<Enemy>();
        }

        public async Task LoadEnemiesAsync()
        {
            var loadedEnemies = await _enemyService.GetEnemiesAsync();
            _enemies.Clear();
            foreach (var enemy in loadedEnemies)
            {
                _enemies.Add(enemy);
            }
        }

        public Enemy GetRandomEnemy()
        {
            if (_enemies.Count == 0)
            {
                throw new InvalidOperationException("No enemies available.");
            }
            Random random = new Random();
            int index = random.Next(_enemies.Count);
            return _enemies[index];
        }

        public void UpdateEnemy(Enemy enemy)
        {
            int index = _enemies.IndexOf(enemy);
            if (index != -1)
            {
                _enemies[index] = enemy;
            }
        }

        public ObservableCollection<Enemy> GetEnemies()
        {
            return _enemies;
        }
    }
}
