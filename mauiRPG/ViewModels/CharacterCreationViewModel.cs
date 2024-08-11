using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;

namespace mauiRPG.ViewModels
{
    public partial class CharacterCreationViewModel : INotifyPropertyChanged
    {
        private readonly CharacterService _characterService;
        private readonly GameStateService _gameStateService;
        private readonly ILogger<CharacterCreationViewModel> _logger;

        public ObservableCollection<Race> Races { get; }
        public ObservableCollection<Class> Classes { get; }
        public ICommand CreateCharacterCommand { get; }
        public ICommand ClosePopupCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler CloseRequested;
        private string _name = "Name";
        private Race _selectedRace = null!;
        private Class _selectedClass = null!;


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
            get => _selectedRace;
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


        public CharacterCreationViewModel(CharacterService characterService, GameStateService gameStateService,
            ILogger<CharacterCreationViewModel> logger)
        {
            _characterService = characterService;
            _gameStateService = gameStateService;
            _logger = logger;

            Races = new ObservableCollection<Race>
            {
                new Orc(),
                new Human(),
                new Dwarf(),
                new Elf()
            };


            Classes = new ObservableCollection<Class>
            {
                new Warrior(),
                new Mage(),
                new Rogue()
            };

            SelectedRace = Races[0];
            SelectedClass = Classes[0];
            CreateCharacterCommand = new Command(CreateCharacter);
            _logger.LogInformation("CharacterCreationViewModel initialized");
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

            if (Application.Current != null)
                if (Application.Current.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Success", "Character successfully created.", "OK");

            await Shell.Current.GoToAsync($"{nameof(LevelSelectView)}");
        }

        [RelayCommand]
        private void Close()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}