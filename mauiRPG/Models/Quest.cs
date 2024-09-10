namespace mauiRPG.Models
{
    public class Quest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconSource { get; set; }
        public int Reward { get; set; }
        // Add other properties as needed (e.g., difficulty, required level, etc.)
    }
}