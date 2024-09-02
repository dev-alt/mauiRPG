using mauiRPG.Models;
using mauiRPG.Services;

namespace mauiRPG.Controllers
{
    public class GameManager
    {
        private readonly PlayerController _playerController;
        private readonly EnemyController _enemyController;
        private readonly CombatManager _combatManager;
        private readonly GameStateService _gameStateService;

        public GameManager(
            PlayerController playerController,
            EnemyController enemyController,
            CombatService combatService,
            GameStateService gameStateService)
        {
            _playerController = playerController;
            _enemyController = enemyController;
            _gameStateService = gameStateService;
            _combatManager = new CombatManager(_playerController.GetPlayer(), null, combatService);
        }

        public async Task InitializeGameAsync()
        {
            await _playerController.LoadPlayerAsync();
            await _enemyController.LoadEnemiesAsync();
        }

        public async Task StartNewGameAsync()
        {
            await InitializeGameAsync();
            _gameStateService.ResetGameState();
        }

        public async Task SaveGameAsync()
        {
            await _gameStateService.SaveGameStateAsync(_playerController.GetPlayer());
        }

        public async Task LoadGameAsync()
        {
            var savedState = await _gameStateService.LoadGameStateAsync();
            if (savedState != null)
            {
                _playerController.UpdatePlayer(savedState);
            }
        }

        public string StartCombat()
        {
            var enemy = _enemyController.GetRandomEnemy();
            _combatManager = new CombatManager(_playerController.GetPlayer(), enemy, new CombatService());
            return $"Combat started against {enemy.Name}!";
        }

        public string ExecuteCombatTurn()
        {
            if (_combatManager == null)
            {
                return "No active combat. Start a new combat first.";
            }

            string turnResult = _combatManager.ExecuteTurn();

            if (_combatManager.IsCombatOver())
            {
                string result = _combatManager.GetCombatResult();
                _combatManager.ResetCombat();
                return turnResult + "\n" + result;
            }

            return turnResult;
        }

        public Player GetCurrentPlayer()
        {
            return _playerController.GetPlayer();
        }
    }
}
