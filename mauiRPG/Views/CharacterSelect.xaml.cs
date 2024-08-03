using mauiRPG.Services;
using mauiRPG.ViewModels;
using Microsoft.Extensions.Logging;

namespace mauiRPG.Views;

public partial class CharacterSelect : ContentPage
{
    public CharacterSelect(CharacterService characterService, GameStateService gameStateService, ILogger<CharacterCreationViewModel> logger)
    {
		InitializeComponent();
        BindingContext = new CharacterCreationViewModel(characterService, gameStateService, logger);
    }

}