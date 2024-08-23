namespace mauiRPG.Models
{
    public class Quest(int id, string name, string description, int experienceReward)
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public bool IsCompleted { get; set; } = false;
        public int ExperienceReward { get; set; } = experienceReward;
    }
}