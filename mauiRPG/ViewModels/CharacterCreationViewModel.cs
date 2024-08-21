using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;

namespace mauiRPG.ViewModels
{
    public partial class CharacterCreationViewModel : ObservableObject
    {
        private readonly CharacterService _characterService;
        private readonly GameStateService _gameStateService;
        private readonly ILogger<CharacterCreationViewModel> _logger;

        public ObservableCollection<Race> Races { get; }
        public ObservableCollection<Class> Classes { get; }

        public event EventHandler? CloseRequested;

        [ObservableProperty]
        private string _name = "Name";

        [ObservableProperty]
        private Race _selectedRace;

        [ObservableProperty]
        private Class _selectedClass;

        public CharacterCreationViewModel(CharacterService characterService, GameStateService gameStateService,
            ILogger<CharacterCreationViewModel> logger)
        {
            _characterService = characterService;
            _gameStateService = gameStateService;
            _logger = logger;

            Races =
            [
                new Orc(),
                new Human(),
                new Dwarf(),
                new Elf()
            ];

            Classes =
            [
                new Warrior(),
                new Mage(),
                new Rogue()
            ];

            SelectedRace = Races[0];
            SelectedClass = Classes[0];

            _logger.LogInformation("CharacterCreationViewModel initialized");
        }

        [RelayCommand]
        private async Task CreateCharacter()
        {
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
                Intelligence = 10,
                Dexterity = 10,
                Constitution = 10
            };

            _characterService.SavePlayer(player);
            _gameStateService.CurrentPlayer = player;

            _logger.LogInformation("Character created and set as CurrentPlayer: {PlayerName}", player.Name);

            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Character successfully created.", "OK");
            }

            await Shell.Current.GoToAsync($"{nameof(LevelSelectView)}");
        }

        [RelayCommand]
        private void Close()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}