namespace mauiRPG.Models
{
    public class Level(string name, string imageSource)
    {
        public int Number { get; set; }
        public required string Name { get; set; } = name;
        public bool IsUnlocked { get; set; }
        public required string ImageSource { get; set; } = imageSource;
    }
}
