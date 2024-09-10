using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;
using CommunityToolkit.Maui.Views;

namespace mauiRPG.ViewModels
{
    public partial class QuestBoardViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;
        private readonly IQuestService _questService;
        private readonly IDialogService _dialogService;
        private readonly Page _currentPage;

        [ObservableProperty]
        private Player? _currentPlayer;

        [ObservableProperty]
        private Quest? _selectedQuest;

        public ObservableCollection<Quest> AvailableQuests { get; } = [];

        public QuestBoardViewModel(GameStateService gameStateService, IQuestService questService, IDialogService dialogService, Page currentPage)
        {
            _gameStateService = gameStateService;
            _questService = questService;
            _dialogService = dialogService;
            _currentPage = currentPage;
            CurrentPlayer = _gameStateService.CurrentPlayer;
            LoadAvailableQuests();
        }

        private async void LoadAvailableQuests()
        {
            try
            {
                var quests = await _questService.GetAvailableQuests(CurrentPlayer);
                AvailableQuests.Clear();
                foreach (var quest in quests.Where(q => q.IsAvailable))
                {
                    AvailableQuests.Add(quest);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorPopup("Error loading quests", ex.Message);
            }
        }

        [RelayCommand]
        private async Task ViewPlayerInfo()
        {
            if (CurrentPlayer == null)
            {
                await ShowErrorPopup("Error", "No player information available.");
                return;
            }
            await _dialogService.ShowPlayerInfo(CurrentPlayer);
        }

        [RelayCommand]
        private async Task AcceptQuest()
        {
            if (CurrentPlayer == null)
            {
                await ShowErrorPopup("Error", "No current player.");
                return;
            }
            if (SelectedQuest == null)
            {
                await ShowErrorPopup("Error", "No quest selected.");
                return;
            }
            try
            {
                if (CurrentPlayer.Level < SelectedQuest.RequiredLevel)
                {
                    await ShowErrorPopup("Cannot Accept Quest", $"You need to be level {SelectedQuest.RequiredLevel} to accept this quest.");
                    return;
                }
                await _questService.AcceptQuest(CurrentPlayer, SelectedQuest);
                await _dialogService.ShowAlert("Quest Accepted", $"You have accepted the quest: {SelectedQuest.Name}");
                LoadAvailableQuests();
            }
            catch (Exception ex)
            {
                await ShowErrorPopup("Error accepting quest", ex.Message);
            }
        }

        [RelayCommand]
        private void SelectQuest(Quest quest)
        {
            SelectedQuest = quest;
        }

        [RelayCommand]
        private async Task RefreshQuests()
        {
            LoadAvailableQuests();
            await _dialogService.ShowAlert("Quests Refreshed", "The quest board has been updated.");
        }

        private async Task ShowErrorPopup(string title, string message)
        {
            var errorPopup = new ErrorPopup($"{title}\n\n{message}");
            await _currentPage.ShowPopupAsync(errorPopup);
        }
    }
}