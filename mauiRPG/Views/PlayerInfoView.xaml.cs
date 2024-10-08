using CommunityToolkit.Maui.Views;
using mauiRPG.Models;
using mauiRPG.ViewModels;
using Microsoft.Extensions.Logging;

namespace mauiRPG.Views
{
    public partial class PlayerInfoPopup : Popup
    {
        private readonly ILogger<PlayerInfoPopup> _logger;

        public PlayerInfoPopup(Player player, ILogger<PlayerInfoPopup> logger)
        {
            InitializeComponent();
            _logger = logger;

            if (player != null)
            {
                _logger.LogInformation("Player info: Name={Name}, Race={Race}",
                    player.Name, player.Race?.Name);
                BindingContext = new PlayerInfoViewModel(player);
            }
            else
            {
                _logger.LogWarning("Player object is null");
            }
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}