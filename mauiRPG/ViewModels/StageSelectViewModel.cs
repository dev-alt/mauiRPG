using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using mauiRPG.Models;
using mauiRPG.Views;

namespace mauiRPG.ViewModels;

public class StageSelectViewModel
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private ObservableCollection<Level> _levels;

    public ObservableCollection<Level> Levels
    {
        get => _levels;
        set
        {
            _levels = value;
            OnPropertyChanged(nameof(Levels));
        }
    }
    public ICommand SelectLevelCommand { get; private set; }

    public StageSelectViewModel()
    {
        _levels = new ObservableCollection<Level>();
        InitializeLevels();
        SelectLevelCommand = new Command<Level>(OnLevelSelected);
    }

    private void InitializeLevels()
    {
        Levels = new ObservableCollection<Level>
        {
            new() { Number = 1, Name = "The Beginning", IsUnlocked = true, ImageSource = "level1.png" },
            new() { Number = 2, Name = "Dark Forest", IsUnlocked = true, ImageSource = "level2.png" },
            new() { Number = 3, Name = "Mystic Mountains", IsUnlocked = false, ImageSource = "level3.png" },
        };
    }

    private static async void OnLevelSelected(Level level)
    {
        if (level.IsUnlocked)
        {
            var mainPage = Application.Current?.MainPage;
            if (mainPage != null)
            {
                await mainPage.Navigation.PushAsync(new LevelPage(level));
            }
            else
            {
                Console.WriteLine("MainPage is null!");
            }
        }
        else
        {
            Console.WriteLine($"Level {level.Number} is locked!");
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}