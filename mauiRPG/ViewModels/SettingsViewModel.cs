using System.Windows.Input;
using mauiRPG.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace mauiRPG.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private int _musicVolume;

        [ObservableProperty]
        private int _sfxVolume;

        [ObservableProperty]
        private string _selectedDifficulty;

        [ObservableProperty]
        private bool _isDarkMode;

        public List<string> DifficultyLevels { get; } = new List<string> { "Easy", "Normal", "Hard" };

        public string ThemeText => IsDarkMode ? "Dark Mode" : "Light Mode";

        public event EventHandler CloseRequested;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            LoadSettings();
        }

        private void LoadSettings()
        {
            MusicVolume = _settingsService.GetMusicVolume();
            SfxVolume = _settingsService.GetSfxVolume();
            SelectedDifficulty = _settingsService.GetDifficulty();
            IsDarkMode = _settingsService.GetTheme() == "Dark";
        }

        [RelayCommand]
        private void SaveSettings()
        {
            _settingsService.SetMusicVolume(MusicVolume);
            _settingsService.SetSfxVolume(SfxVolume);
            _settingsService.SetDifficulty(SelectedDifficulty);
            _settingsService.SetTheme(IsDarkMode ? "Dark" : "Light");
            // Optionally close the popup after saving
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        private void Close()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}