using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using mauiRPG.Models;

namespace mauiRPG.ViewModels;

public class StageSelectViewModel
{
    public event PropertyChangedEventHandler PropertyChanged;

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
        InitializeLevels();
        SelectLevelCommand = new Command<Level>(OnLevelSelected);
    }

    private void InitializeLevels()
    {
        Levels = new ObservableCollection<Level>
        {
            new Level { Number = 1, Name = "The Beginning", IsUnlocked = true, ImageSource = "level1.png" },
            new Level { Number = 2, Name = "Dark Forest", IsUnlocked = true, ImageSource = "level2.png" },
            new Level { Number = 3, Name = "Mystic Mountains", IsUnlocked = false, ImageSource = "level3.png" },
            // Add more levels as needed
        };
    }

    private void OnLevelSelected(Level level)
    {
        if (level.IsUnlocked)
        {
            // Navigate to the selected level
            Console.WriteLine($"Starting Level {level.Number}: {level.Name}");
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