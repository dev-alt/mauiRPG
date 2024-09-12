using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using System;
using System.Collections.ObjectModel;
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
            _characterService = characterService ?? throw new ArgumentNullException(nameof(characterService));
            _gameStateService = gameStateService ?? throw new ArgumentNullException(nameof(gameStateService));
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
        private async Task LoadCharacter()
        {
            try
            {
                if (SelectedCharacter == null)
                {
                    Debug.WriteLine("No character selected");
                    ErrorOccurred?.Invoke(this, "Please select a character before loading.");
                    return;
                }

                if (SelectedCharacter is Player player)
                {
                    _gameStateService.SetCurrentPlayer(player);
                    Debug.WriteLine($"Current player set to: {player.Name}");
                    CharacterSelected?.Invoke(this, player);
                    await Shell.Current.GoToAsync("///LevelSelect");
                    CloseRequested?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Debug.WriteLine($"Selected character is not a valid player. Type: {SelectedCharacter.GetType().Name}");
                    ErrorOccurred?.Invoke(this, "The selected character is not a valid player. Please select a different character.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading character: {ex.Message}");
                ErrorOccurred?.Invoke(this, "An error occurred while loading the character. Please try again.");
            }
        }

        [RelayCommand]
        private void DeleteCharacter()
        {
            try
            {
                if (SelectedCharacter == null)
                {
                    ErrorOccurred?.Invoke(this, "Please select a character to delete.");
                    return;
                }

                if (_characterService.DeleteCharacter(SelectedCharacter.Name))
                {
                    Characters.Remove(SelectedCharacter);
                    SelectedCharacter = null;
                }
                else
                {
                    ErrorOccurred?.Invoke(this, "Failed to delete the character. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting character: {ex.Message}");
                ErrorOccurred?.Invoke(this, "An error occurred while deleting the character. Please try again.");
            }
        }

        [RelayCommand]
        private void ClosePopup()
        {
            try
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error closing popup: {ex.Message}");
                ErrorOccurred?.Invoke(this, "An error occurred while closing the popup. Please try again.");
            }
        }
    }
}