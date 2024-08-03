using mauiRPG.Models;
using mauiRPG.ViewModels;

namespace mauiRPG.Views;

[QueryProperty(nameof(LevelNumber), "levelNumber")]
public partial class LevelPage : ContentPage
{
    private int _levelNumber;
    public int LevelNumber
    {
        get => _levelNumber;
        set
        {
            _levelNumber = value;
            OnPropertyChanged();
            LoadLevel(_levelNumber);
        }
    }

    public LevelPage()
    {
        InitializeComponent();
    }

    private void LoadLevel(int levelNumber)
    {
        // Here you would typically load the level data based on the level number
        // For now, we'll just create a dummy Level object
        var level = new Level
        {
            Number = levelNumber,
            Name = $"Level {levelNumber}",
            IsUnlocked = true,
            ImageSource = $"level{levelNumber}.png"
        };

        BindingContext = new LevelDetailsViewModel(level);
    }
}