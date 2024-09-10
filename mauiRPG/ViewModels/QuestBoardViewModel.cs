using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using mauiRPG.Models;
using mauiRPG.Services;
using mauiRPG.Views;

namespace mauiRPG.ViewModels
{
    public partial class QuestBoardViewModel : ObservableObject
    {
        private readonly GameStateService _gameStateService;
        private readonly IQuestService _questService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private Player? _currentPlayer;

        [ObservableProperty]
        private Quest? _selectedQuest;

        public ObservableCollection<Quest> AvailableQuests { get; } = [];

        public QuestBoardViewModel(GameStateService gameStateService, IQuestService questService, IDialogService dialogService)
        {
            _gameStateService = gameStateService;
            _questService = questService;
            _dialogService = dialogService;
            CurrentPlayer = _gameStateService.CurrentPlayer;
            LoadAvailableQuests();
        }

        private void LoadAvailableQuests()
        {
            try
            {
                var quests = _questService.GetAvailableQuests(CurrentPlayer);
                AvailableQuests.Clear();
                foreach (var quest in quests.Where(q => q.IsAvailable))
                {
                    AvailableQuests.Add(quest);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError("Error loading quests", ex.Message);
            }
        }

        [RelayCommand]
        private async Task ViewPlayerInfo()
        {
            if (CurrentPlayer == null)
            {
                await _dialogService.ShowAlert("Error", "No player information available.");
                return;
            }
            await _dialogService.ShowPlayerInfo(CurrentPlayer);
        }

        [RelayCommand]
        private async Task AcceptQuest()
        {
            if (CurrentPlayer == null)
            {
                await _dialogService.ShowAlert("Error", "No current player.");
                return;
            }
            if (SelectedQuest == null)
            {
                await _dialogService.ShowAlert("Error", "No quest selected.");
                return;
            }
            try
            {
                if (CurrentPlayer.Level < SelectedQuest.RequiredLevel)
                {
                    await _dialogService.ShowAlert("Cannot Accept Quest", $"You need to be level {SelectedQuest.RequiredLevel} to accept this quest.");
                    return;
                }
                await _questService.AcceptQuest(CurrentPlayer, SelectedQuest);
                await _dialogService.ShowAlert("Quest Accepted", $"You have accepted the quest: {SelectedQuest.Name}");
                LoadAvailableQuests();
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError("Error accepting quest", ex.Message);
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
    }

    public interface IQuestService
    {
        Task AcceptQuest(Player currentPlayer, Quest quest);
        IEnumerable<Quest> GetAvailableQuests(Player? currentPlayer);
    }

    public interface IDialogService
    {
        Task ShowAlert(string title, string message);
        Task ShowError(string title, string message);
        Task ShowPlayerInfo(Player player);
    }
}