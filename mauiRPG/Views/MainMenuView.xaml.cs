using mauiRPG.Services;
using mauiRPG.Views;

namespace mauiRPG.Views
{
    public partial class MainMenuView : ContentPage
    {
        private readonly CharacterService _characterService;

        public MainMenuView()
        {
            InitializeComponent();
            _characterService = new CharacterService();
        }

        private async void OnCreateNewCharacterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CharacterSelect));
        }

        private async void OnLoadCharacterClicked(object sender, EventArgs e)
        {
            var characters = _characterService.LoadCharacters();
            if (characters.Count == 0)
            {
                await DisplayAlert("No Characters", "No saved characters found. Please create a new character.", "OK");
                return;
            }

            var characterNames = characters.Select(c => c.Name).ToArray();
            var selectedCharacter = await DisplayActionSheet("Select a character", "Cancel", null, characterNames);

            if (selectedCharacter == "Cancel") return;
            {
                try
                {
                    var character = characters.First(c => c.Name == selectedCharacter);
                    await DisplayAlert("Character Loaded", $"Loaded character: {character.Name}", "OK");
                }
                catch(Exception ex)
                {
                    throw new Exception($"Error loading character: {ex.Message}");
                }
            }
        }
        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            // TODO: Implement settings page navigation
            await DisplayAlert("Settings", "Settings page not implemented yet.", "OK");
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            Application.Current?.Quit();
        }
    }
}