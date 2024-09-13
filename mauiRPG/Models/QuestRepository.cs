namespace mauiRPG.Models;

public interface IQuestRepository
{
    IReadOnlyList<Quest> GetAllQuests();
}

public class QuestRepository : IQuestRepository
{
    private readonly List<Quest> _quests;

    public QuestRepository()
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

    public IReadOnlyList<Quest> GetAllQuests()
    {
        return _quests;
    }
}
