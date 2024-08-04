using System.Windows.Input;
using mauiRPG.Services;
using mauiRPG.Views;
using System.Collections.ObjectModel;
using mauiRPG.Models;

namespace mauiRPG.ViewModels
{
    public class MainViewModel : BindableObject
    {
        private readonly CharacterService _characterService;
        private bool _isCharacterListVisible;
        private bool _isSettingsVisible;
        private Character _selectedCharacter = null!;

        public ICommand CreateNewCharacterCommand { get; }
        public ICommand ShowLoadCharacterCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand LoadCharacterCommand { get; }
        public ICommand CancelLoadCharacterCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand CancelSettingsCommand { get; }

        public ICommand ExitCommand { get; }
        public ObservableCollection<Character> Characters { get; }

        public bool IsCharacterListVisible
        {
            get => _isCharacterListVisible;
            set
            {
                _isCharacterListVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsSettingsVisible
        {
            get => _isSettingsVisible;
            set
            {
                _isSettingsVisible = value;
                OnPropertyChanged();
            }
        }
        public Character SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            _characterService = new CharacterService();
            Characters = new ObservableCollection<Character>(_characterService.LoadCharacters());

            CreateNewCharacterCommand = new Command(async () => await CreateNewCharacter());
            ShowLoadCharacterCommand = new Command(ShowLoadCharacter);
            LoadCharacterCommand = new Command(async () => await LoadCharacter());
            CancelLoadCharacterCommand = new Command(CancelLoadCharacter);
            ShowSettingsCommand = new Command(ShowOpenSettings);
            SettingsCommand = new Command(async () => await OpenSettings());
            CancelSettingsCommand = new Command(CancelSettings);
            ExitCommand = new Command(Exit);
        }

        private void ShowOpenSettings()
        {
            IsSettingsVisible = true;
        }

        private async Task CreateNewCharacter()
        {
            await Shell.Current.GoToAsync(nameof(CharacterSelect));
        }

        private void ShowLoadCharacter()
        {
            if (Characters.Count == 0)
            {
                Shell.Current.DisplayAlert("No Characters", "No saved characters found. Please create a new character.", "OK");
                return;
            }
            IsCharacterListVisible = true;
        }

        private async Task LoadCharacter()
        {
            await Shell.Current.DisplayAlert("Character Loaded", $"Loaded character: {SelectedCharacter.Name}", "OK");
            IsCharacterListVisible = false;
            // TODO: Implement logic to start the game with the loaded character
        }

        private void CancelLoadCharacter()
        {
            IsCharacterListVisible = false;
            SelectedCharacter = null!;
        }
        private void CancelSettings() 
        {
            IsSettingsVisible = false;
        }
        private Task OpenSettings()
        {
            IsSettingsVisible = false;
            return Task.CompletedTask;
        }

        private void Exit()
        {
            Application.Current?.Quit();
        }
    }
}