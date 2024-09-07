using CommunityToolkit.Mvvm.ComponentModel;
using mauiRPG.Models;

namespace mauiRPG.ViewModels
{
    public partial class PlayerInfoViewModel : ObservableObject
    {
        private readonly Player _player;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string raceName;

        [ObservableProperty]
        private string className;

        [ObservableProperty]
        private string level;

        [ObservableProperty]
        private string health;

        [ObservableProperty]
        private string strength;

        [ObservableProperty]
        private string intelligence;

        [ObservableProperty]
        private string dexterity;

        [ObservableProperty]
        private string constitution;

        public PlayerInfoViewModel(Player player)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            UpdateProperties();
        }

        private void UpdateProperties()
        {
            Name = _player.Name ?? "Unknown";
            RaceName = _player.Race?.Name ?? "Unknown";
            ClassName = _player.Class?.Name ?? "Unknown";
            Level = _player.Level.ToString();
            Health = _player.Health.ToString();
            Strength = _player.Strength.ToString();
            Intelligence = _player.Intelligence.ToString();
            Dexterity = _player.Dexterity.ToString();
            Constitution = _player.Constitution.ToString();
        }
    }
}