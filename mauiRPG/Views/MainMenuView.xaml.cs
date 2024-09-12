using CommunityToolkit.Maui.Views;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.ViewModels;
using System.Diagnostics;

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
            try
            {
                AnimateElements();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnPageLoaded: {ex.Message}");
                _ = ShowErrorPopup("An error occurred while loading the page.");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.ShowCharacterPopupRequested += OnShowCharacterPopupRequested;
            _viewModel.ShowSettingsPopupRequested += OnShowSettingsPopupRequested;
            _viewModel.ShowErrorPopupRequested += OnShowErrorPopupRequested;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.ShowCharacterPopupRequested -= OnShowCharacterPopupRequested;
            _viewModel.ShowSettingsPopupRequested -= OnShowSettingsPopupRequested;
            _viewModel.ShowErrorPopupRequested -= OnShowErrorPopupRequested;
        }

        private async void AnimateElements()
        {
            try
            {
                var animationTasks = new[]
                {
                    FadeInBackground(),
                    ScaleUpLogo(),
                    FadeInTitle()
                };

                await Task.WhenAll(animationTasks);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AnimateElements: {ex.Message}");
                await ShowErrorPopup("An error occurred while animating elements.");
            }
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
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnShowCharacterPopupRequested: {ex.Message}");
                await ShowErrorPopup("An error occurred while showing the character selection popup.");
            }
        }

        private async void OnCharacterSelectErrorOccurred(object? sender, string errorMessage)
        {
            await ShowErrorPopup(errorMessage);
        }

        private async void OnShowSettingsPopupRequested(object? sender, EventArgs e)
        {
            try
            {
                _currentPopup = new SettingsSelectPopup(_settingsService);
                await ShowPopupSafely(_currentPopup);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnShowSettingsPopupRequested: {ex.Message}");
                await ShowErrorPopup("An error occurred while showing the settings popup.");
            }
        }

        private async Task<object?> ShowPopupSafely(Popup popup)
        {
            try
            {
                if (Application.Current?.MainPage == null)
                {
                    Debug.WriteLine("MainPage is null");
                    await ShowErrorPopup("An error occurred while loading the popup.");
                    return null;
                }

                var result = await Application.Current.MainPage.ShowPopupAsync(popup);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to show popup: {ex.Message}");
                await ShowErrorPopup("An error occurred while loading the popup.");
                return null;
            }
        }

        private async void OnShowErrorPopupRequested(object? sender, string errorMessage)
        {
            await ShowErrorPopup(errorMessage);
        }

        private async Task ShowErrorPopup(string errorMessage)
        {
            try
            {
                var errorPopup = new ErrorPopup(errorMessage);
                await Application.Current?.MainPage?.ShowPopupAsync(errorPopup)!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error showing error popup: {ex.Message}");
                await DisplayAlert("Error", errorMessage, "OK");
            }
        }
    }
}