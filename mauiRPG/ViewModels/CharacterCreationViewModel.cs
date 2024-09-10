using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace mauiRPG.ViewModels
{
    public partial class CharacterCreationViewModel : ObservableObject
    {
        private readonly CharacterService _characterService;
        private readonly GameStateService _gameStateService;
        private readonly ILogger<CharacterCreationViewModel> _logger;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private Race? _selectedRace;

        public ObservableCollection<Race> Races { get; } =
        [
            new Human(),
            new Elf(),
            new Dwarf(),
            new Orc()
        ];

        public CharacterCreationViewModel(CharacterService characterService, GameStateService gameStateService, ILogger<CharacterCreationViewModel> logger)
        {
            _characterService = characterService;
            _gameStateService = gameStateService;
            _logger = logger;

            _logger.LogInformation("CharacterCreationViewModel initialized");
        }

        [RelayCommand]
        private void SelectRace(string raceName)
        {
            SelectedRace = Races.FirstOrDefault(r => r.Name == raceName);
            _logger.LogInformation("Selected race: {RaceName}", SelectedRace?.Name);
        }

        [RelayCommand]
        private async Task CreateCharacter()
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedRace == null)
            {
                _logger.LogWarning("Attempted to create character with invalid input. Name: {Name}, Race: {Race}", Name, SelectedRace?.Name);
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter a name and select a race.", "OK");
                return;
            }

            var player = new Player
            {
                Name = Name,
                Race = SelectedRace,
                Level = 1,
                Health = 100,
                MaxHealth = 100,
                Strength = 10,
                Intelligence = 10,
                Dexterity = 10,
                Constitution = 10
            };

            _characterService.SavePlayer(player);
            _gameStateService.CurrentPlayer = player;

            _logger.LogInformation("Created character: {PlayerName}, Race: {RaceName}", player.Name, player.Race.Name);

            await Application.Current.MainPage.DisplayAlert("Success", "Character created successfully!", "OK");
            await Shell.Current.GoToAsync($"{nameof(LevelSelectView)}");
        }
    }
}