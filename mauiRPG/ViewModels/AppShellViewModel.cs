using CommunityToolkit.Mvvm.ComponentModel;
using mauiRPG.Services;

namespace mauiRPG.ViewModels
{
    public partial class AppShellViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;

        [ObservableProperty]
        private bool _isCharacterLoaded;

        public AppShellViewModel(GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            _gameStateService.CharacterLoaded += OnCharacterLoadStateChanged;
            _gameStateService.CharacterUnloaded += OnCharacterLoadStateChanged;
            IsCharacterLoaded = _gameStateService.IsCharacterLoaded;
        }

        private void OnCharacterLoadStateChanged(object? sender, EventArgs e)
        {
            IsCharacterLoaded = _gameStateService.IsCharacterLoaded;
            OnPropertyChanged(nameof(IsCharacterLoaded));
        }
    }
}