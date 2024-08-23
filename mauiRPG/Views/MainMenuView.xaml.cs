using CommunityToolkit.Maui.Views;
using mauiRPG.ViewModels;
using mauiRPG.Services;
using System.Diagnostics;
using mauiRPG.Models;

namespace mauiRPG.Views
{
    public partial class MainMenuView : ContentPage
    {
        private Popup? _currentPopup;
        private readonly MainViewModel _viewModel;
        private readonly CharacterService _characterService;
        private readonly ISettingsService _settingsService;
        private readonly GameStateService _gameStateService;

        public MainMenuView(CharacterService characterService, ISettingsService settingsService, GameStateService gameStateService)
        {
            InitializeComponent();
            _characterService = characterService;
            _settingsService = settingsService;
            _gameStateService = gameStateService;
            _viewModel = new MainViewModel(characterService, settingsService, gameStateService);
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.ShowCharacterPopupRequested += OnShowCharacterPopupRequested;
            _viewModel.ShowSettingsPopupRequested += OnShowSettingsPopupRequested;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.ShowCharacterPopupRequested -= OnShowCharacterPopupRequested;
            _viewModel.ShowSettingsPopupRequested -= OnShowSettingsPopupRequested;
        }

        private async void OnShowCharacterPopupRequested(object? sender, EventArgs e)
        {
            var characterSelectViewModel = new CharacterSelectViewModel(_characterService, _gameStateService);
            _currentPopup = new CharacterSelectPopup(characterSelectViewModel);
            var result = await this.ShowPopupAsync(_currentPopup);

            if (result is Character selectedCharacter)
            {
                Debug.WriteLine($"Character selected from popup: {selectedCharacter.Name}");
                await _viewModel.LoadSelectedCharacterCommand.ExecuteAsync(selectedCharacter);
            }
        }

        private async void OnShowSettingsPopupRequested(object? sender, EventArgs e)
        {
            _currentPopup = new SettingsSelectPopup(_settingsService);
            await this.ShowPopupAsync(_currentPopup);
        }
    }
}