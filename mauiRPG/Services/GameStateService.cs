using mauiRPG.Models;
using Microsoft.Extensions.Logging;

namespace mauiRPG.Services
{
    public class GameStateService(ILogger<GameStateService> logger)
    {
        private Player? _currentPlayer;

        public event EventHandler? CharacterLoaded;
        public event EventHandler? CharacterUnloaded;

        public Player? CurrentPlayer
        {
            get
            {
                logger.LogInformation("CurrentPlayer accessed. Value is {CurrentPlayerStatus}", _currentPlayer?.Name ?? "null");
                return _currentPlayer;
            }
            private set
            {
                _currentPlayer = value;
                logger.LogInformation("CurrentPlayer set to {CurrentPlayerStatus}", _currentPlayer?.Name ?? "null");
            }
        }

        public void LoadCharacter(Player character)
        {
            CurrentPlayer = character;
            logger.LogInformation("Character loaded: {CharacterName}", character.Name);
            CharacterLoaded?.Invoke(this, EventArgs.Empty);
            logger.LogInformation("CharacterLoaded event invoked");
        }

        public void UnloadCharacter()
        {
            logger.LogInformation("Character unloaded: {CharacterName}", CurrentPlayer?.Name ?? "null");
            CurrentPlayer = null;
            CharacterUnloaded?.Invoke(this, EventArgs.Empty);
            logger.LogInformation("CharacterUnloaded event invoked");
        }

        public bool IsCharacterLoaded => CurrentPlayer != null;

        public void SetCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
            logger.LogInformation("SetCurrentPlayer called with player: {PlayerName}", player.Name);
            CharacterLoaded?.Invoke(this, EventArgs.Empty);
            logger.LogInformation("CharacterLoaded event invoked from SetCurrentPlayer");
        }
    }
}