using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mauiRPG.Services;

namespace mauiRPG.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly ISettingsService _settingsService;

    private int _musicVolume;
    public int MusicVolume
    {
        get => _musicVolume;
        set
        {
            if (_musicVolume != value)
            {
                _musicVolume = value;
                OnPropertyChanged(nameof(MusicVolume));
            }
        }
    }

    private int _sfxVolume;
    public int SfxVolume
    {
        get => _sfxVolume;
        set
        {
            if (_sfxVolume != value)
            {
                _sfxVolume = value;
                OnPropertyChanged(nameof(SfxVolume));
            }
        }
    }

    public ObservableCollection<string> DifficultyLevels { get; } = new ObservableCollection<string> { "Easy", "Normal", "Hard" };

    private string _selectedDifficulty;
    public string SelectedDifficulty
    {
        get => _selectedDifficulty;
        set
        {
            if (_selectedDifficulty != value)
            {
                _selectedDifficulty = value;
                OnPropertyChanged(nameof(SelectedDifficulty));
            }
        }
    }

    private bool _isDarkMode;
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                OnPropertyChanged(nameof(IsDarkMode));
                OnPropertyChanged(nameof(ThemeText));
            }
        }
    }

    public string ThemeText => IsDarkMode ? "Dark Mode" : "Light Mode";

    public ICommand SaveSettingsCommand { get; }

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        LoadSettings();
        SaveSettingsCommand = new Command(SaveSettings);
    }

    private void LoadSettings()
    {
        MusicVolume = _settingsService.GetMusicVolume();
        SfxVolume = _settingsService.GetSfxVolume();
        SelectedDifficulty = _settingsService.GetDifficulty();
        IsDarkMode = _settingsService.GetTheme() == "Dark";
    }

    private void SaveSettings()
    {
        _settingsService.SetMusicVolume(MusicVolume);
        _settingsService.SetSfxVolume(SfxVolume);
        _settingsService.SetDifficulty(SelectedDifficulty);
        _settingsService.SetTheme(IsDarkMode ? "Dark" : "Light");
        // Optionally, you can show a confirmation message to the user
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
}