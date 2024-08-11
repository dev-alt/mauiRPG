using System.Windows.Input;
using mauiRPG.Services;
using mauiRPG.Views;
using System.Collections.ObjectModel;
using mauiRPG.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace mauiRPG.ViewModels
{
    public partial class MainViewModel : ObservableObject, IRecipient<PopupClosedMessage>
    {
        private readonly CharacterService _characterService;
        private bool _isCharacterListVisible;
        private bool _isSettingsVisible;
        private Character _selectedCharacter = null!;
        private Popup _currentPopup;
        private readonly MainViewModel _viewModel;
        #region Commands
        public ICommand CreateNewCharacterCommand { get; }
        public ICommand ShowLoadCharacterCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand LoadCharacterCommand { get; }
        public ICommand CancelLoadCharacterCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand CancelSettingsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ClosePopupCommand { get; }
        public event EventHandler? ShowCharacterPopupRequested;
        public event EventHandler? ShowSettingsPopupRequested;
        #endregion
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
        [ObservableProperty]
        private bool _isAnyPopupVisible;

        public Character SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(CharacterService characterService)
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
            ClosePopupCommand = new Command(ClosePopups);
            WeakReferenceMessenger.Default.Register<PopupClosedMessage>(this);
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
            IsAnyPopupVisible = true;
            ShowCharacterPopupRequested?.Invoke(this, EventArgs.Empty);
        }

        private void ShowOpenSettings()
        {
            IsSettingsVisible = true;
            IsAnyPopupVisible = true;
            ShowSettingsPopupRequested?.Invoke(this, EventArgs.Empty);
        }
        public void Receive(PopupClosedMessage message)
        {
            IsAnyPopupVisible = false;
        }
        public void ClosePopups()
        {
            IsCharacterListVisible = false;
            IsSettingsVisible = false;
            IsAnyPopupVisible = false;
        }

        private async Task LoadCharacter()
        {
            if (SelectedCharacter == null)
            {
                await Shell.Current.DisplayAlert("Error", "No character selected.", "OK");
                return;
            }

            await Shell.Current.DisplayAlert("Character Loaded", $"Loaded character: {SelectedCharacter.Name}", "OK");
            IsCharacterListVisible = false;
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