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
            Loaded += OnPageLoaded;
        }

        private void OnPageLoaded(object? sender, EventArgs e)
        {
            AnimateElements();
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

        private async void AnimateElements()
        {
            var animationTasks = new[]
            {
                FadeInBackground(),
                ScaleUpLogo(),
                FadeInTitle()
            };

            await Task.WhenAll(animationTasks);
        }

        private async Task FadeInBackground()
        {
            for (double i = 0.7; i >= 0.5; i -= 0.01)
            {
                OverlayBox.Opacity = i;
                await Task.Delay(20);
            }
        }

        private async Task ScaleUpLogo()
        {
            for (double i = 1; i <= 1.1; i += 0.002)
            {
                GameLogo.Scale = i;
                await Task.Delay(20);
            }
        }

        private async Task FadeInTitle()
        {
            for (double i = 0; i <= 1; i += 0.02)
            {
                GameTitle.Opacity = i;
                await Task.Delay(20);
            }
        }

        private async void OnShowCharacterPopupRequested(object? sender, EventArgs e)
        {
            var characterSelectViewModel = new CharacterSelectViewModel(_characterService, _gameStateService);
            var characterSelectPopup = new CharacterSelectPopup(characterSelectViewModel);
            characterSelectPopup.ErrorOccurred += OnCharacterSelectErrorOccurred;
            _currentPopup = characterSelectPopup;

            var result = await ShowPopupSafely(_currentPopup);

            if (result is Character selectedCharacter)
            {
                Debug.WriteLine($"Character selected from popup: {selectedCharacter.Name}");
                await _viewModel.LoadSelectedCharacterCommand.ExecuteAsync(selectedCharacter);
            }
        }

        private async void OnCharacterSelectErrorOccurred(object? sender, string errorMessage)
        {
            var errorPopup = new ErrorPopup(errorMessage);
            await ShowPopupSafely(errorPopup);
        }



        private async void OnShowSettingsPopupRequested(object? sender, EventArgs e)
        {
            _currentPopup = new SettingsSelectPopup(_settingsService);
            await ShowPopupSafely(_currentPopup);
        }

        private async Task<object?> ShowPopupSafely(Popup popup)
        {
            try
            {
                if (Application.Current?.MainPage == null)
                {
                    Debug.WriteLine("MainPage is null");
                    await DisplayAlert("Error", "An error occurred while loading the popup.", "OK");
                    return null;
                }

                var result = await Application.Current.MainPage.ShowPopupAsync(popup);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to show popup: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while loading the popup.", "OK");
                return null;
            }
        }
    }
}