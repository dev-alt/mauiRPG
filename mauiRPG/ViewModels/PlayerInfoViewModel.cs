using System.ComponentModel;
using System.Runtime.CompilerServices;
using mauiRPG.Models;

namespace mauiRPG.ViewModels
{
    public class PlayerInfoViewModel : INotifyPropertyChanged
    {
        private Player _player = null!;

        public Player Player
        {
            get => _player;
            set
            {
                if (_player == value) return;
                _player = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(RaceName));
                OnPropertyChanged(nameof(ClassName));
                OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(Health));
                OnPropertyChanged(nameof(Strength));
                OnPropertyChanged(nameof(Intelligence));
                OnPropertyChanged(nameof(Dexterity));
                OnPropertyChanged(nameof(Constitution));
            }
        }

        public string Name => Player?.Name ?? "N/A";
        public string RaceName => Player?.Race.ToString() ?? "N/A";
        public string ClassName => Player?.Class.ToString() ?? "N/A";
        public string Level => Player?.Level.ToString() ?? "N/A";
        public string Health => Player?.Health.ToString() ?? "N/A";
        public string Strength => Player?.Strength.ToString() ?? "N/A";
        public string Intelligence => Player?.Intelligence.ToString() ?? "N/A";
        public string Dexterity => Player?.Dexterity.ToString() ?? "N/A";
        public string Constitution => Player?.Constitution.ToString() ?? "N/A";

        public PlayerInfoViewModel(Player player = null!)
        {
            Player = player;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}