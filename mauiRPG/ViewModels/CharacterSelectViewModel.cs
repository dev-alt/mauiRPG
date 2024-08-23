using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;

namespace mauiRPG.ViewModels
{
    public partial class CharacterSelectViewModel : ObservableObject
    {
        private readonly CharacterService _characterService;
        private readonly GameStateService _gameStateService;

        [ObservableProperty]
        private Character? _selectedCharacter;

        public ObservableCollection<Character> Characters { get; }

        public event EventHandler? CloseRequested;

        public CharacterSelectViewModel(CharacterService characterService, GameStateService gameStateService)
        {
            _characterService = characterService;
            _gameStateService = gameStateService;
            Characters = new ObservableCollection<Character>(_characterService.LoadCharacters());
        }

        [RelayCommand]
        private async Task LoadCharacter()
        {
            if (SelectedCharacter == null)
            {
                await Shell.Current.DisplayAlert("Error", "No character selected.", "OK");
                return;
            }

            _gameStateService.CurrentPlayer = (Player)SelectedCharacter;
            CloseRequested?.Invoke(this, EventArgs.Empty);
            await Shell.Current.GoToAsync($"{nameof(LevelSelectView)}");
        }

        [RelayCommand]
        private void ClosePopup()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}