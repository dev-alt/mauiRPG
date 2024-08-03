using mauiRPG.Models;
using Microsoft.Extensions.Logging;

namespace mauiRPG.Services
{
    public class GameStateService
    {
        private readonly ILogger<GameStateService> _logger;
        private Player _currentPlayer = null!;

        public GameStateService(ILogger<GameStateService> logger)
        {
            _logger = logger;
        }

        public Player CurrentPlayer
        {
            get
            {
                _logger.LogInformation("CurrentPlayer accessed. Value is {CurrentPlayerStatus}", $"set to {_currentPlayer.Name}");
                return _currentPlayer;
            }
            set
            {
                _currentPlayer = value;
                _logger.LogInformation("CurrentPlayer set to {CurrentPlayerStatus}", $"{_currentPlayer.Name}");
            }
        }
    }
}