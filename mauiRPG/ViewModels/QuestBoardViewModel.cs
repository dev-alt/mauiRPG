using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace mauiRPG.ViewModels
{
    public partial class QuestBoardViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;
        private readonly IQuestService _questService;
        private readonly IDialogService _dialogService;
        private readonly ILogger<QuestBoardViewModel> _logger;

        [ObservableProperty]
        private Player? _currentPlayer;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsQuestSelected))]
        private Quest? _selectedQuest;

        public bool IsQuestSelected => SelectedQuest != null;

        [ObservableProperty]
        private bool _isLoading;

        public ObservableCollection<Quest> AvailableQuests { get; } = [];

        public QuestBoardViewModel(
            GameStateService gameStateService,
            IQuestService questService,
            IDialogService dialogService,
            ILogger<QuestBoardViewModel> logger)
        {
            _gameStateService = gameStateService ?? throw new ArgumentNullException(nameof(gameStateService));
            _questService = questService ?? throw new ArgumentNullException(nameof(questService));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            CurrentPlayer = _gameStateService.CurrentPlayer;
            _logger.LogInformation("QuestBoardViewModel initialized for player: {PlayerName}", CurrentPlayer?.Name ?? "Unknown");

            LoadAvailableQuestsAsync().ConfigureAwait(false);
        }

        private async Task LoadAvailableQuestsAsync()
        {
            try
            {
                IsLoading = true;
                _logger.LogInformation("Loading available quests");

                var quests = await _questService.GetAvailableQuests(CurrentPlayer);
                AvailableQuests.Clear();
                foreach (var quest in quests.Where(q => q.IsAvailable))
                {
                    AvailableQuests.Add(quest);
                }

                _logger.LogInformation("Loaded {QuestCount} available quests", AvailableQuests.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading available quests");
                await ShowErrorPopupAsync("Error loading quests", "Failed to load available quests. Please try again.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ViewPlayerInfo()
        {
            try
            {
                if (CurrentPlayer == null)
                {
                    _logger.LogWarning("Attempted to view player info with no current player");
                    await ShowErrorPopupAsync("Error", "No player information available.");
                    return;
                }

                _logger.LogInformation("Viewing player info for: {PlayerName}", CurrentPlayer.Name);
                await _dialogService.ShowPlayerInfo(CurrentPlayer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing player info");
                await ShowErrorPopupAsync("Error", "Failed to display player information. Please try again.");
            }
        }


        [RelayCommand]
        private void SelectQuest(Quest quest)
        {
            SelectedQuest = quest;
            _logger.LogInformation("Quest selected: {QuestName}", quest.Name);
        }

        [RelayCommand]
        private async Task AcceptQuest(Quest quest)
        {
            if (CurrentPlayer == null)
            {
                _logger.LogWarning("Attempted to accept quest with no current player");
                await ShowErrorPopupAsync("Error", "No current player.");
                return;
            }
            try
            {
                _logger.LogInformation("Player {PlayerName} attempting to accept quest: {QuestName}", CurrentPlayer.Name, quest.Name);
                if (CurrentPlayer.Level < quest.RequiredLevel)
                {
                    _logger.LogWarning("Player level too low to accept quest. Player Level: {PlayerLevel}, Required Level: {RequiredLevel}", CurrentPlayer.Level, quest.RequiredLevel);
                    await ShowErrorPopupAsync("Cannot Accept Quest", $"You need to be level {quest.RequiredLevel} to accept this quest.");
                    return;
                }
                bool accepted = await _questService.AcceptQuest(CurrentPlayer, quest);
                if (accepted)
                {
                    _logger.LogInformation("Quest {QuestName} accepted by player {PlayerName}", quest.Name, CurrentPlayer.Name);
                    await ShowErrorPopupAsync("Quest Accepted", $"You have accepted the quest: {quest.Name}");
                    await LoadAvailableQuestsAsync();
                }
                else
                {
                    _logger.LogWarning("Failed to accept quest {QuestName} for player {PlayerName}", quest.Name, CurrentPlayer.Name);
                    await ShowErrorPopupAsync("Error", "Failed to accept the quest. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting quest {QuestName} for player {PlayerName}", quest.Name, CurrentPlayer.Name);
                await ShowErrorPopupAsync("Error accepting quest", "An unexpected error occurred. Please try again.");
            }
        }

        [RelayCommand]
        private async Task RefreshQuests()
        {
            _logger.LogInformation("Refreshing quest board");
            await LoadAvailableQuestsAsync();
            await ShowErrorPopupAsync("Quests Refreshed", "The quest board has been updated.");
        }

        private async Task ShowErrorPopupAsync(string title, string message)
        {
            _logger.LogWarning("Showing error popup. Title: {Title}, Message: {Message}", title, message);
            var errorPopup = new ErrorPopup($"{title}\n\n{message}");
            if (Application.Current?.MainPage is Page currentPage)
            {
                await currentPage.ShowPopupAsync(errorPopup);
            }
            else
            {
                _logger.LogWarning("Unable to show error popup: Current page is not available");
            }
        }
        public void OnAppearing()
        {
            _logger.LogInformation("QuestBoardView is appearing");
            LoadAvailableQuestsAsync().ConfigureAwait(false);
        }
        [RelayCommand]
        private static async Task GoBack()
        {
            await Shell.Current.GoToAsync("///LevelSelect");
        }

        public void OnDisappearing()
        {
            _logger.LogInformation("QuestBoardView is disappearing");
        }
    }
}