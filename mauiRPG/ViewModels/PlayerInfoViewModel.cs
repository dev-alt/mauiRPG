using System.ComponentModel;
using System.Runtime.CompilerServices;
using mauiRPG.Models;

namespace mauiRPG.ViewModels
{
    public class PlayerInfoViewModel(Player player) : INotifyPropertyChanged
    {
        private readonly Player _player = player ?? throw new ArgumentNullException(nameof(player));

        public string Name => _player?.Name ?? "Unknown";
        public string RaceName => _player?.Race?.Name ?? "Unknown";
        public string Level => _player?.Level.ToString() ?? "0";
        public string Health => _player?.Health.ToString() ?? "0";
        public string Strength => _player?.Strength.ToString() ?? "0";
        public string Intelligence => _player?.Intelligence.ToString() ?? "0";
        public string Dexterity => _player?.Dexterity.ToString() ?? "0";
        public string Constitution => _player?.Constitution.ToString() ?? "0";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}