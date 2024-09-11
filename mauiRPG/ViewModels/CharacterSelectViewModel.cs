using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using System.Diagnostics;

namespace mauiRPG.ViewModels
{
    public partial class CharacterSelectViewModel : ObservableObject
    {
        private readonly CharacterService _characterService;
        private readonly GameStateService _gameStateService;

        [ObservableProperty]
        private Character? _selectedCharacter;

        public ObservableCollection<Character> Characters { get; } = [];

        public event EventHandler? CloseRequested;
        public event EventHandler<Character>? CharacterSelected;
        public event EventHandler<string>? ErrorOccurred;

        public CharacterSelectViewModel(CharacterService characterService, GameStateService gameStateService)
        {
            _characterService = characterService;
            _gameStateService = gameStateService;
            LoadCharacters();
        }

        private void LoadCharacters()
        {
            try
            {
                var loadedCharacters = _characterService.LoadCharacters();
                Characters.Clear();
                foreach (var character in loadedCharacters)
                {
                    Characters.Add(character);
                }
                Debug.WriteLine($"Loaded {Characters.Count} characters in CharacterSelectViewModel");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading characters: {ex.Message}");
                ErrorOccurred?.Invoke(this, "Failed to load characters. Please try again.");
            }
        }

        [RelayCommand]
        private void LoadCharacter()
        {
            if (SelectedCharacter == null)
            {
                Debug.WriteLine("No character selected");
                ErrorOccurred?.Invoke(this, "Please select a character before loading.");
                return;
            }

            if (SelectedCharacter is Player player)
            {
                _gameStateService.CurrentPlayer = player;
                Debug.WriteLine($"Current player set to: {player.Name}");
                CharacterSelected?.Invoke(this, player);
            }
            else
            {
                Debug.WriteLine($"Selected character is not a valid player. Type: {SelectedCharacter.GetType().Name}");
                ErrorOccurred?.Invoke(this, "The selected character is not a valid player. Please select a different character.");
            }
        }

        [RelayCommand]
        private void ClosePopup()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

    }
}