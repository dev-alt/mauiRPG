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

        public CharacterSelectViewModel(CharacterService characterService, GameStateService gameStateService)
        {
            _characterService = characterService;
            _gameStateService = gameStateService;
            LoadCharacters();
        }

        private void LoadCharacters()
        {
            var loadedCharacters = _characterService.LoadCharacters();
            Characters.Clear();
            foreach (var character in loadedCharacters)
            {
                Characters.Add(character);
            }
            Debug.WriteLine($"Loaded {Characters.Count} characters in CharacterSelectViewModel");
        }

        [RelayCommand]
        private void LoadCharacter()
        {
            switch (SelectedCharacter)
            {
                case null:
                    Debug.WriteLine("No character selected");
                    return;
                case Player player:
                    _gameStateService.CurrentPlayer = player;
                    Debug.WriteLine($"Current player set to: {player.Name}");
                    CharacterSelected?.Invoke(this, player);
                    break;
                default:
                    Debug.WriteLine("Selected character is not a valid player");
                    break;
            }
        }

        [RelayCommand]
        private void ClosePopup()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}