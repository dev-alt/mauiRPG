using CommunityToolkit.Mvvm.ComponentModel;
using mauiRPG.Models;

namespace mauiRPG.ViewModels
{
    public partial class LevelViewModel(Level level) : ObservableObject
    {
        [ObservableProperty]
        private Level _level = level;
    }
}