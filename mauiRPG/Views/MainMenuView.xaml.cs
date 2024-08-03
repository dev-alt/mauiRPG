using mauiRPG.Views;

namespace mauiRPG.Views
{
    public partial class MainMenuView : ContentPage
    {
        public MainMenuView()
        {
            InitializeComponent();
        }

        private async void OnCreateNewCharacterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CharacterSelect));
        }

        private async void OnLoadCharacterClicked(object sender, EventArgs e)
        {
            // TODO: Implement character loading functionality
            await DisplayAlert("Load Character", "Character loading not implemented yet.", "OK");
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            // TODO: Implement settings page navigation
            await DisplayAlert("Settings", "Settings page not implemented yet.", "OK");
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}