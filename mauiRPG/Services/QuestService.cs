using Microsoft.Extensions.Logging;
using mauiRPG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mauiRPG.Services
{
    public interface IQuestService
    {
        Task<IEnumerable<Quest>> GetAvailableQuests(Player? currentPlayer);
        Task<bool> AcceptQuest(Player currentPlayer, Quest quest);
        Task<bool> CompleteQuest(Player currentPlayer, Quest quest);
        Task<IEnumerable<Quest>> GetActiveQuests(Player currentPlayer);
    }

    public class QuestService : IQuestService
    {
        private readonly IQuestRepository _questRepository;
        private readonly ILogger<QuestService> _logger;

        public QuestService(IQuestRepository questRepository, ILogger<QuestService> logger)
        {
            ArgumentNullException.ThrowIfNull(questRepository, nameof(questRepository));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _questRepository = questRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Quest>> GetAvailableQuests(Player? currentPlayer)
        {
            try
            {
                _logger.LogInformation("Getting available quests for player: {PlayerName}", currentPlayer?.Name ?? "Unknown");
                await Task.Delay(100).ConfigureAwait(false);
                var allQuests = _questRepository.GetAllQuests();
                var availableQuests = allQuests.Where(q => q.IsAvailable && (currentPlayer?.Level ?? 0) >= q.RequiredLevel).ToList();
                _logger.LogInformation("Found {AvailableQuestCount} available quests", availableQuests.Count);
                return availableQuests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available quests");
                return [];
            }
        }

        public async Task<bool> AcceptQuest(Player currentPlayer, Quest quest)
        {
            ArgumentNullException.ThrowIfNull(currentPlayer, nameof(currentPlayer));
            ArgumentNullException.ThrowIfNull(quest, nameof(quest));

            try
            {
                _logger.LogInformation("Player {PlayerName} attempting to accept quest: {QuestName}", currentPlayer.Name, quest.Name);
                await Task.Delay(100).ConfigureAwait(false);

                if (currentPlayer.Level < quest.RequiredLevel)
                {
                    _logger.LogWarning("Player level too low to accept quest. Player Level: {PlayerLevel}, Required Level: {RequiredLevel}", currentPlayer.Level, quest.RequiredLevel);
                    return false;
                }

                quest.Accept();
                currentPlayer.ActiveQuests.Add(quest);
                _logger.LogInformation("Quest {QuestName} accepted by player {PlayerName}", quest.Name, currentPlayer.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting quest {QuestName} for player {PlayerName}", quest.Name, currentPlayer.Name);
                return false;
            }
        }

        public async Task<bool> CompleteQuest(Player currentPlayer, Quest quest)
        {
            ArgumentNullException.ThrowIfNull(currentPlayer, nameof(currentPlayer));
            ArgumentNullException.ThrowIfNull(quest, nameof(quest));

            try
            {
                _logger.LogInformation("Player {PlayerName} attempting to complete quest: {QuestName}", currentPlayer.Name, quest.Name);
                await Task.Delay(100).ConfigureAwait(false);

                if (!currentPlayer.ActiveQuests.Contains(quest))
                {
                    _logger.LogWarning("Quest {QuestName} is not active for player {PlayerName}", quest.Name, currentPlayer.Name);
                    return false;
                }

                quest.Complete();
                currentPlayer.ActiveQuests.Remove(quest);
                currentPlayer.CompletedQuests.Add(quest);
                currentPlayer.Gold += quest.Reward;
                _logger.LogInformation("Quest {QuestName} completed by player {PlayerName}. Reward: {Reward} gold", quest.Name, currentPlayer.Name, quest.Reward);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing quest {QuestName} for player {PlayerName}", quest.Name, currentPlayer.Name);
                return false;
            }
        }

        public async Task<IEnumerable<Quest>> GetActiveQuests(Player currentPlayer)
        {
            ArgumentNullException.ThrowIfNull(currentPlayer, nameof(currentPlayer));

            try
            {
                _logger.LogInformation("Getting active quests for player: {PlayerName}", currentPlayer.Name);
                await Task.Delay(100).ConfigureAwait(false);
                _logger.LogInformation("Player {PlayerName} has {ActiveQuestCount} active quests", currentPlayer.Name, currentPlayer.ActiveQuests.Count);
                return [.. currentPlayer.ActiveQuests];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active quests for player {PlayerName}", currentPlayer.Name);
                return [];
            }
        }
    }
}