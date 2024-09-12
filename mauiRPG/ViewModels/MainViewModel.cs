using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace mauiRPG.ViewModels
{
    public partial class MainViewModel : ObservableObject, IRecipient<PopupClosedMessage>
    {
        private readonly CharacterService _characterService;
        private readonly ISettingsService _settingsService;
        private readonly GameStateService _gameStateService;

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
        public event EventHandler<string>? ShowErrorPopupRequested;

        public MainViewModel(CharacterService characterService, ISettingsService settingsService, GameStateService gameStateService)
        {
            _characterService = characterService;
            _settingsService = settingsService;
            _gameStateService = gameStateService;
            WeakReferenceMessenger.Default.Register<PopupClosedMessage>(this);
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
                Debug.WriteLine($"Loaded {Characters.Count} characters");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading characters: {ex.Message}");
                ShowErrorPopupRequested?.Invoke(this, "Failed to load characters. Please try again.");
            }
        }

        [RelayCommand]
        private async Task CreateNewCharacter()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(CharacterSelect));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to character creation: {ex.Message}");
                ShowErrorPopupRequested?.Invoke(this, "Failed to open character creation. Please try again.");
            }
        }

        [RelayCommand]
        private void ShowLoadCharacter()
        {
            try
            {
                if (Characters.Count == 0)
                {
                    ShowErrorPopupRequested?.Invoke(this, "No saved characters found. Please create a new character.");
                    return;
                }
                IsCharacterListVisible = true;
                IsAnyPopupVisible = true;
                ShowCharacterPopupRequested?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error showing load character: {ex.Message}");
                ShowErrorPopupRequested?.Invoke(this, "Failed to load character selection. Please try again.");
            }
        }

        [RelayCommand]
        private async Task LoadSelectedCharacter()
        {
            try
            {
                if (SelectedCharacter == null)
                {
                    ShowErrorPopupRequested?.Invoke(this, "No character selected.");
                    return;
                }

                if (SelectedCharacter is Player player)
                {
                    _gameStateService.SetCurrentPlayer(player);
                    Debug.WriteLine($"Current player set to: {player.Name}");
                    IsCharacterListVisible = false;
                    IsAnyPopupVisible = false;
                    await Shell.Current.GoToAsync($"//{nameof(LevelSelectView)}");
                }
                else
                {
                    ShowErrorPopupRequested?.Invoke(this, "Selected character is not a valid player.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading selected character: {ex.Message}");
                ShowErrorPopupRequested?.Invoke(this, "Failed to load the selected character. Please try again.");
            }
        }

        [RelayCommand]
        private void ShowOpenSettings()
        {
            try
            {
                IsSettingsVisible = true;
                IsAnyPopupVisible = true;
                ShowSettingsPopupRequested?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening settings: {ex.Message}");
                ShowErrorPopupRequested?.Invoke(this, "Failed to open settings. Please try again.");
            }
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
            try
            {
                Application.Current?.Quit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error exiting application: {ex.Message}");
            }
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