using mauiRPG.Models;
using mauiRPG.ViewModels;
using System.Text.Json;

namespace mauiRPG.Views
{
    [QueryProperty(nameof(PlayerJson), "PlayerJson")]
    public partial class PlayerInfoView : ContentPage
    {
        private string playerJson = null!;
        public string PlayerJson
        {
            get => playerJson;
            set
            {
                playerJson = Uri.UnescapeDataString(value);
                LoadPlayer();
            }
        }

        public PlayerInfoView()
        {
            InitializeComponent();
        }

        private void LoadPlayer()
        {
            if (!string.IsNullOrEmpty(playerJson))
            {
                try
                {
                    var player = JsonSerializer.Deserialize<Player>(playerJson);
                    if (player != null) BindingContext = new PlayerInfoViewModel(player);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error deserializing player: {ex.Message}");
                    // Handle the error, perhaps show an alert to the user
                }
            }
        }
    }
}