using CommunityToolkit.Maui.Views;
using mauiRPG.ViewModels;
using mauiRPG.Services;

namespace mauiRPG.Views
{
    public partial class MainMenuView : ContentPage
    {
        private Popup? _currentPopup;
        private readonly MainViewModel _viewModel;
        private readonly ISettingsService _settingsService;
        private readonly CharacterService _characterService;

        public MainMenuView(CharacterService characterService, ISettingsService settingsService)
        {
            InitializeComponent();
            _characterService = characterService;
            _settingsService = settingsService;
            _viewModel = new MainViewModel(_characterService);
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
            var characterSelectViewModel = new CharacterSelectViewModel(_characterService);
            _currentPopup = new CharacterSelectPopup(characterSelectViewModel);
            await this.ShowPopupAsync(_currentPopup);
        }

        private async void OnShowSettingsPopupRequested(object? sender, EventArgs e)
        {
            _currentPopup = new SettingsSelectPopup(_settingsService);
            await this.ShowPopupAsync(_currentPopup);
        }
    }
}