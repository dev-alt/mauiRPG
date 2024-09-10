using System;

namespace mauiRPG.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string IconSource { get; set; }
        public int Reward { get; set; }
        public int RequiredLevel { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        private QuestStatus Status { get; set; } = QuestStatus.Available;

        public bool IsAvailable => Status == QuestStatus.Available;
        public bool IsCompleted => Status == QuestStatus.Completed;

        public void Accept()
        {
            if (Status != QuestStatus.Available)
                throw new InvalidOperationException("This quest is not available.");

            Status = QuestStatus.InProgress;
            AcceptedDate = DateTime.Now;
        }

        public void Complete()
        {
            if (Status != QuestStatus.InProgress)
                throw new InvalidOperationException("This quest is not in progress.");

            Status = QuestStatus.Completed;
            CompletedDate = DateTime.Now;
        }
    }

    public enum QuestStatus
    {
        Available,
        InProgress,
        Completed
    }
}