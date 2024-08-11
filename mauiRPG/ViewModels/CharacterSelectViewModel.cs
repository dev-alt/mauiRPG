using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;

namespace mauiRPG.ViewModels
{
    public partial class CharacterSelectViewModel : ObservableObject
    {
        private readonly CharacterService _characterService;

        [ObservableProperty]
        private Character _selectedCharacter;

        public ObservableCollection<Character> Characters { get; }

        public event EventHandler CloseRequested;

        public CharacterSelectViewModel(CharacterService characterService)
        {
            _characterService = characterService;
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

            await Shell.Current.DisplayAlert("Character Loaded", $"Loaded character: {SelectedCharacter.Name}", "OK");
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        private void ClosePopup()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}