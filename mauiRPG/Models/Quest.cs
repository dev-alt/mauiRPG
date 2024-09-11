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
        public DateTime? AcceptedDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        private QuestStatus _status = QuestStatus.Available;

        public bool IsAvailable => _status == QuestStatus.Available;
        public bool IsActive => _status == QuestStatus.InProgress;
        public bool IsCompleted => _status == QuestStatus.Completed;

        public void Accept()
        {
            if (_status != QuestStatus.Available)
                throw new InvalidOperationException("This quest is not available.");

            _status = QuestStatus.InProgress;
            AcceptedDate = DateTime.Now;
        }

        public void Complete()
        {
            if (_status != QuestStatus.InProgress)
                throw new InvalidOperationException("This quest is not in progress.");

            _status = QuestStatus.Completed;
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