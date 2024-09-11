using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

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

        public event EventHandler<string>? ShowErrorRequested;
        public event EventHandler<string>? ShowSuccessRequested;

        public CharacterCreationViewModel(CharacterService characterService, GameStateService gameStateService, ILogger<CharacterCreationViewModel> logger)
        {
            _characterService = characterService;
            _gameStateService = gameStateService;
            _logger = logger;
            _logger.LogInformation("CharacterCreationViewModel initialized");
        }

        [RelayCommand]
        private void SelectRace(Race race)
        {
            try
            {
                SelectedRace = race;
                _logger.LogInformation("Selected race: {RaceName}", race.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selecting race");
            }
        }

        [RelayCommand]
        private async Task CreateCharacter()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) || SelectedRace == null)
                {
                    _logger.LogWarning("Attempted to create character with invalid input. Name: {Name}, Race: {Race}", Name, SelectedRace?.Name);
                    ShowErrorRequested?.Invoke(this, "Brave adventurer, thy quest cannot begin without a name and chosen lineage. Please provide both to forge thy legend.");
                    return;
                }

                _logger.LogInformation("Creating character with Name: {Name}, Race: {Race}", Name, SelectedRace.Name);

                var player = new Player
                {
                    Name = Name,
                    Race = SelectedRace,
                    Level = 1,
                    CurrentHealth = 100,
                    MaxHealth = 100,
                    Strength = 10,
                    Intelligence = 10,
                    Dexterity = 10,
                    Constitution = 10
                };

                _logger.LogInformation("Player object created successfully");

                _characterService.SaveCharacter(player);
                _logger.LogInformation("Player saved successfully");

                _gameStateService.SetCurrentPlayer(player);
                _logger.LogInformation("Current player set in GameStateService");

                ShowSuccessRequested?.Invoke(this, "Huzzah! Thy character has been forged in the annals of legend. May thy quest be glorious!");

                await Shell.Current.GoToAsync("LevelSelect");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating character");
                ShowErrorRequested?.Invoke(this, $"An error occurred while creating your character: {ex.Message}");
            }
        }
    }
}