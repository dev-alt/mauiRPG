using mauiRPG.Services;
using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class CharacterSelect : ContentPage
{
    public CharacterSelect(CharacterService characterService, GameStateService gameStateService)
    {
		InitializeComponent();
        BindingContext = new CharacterCreationViewModel(characterService, gameStateService);
    }

}