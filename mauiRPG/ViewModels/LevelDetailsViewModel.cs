using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mauiRPG.Models;

namespace mauiRPG.ViewModels
{
    public class LevelDetailsViewModel : INotifyPropertyChanged
    {
        private Level _level;

        public Level Level
        {
            get => _level;
            set
            {
                _level = value;
                OnPropertyChanged(nameof(Level));
            }
        }

        public LevelDetailsViewModel(Level level)
        {
            Level = level;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
