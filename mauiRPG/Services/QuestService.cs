using mauiRPG.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mauiRPG.Services
{
    public interface IQuestService
    {
        Task<IEnumerable<Quest>> GetAvailableQuests(Player? currentPlayer);
        Task AcceptQuest(Player currentPlayer, Quest quest);
        Task CompleteQuest(Player currentPlayer, Quest quest);
        Task<IEnumerable<Quest>> GetActiveQuests(Player currentPlayer);
    }

    public class QuestService(IQuestRepository questRepository) : IQuestService
    {
        public async Task<IEnumerable<Quest>> GetAvailableQuests(Player? currentPlayer)
        {
            await Task.Delay(100).ConfigureAwait(false); // Simulate I/O-bound delay
            return questRepository.GetAllQuests().Where(q => q.IsAvailable && (currentPlayer?.Level ?? 0) >= q.RequiredLevel);
        }

        public async Task AcceptQuest(Player currentPlayer, Quest quest)
        {
            await Task.Delay(100).ConfigureAwait(false); // Simulate I/O-bound delay
            if (currentPlayer.Level < quest.RequiredLevel)
            {
                throw new InvalidOperationException("Player level is too low to accept this quest.");
            }

            quest.Accept();
            currentPlayer.ActiveQuests.Add(quest);
        }

        public async Task CompleteQuest(Player currentPlayer, Quest quest)
        {
            await Task.Delay(100).ConfigureAwait(false); // Simulate I/O-bound delay
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
            await Task.Delay(100).ConfigureAwait(false); // Simulate I/O-bound delay
            return currentPlayer.ActiveQuests;
        }
    }
}