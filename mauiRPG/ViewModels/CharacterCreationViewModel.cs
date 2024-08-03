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
using Microsoft.Extensions.Logging;

namespace mauiRPG.ViewModels
{
    public class CharacterCreationViewModel : INotifyPropertyChanged
    {
        private readonly CharacterService _characterService;
        private readonly GameStateService _gameStateService;
        private readonly ILogger<CharacterCreationViewModel> _logger;

        private string _name = "Default Name";
        private Race _selectedRace;
        private Class _selectedClass;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        public Race SelectedRace
        {
            get => (Race)_selectedRace;
            set
            {
                _selectedRace = value;
                OnPropertyChanged(nameof(SelectedRace));
            }
        }

        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
            }
        }

        public ObservableCollection<Race> Races { get; }
        public ObservableCollection<Class> Classes { get; }

        public ICommand CreateCharacterCommand { get; }
        public CharacterCreationViewModel(CharacterService characterService, GameStateService gameStateService, ILogger<CharacterCreationViewModel> logger)
        {
            _characterService = characterService;
            _gameStateService = gameStateService;
            _logger = logger;

            Races = new ObservableCollection<Race>
            {
                new Orc { },
                new Human {},
                new Dwarf { },
                new Elf {  }
            };

            Classes = new ObservableCollection<Class>
            {
                new Class { },
            };


            CreateCharacterCommand = new Command(CreateCharacter);

            _logger.LogInformation("CharacterCreationViewModel initialized");
        }
        private async void CreateCharacter()
        {
            // Validate the inputs
            if (string.IsNullOrWhiteSpace(Name) || SelectedRace == null || SelectedClass == null)
            {
                await Application.Current?.MainPage?.DisplayAlert("Error", "Please enter a valid name, and select a race and class.", "OK")!;
                return;
            }

            var player = new Player
            {
                Name = Name,
                Race = SelectedRace,
                Class = SelectedClass,
                Level = 1,
                Health = 100,
                Strength = 10,
                Intelligence = 10 ,
                Dexterity = 10,
                Constitution = 10
            };

            _characterService.SavePlayer(player);
            _gameStateService.CurrentPlayer = player;

            // Ensure player.Name is not null
            _logger.LogInformation("Character created and set as CurrentPlayer: {PlayerName}", player.Name);

            await Application.Current.MainPage.DisplayAlert("Success", "Character successfully created.", "OK");

            await Shell.Current.GoToAsync($"{nameof(LevelSelectView)}");
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}