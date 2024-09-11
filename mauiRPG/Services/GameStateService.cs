using mauiRPG.Models;
using Microsoft.Extensions.Logging;
using System;

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
                logger.LogInformation("CurrentPlayer accessed. Value is {CurrentPlayerStatus}", $"set to {_currentPlayer?.Name ?? "null"}");
                return _currentPlayer;
            }
            private set
            {
                _currentPlayer = value;
                logger.LogInformation("CurrentPlayer set to {CurrentPlayerStatus}", $"{_currentPlayer?.Name ?? "null"}");
            }
        }

        public void LoadCharacter(Player character)
        {
            CurrentPlayer = character;
            logger.LogInformation("Character loaded: {CharacterName}", character.Name);
            CharacterLoaded?.Invoke(this, EventArgs.Empty);
        }

        public void UnloadCharacter()
        {
            logger.LogInformation("Character unloaded: {CharacterName}", CurrentPlayer?.Name ?? "null");
            CurrentPlayer = null;
            CharacterUnloaded?.Invoke(this, EventArgs.Empty);
        }

        public bool IsCharacterLoaded => CurrentPlayer != null;

        // Public method to set CurrentPlayer
        public void SetCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
        }
    }
}