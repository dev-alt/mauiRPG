using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
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

        [ObservableProperty]
        private Player? _currentPlayer;

        public ObservableCollection<Quest> AvailableQuests { get; } = new();

        public QuestBoardViewModel(GameStateService gameStateService, IQuestService questService)
        {
            _gameStateService = gameStateService;
            _questService = questService;

            CurrentPlayer = _gameStateService.CurrentPlayer;
            LoadAvailableQuests();
        }

        private void LoadAvailableQuests()
        {
            var quests = _questService.GetAvailableQuests(CurrentPlayer);
            AvailableQuests.Clear();
            foreach (var quest in quests)
            {
                AvailableQuests.Add(quest);
            }
        }

        [RelayCommand]
        private async Task ViewPlayerInfo()
        {

        }

        [RelayCommand]
        private async Task AcceptQuest(Quest quest)
        {
            if (CurrentPlayer == null) return;

            // Here you would typically update the player's active quests and possibly navigate to a quest detail page
            await _questService.AcceptQuest(CurrentPlayer, quest);

            // For now, we'll just show a message
            await Application.Current.MainPage.DisplayAlert("Quest Accepted", $"You have accepted the quest: {quest.Name}", "OK");

            // Refresh the available quests
            LoadAvailableQuests();
        }
    }

    public interface IQuestService
    {
        Task AcceptQuest(Player currentPlayer, Quest quest);
        IEnumerable<Quest> GetAvailableQuests(Player? currentPlayer);
    }
}