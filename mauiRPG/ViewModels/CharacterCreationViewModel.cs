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
        private readonly InventoryService _inventoryService;
        private readonly ILogger<CharacterCreationViewModel> _logger;

        [ObservableProperty] private string _name = string.Empty;

        [ObservableProperty] private Race? _selectedRace;

        public ObservableCollection<Race> Races { get; } =
        [
            new Human(),
            new Elf(),
            new Dwarf(),
            new Orc()
        ];

        public event EventHandler<string>? ShowErrorRequested;
        public event EventHandler<string>? ShowSuccessRequested;

        public CharacterCreationViewModel(CharacterService characterService, GameStateService gameStateService, InventoryService inventoryService,
            ILogger<CharacterCreationViewModel> logger)
        {
            _characterService = characterService;
            _gameStateService = gameStateService;
            _inventoryService = inventoryService;
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
                // Check if the services are not null
                if (_characterService == null || _inventoryService == null || _gameStateService == null)
                {
                    throw new InvalidOperationException("One or more required services are not initialized.");
                }

                // Check for name and selected race
                if (string.IsNullOrWhiteSpace(Name) || SelectedRace == null)
                {
                    _logger.LogWarning("Attempted to create character with invalid input. Name: {Name}, Race: {Race}",
                        Name, SelectedRace?.Name);
                    ShowErrorRequested?.Invoke(this,
                        "Brave adventurer, thy quest cannot begin without a name and chosen lineage. Please provide both to forge thy legend.");
                    return;
                }

                _logger.LogInformation("Creating character with Name: {Name}, Race: {Race}", Name, SelectedRace.Name);

                var player = new Player
                {
                    Id = GenerateUniqueId(),
                    Name = Name,
                    Race = SelectedRace,
                    Level = 1,
                    CurrentHealth = 100,
                    MaxHealth = 100,
                    Strength = 15,
                    Intelligence = 15,
                    Dexterity = 15,
                    Constitution = 15
                };

                // Check if inventory service is working correctly
                if (_inventoryService == null)
                {
                    throw new InvalidOperationException("Inventory service is not initialized.");
                }

                var healthPotion = new HealthPotion
                {
                    Name = "Health Potion",
                    Description = "Restores 50 health points",
                    IconSource = "health_potion_icon.png",
                    Id = 1,
                    HealAmount = 50
                };

                _inventoryService.AddItem(player.Id, healthPotion);
                _logger.LogInformation("Health potion added to player's inventory");

                // Debug: Show current inventory
                var inventoryItems = _inventoryService.GetPlayerItems(player.Id);
                if (inventoryItems == null)
                {
                    _logger.LogWarning("Inventory items could not be retrieved.");
                }
                else
                {
                    _logger.LogInformation("Player inventory items: {InventoryItems}", string.Join(", ", inventoryItems.Select(item => item.Name)));
                }

                _characterService.SaveCharacter(player);
                _logger.LogInformation("Player saved successfully");

                _gameStateService.SetCurrentPlayer(player);
                _logger.LogInformation("Current player set in GameStateService");

                ShowSuccessRequested?.Invoke(this,
                    "Huzzah! Thy character has been forged in the annals of legend. May thy quest be glorious!");

                // Check if Shell.Current is null before using it
                if (Shell.Current == null)
                {
                    _logger.LogError("Shell.Current is null. Cannot navigate.");
                    ShowErrorRequested?.Invoke(this, "Navigation error: Shell.Current is not initialized.");
                    return;
                }

                await Shell.Current.GoToAsync("///LevelSelect");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating character");
                ShowErrorRequested?.Invoke(this, $"An error occurred while creating your character: {ex.Message}");
            }
        }


        private static int GenerateUniqueId()
        {
            int uniqueId = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            uniqueId += new Random().Next(1000, 9999);
            return uniqueId;
        }
    }
}
