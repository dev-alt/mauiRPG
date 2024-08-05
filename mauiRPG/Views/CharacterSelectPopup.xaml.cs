using mauiRPG.Services;
using mauiRPG.ViewModels;
using Microsoft.Extensions.Logging;


namespace mauiRPG.Views
{
    public partial class CharacterSelectPopup : ContentView
    {
        public CharacterSelectPopup()
        {
            InitializeComponent();
        }

        public CharacterSelectPopup(CharacterService characterService, GameStateService gameStateService, ILogger<CharacterCreationViewModel> logger)
        {
            InitializeComponent();
            BindingContext = new CharacterCreationViewModel(characterService, gameStateService, logger);
        }
    }
}