using mauiRPG.Models;
using Microsoft.Extensions.Logging;

namespace mauiRPG.Services
{
    public class GameStateService(ILogger<GameStateService> logger)
    {
        private Player _currentPlayer = null!;

        public Player CurrentPlayer
        {
            get
            {
                logger.LogInformation("CurrentPlayer accessed. Value is {CurrentPlayerStatus}", $"set to {_currentPlayer.Name}");
                return _currentPlayer;
            }
            set
            {
                _currentPlayer = value;
                logger.LogInformation("CurrentPlayer set to {CurrentPlayerStatus}", $"{_currentPlayer.Name}");
            }
        }
    }
}