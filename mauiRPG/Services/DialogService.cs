using CommunityToolkit.Maui.Views;
using mauiRPG.Models;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;


namespace mauiRPG.Services
{
    public interface IDialogService
    {
        Task ShowAlert(string title, string message);
        Task ShowError(string title, string message);
        Task ShowPlayerInfo(Player player);
        Task<bool> ShowConfirmation(string title, string message);
    }

    public class DialogService(ILogger<PlayerInfoPopup> logger) : IDialogService
    {

        private readonly ILogger<PlayerInfoPopup> _logger = logger;

        public async Task ShowAlert(string title, string message)
        {
            await Application.Current!.MainPage!.DisplayAlert(title, message, "OK");
        }

        public async Task ShowError(string title, string message)
        {
            await Application.Current!.MainPage!.DisplayAlert(title, message, "OK");
        }

        public async Task ShowPlayerInfo(Player player)
        {
            var popup = new PlayerInfoPopup(player, _logger);
            await Application.Current!.MainPage!.ShowPopupAsync(popup);
        }

        public async Task<bool> ShowConfirmation(string title, string message)
        {
            return await Application.Current!.MainPage!.DisplayAlert(title, message, "Yes", "No");
        }
    }
}