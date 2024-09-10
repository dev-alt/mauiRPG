using mauiRPG.Models;

namespace mauiRPG.Services
{
    public interface IQuestService
    {
        Task<IEnumerable<Quest>> GetAvailableQuests(Player? currentPlayer);
        Task AcceptQuest(Player currentPlayer, Quest quest);
        Task CompleteQuest(Player currentPlayer, Quest quest);
        Task<IEnumerable<Quest>> GetActiveQuests(Player currentPlayer);
    }

    public class QuestService : IQuestService
    {
        private readonly List<Quest> _quests;

        public QuestService()
        {
            _quests =
            [
                new Quest
                {
                    Id = 1,
                    Name = "Slay the Dragon",
                    Description = "Defeat the fearsome dragon terrorizing the nearby village.",
                    IconSource = "dragon_quest.png",
                    Reward = 1000,
                    RequiredLevel = 5
                },

                new Quest
                {
                    Id = 2,
                    Name = "Retrieve the Artifact",
                    Description = "Locate and retrieve the ancient artifact from the haunted ruins.",
                    IconSource = "artifact_quest.png",
                    Reward = 750,
                    RequiredLevel = 3
                },

                new Quest
                {
                    Id = 3,
                    Name = "Escort the Merchant",
                    Description = "Safely escort the merchant to the neighboring town.",
                    IconSource = "escort_quest.png",
                    Reward = 500,
                    RequiredLevel = 1
                }
            ];
        }

        public async Task<IEnumerable<Quest>> GetAvailableQuests(Player? currentPlayer)
        {
            await Task.Delay(100); // Simulating network delay
            return _quests.Where(q => q.IsAvailable && (currentPlayer?.Level ?? 0) >= q.RequiredLevel);
        }

        public async Task AcceptQuest(Player currentPlayer, Quest quest)
        {
            await Task.Delay(100); // Simulating network delay
            if (currentPlayer.Level < quest.RequiredLevel)
            {
                throw new InvalidOperationException("Player level is too low to accept this quest.");
            }

            quest.Accept();
            currentPlayer.ActiveQuests.Add(quest);
        }

        public async Task CompleteQuest(Player currentPlayer, Quest quest)
        {
            await Task.Delay(100); // Simulating network delay
            if (!currentPlayer.ActiveQuests.Contains(quest))
            {
                throw new InvalidOperationException("This quest is not active for the current player.");
            }

            quest.Complete();
            currentPlayer.ActiveQuests.Remove(quest);
            currentPlayer.CompletedQuests.Add(quest);
            currentPlayer.Gold += quest.Reward;
        }

        public async Task<IEnumerable<Quest>> GetActiveQuests(Player currentPlayer)
        {
            await Task.Delay(100); // Simulating network delay
            return currentPlayer.ActiveQuests;
        }
    }
}