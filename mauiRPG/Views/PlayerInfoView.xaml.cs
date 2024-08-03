using mauiRPG.Models;
using mauiRPG.ViewModels;
using System.Text.Json;

namespace mauiRPG.Views
{
    [QueryProperty(nameof(PlayerJson), "CurrentPlayer")]
    public partial class PlayerInfoView : ContentPage
    {
        private string _playerJson;
        public string PlayerJson
        {
            get => _playerJson;
            set
            {
                _playerJson = value;
                OnPropertyChanged();
                LoadPlayer();
            }
        }

        public PlayerInfoView()
        {
            InitializeComponent();
            BindingContext = new PlayerInfoViewModel();
        }

        private void LoadPlayer()
        {
            if (!string.IsNullOrEmpty(PlayerJson))
            {
                try
                {
                    var player = JsonSerializer.Deserialize<Player>(PlayerJson);
                    (BindingContext as PlayerInfoViewModel).Player = player;
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