using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;

namespace mauiRPG.ViewModels
{
    internal class CharacterCreationViewModel : INotifyPropertyChanged
    {
        private readonly CharacterService _characterService;
        private string _name = "Default Name";
        private IRace _selectedRace = new Human { RaceName = "Human" };
        private IClass _selectedClass = new Mage();
        private readonly GameStateService _gameStateService;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        public IRace SelectedRace
        {
            get => _selectedRace;
            set
            {
                _selectedRace = value;
                OnPropertyChanged(nameof(SelectedRace));
            }
        }

        public IClass SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
            }
        }

        public ObservableCollection<IRace> Races { get; }
        public ObservableCollection<IClass> Classes { get; }

        public ICommand CreateCharacterCommand { get; }
        public CharacterCreationViewModel(CharacterService characterService, GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            _characterService = characterService;
            Races = new ObservableCollection<IRace>
            {
                new Orc { RaceName = "Orc" },
                new Human { RaceName = "Human" },
                new Dwarf { RaceName = "Dwarf" },
                new Elf { RaceName = "Elf" }
            };

            Classes = new ObservableCollection<IClass>
            {
                new Mage()

            };

            CreateCharacterCommand = new Command(CreateCharacter);
        }
        private async void CreateCharacter()
        {
            // Validate the inputs
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Application.Current?.MainPage?.DisplayAlert("Error", "Please enter a valid name, and select a race and class.", "OK")!;
                return;
            }


            var player = new Player
            {
                Name = Name,
                Race = SelectedRace,
                RaceName = SelectedRace.RaceName,
                Class = SelectedClass,
                ClassName = SelectedClass.ClassName,
                Level = 1,
                Health = 100,
                Strength = 10 + SelectedRace.StrengthBonus,
                Intelligence = 10 + SelectedRace.IntelligenceBonus,
                Dexterity = 10,
                Constitution = 10
            };

            _characterService.SaveCharacter(player);

            await Application.Current?.MainPage?.DisplayAlert("Success", "Character successfully created.", "OK")!;

            await Shell.Current.GoToAsync("LevelSelectView");

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}