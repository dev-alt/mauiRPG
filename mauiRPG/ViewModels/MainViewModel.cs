using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using System.Collections.ObjectModel;

namespace mauiRPG.ViewModels
{
    public partial class MainViewModel : ObservableObject, IRecipient<PopupClosedMessage>
    {
        private readonly CharacterService _characterService;
        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private bool _isCharacterListVisible;

        [ObservableProperty]
        private bool _isSettingsVisible;

        [ObservableProperty]
        private bool _isAnyPopupVisible;

        [ObservableProperty]
        private Character? _selectedCharacter;

        public ObservableCollection<Character> Characters { get; } = [];

        public event EventHandler? ShowCharacterPopupRequested;
        public event EventHandler? ShowSettingsPopupRequested;

        public MainViewModel(CharacterService characterService, ISettingsService settingsService)
        {
            _characterService = characterService;
            _settingsService = settingsService;
            WeakReferenceMessenger.Default.Register<PopupClosedMessage>(this);
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
        }

        [RelayCommand]
        private async Task CreateNewCharacter()
        {
            await Shell.Current.GoToAsync(nameof(CharacterSelect));
        }

        [RelayCommand]
        private void ShowLoadCharacter()
        {
            if (_characterService.LoadCharacters().Count == 0)
            {
                Shell.Current.DisplayAlert("No Characters", "No saved characters found. Please create a new character.", "OK");
                return;
            }
            IsCharacterListVisible = true;
            IsAnyPopupVisible = true;
            ShowCharacterPopupRequested?.Invoke(this, EventArgs.Empty);
        }
        [RelayCommand]
        private void ShowOpenSettings()
        {
            IsSettingsVisible = true;
            IsAnyPopupVisible = true;
            ShowSettingsPopupRequested?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        private void CancelLoadCharacter()
        {
            IsCharacterListVisible = false;
            SelectedCharacter = null;
        }

        [RelayCommand]
        private void CancelSettings()
        {
            IsSettingsVisible = false;
        }

        [RelayCommand]
        private static void Exit()
        {
            Application.Current?.Quit();
        }

        [RelayCommand]
        private void ClosePopups()
        {
            IsCharacterListVisible = false;
            IsSettingsVisible = false;
            IsAnyPopupVisible = false;
        }

        public void Receive(PopupClosedMessage message)
        {
            IsAnyPopupVisible = false;
        }
    }
}