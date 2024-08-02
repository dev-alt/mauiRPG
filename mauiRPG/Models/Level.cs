using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public class Level
    {
        public int Number { get; set; }
        public required string Name { get; set; }
        public bool IsUnlocked { get; set; }
        public required string ImageSource { get; set; }
        public Level(string name, string imageSource)
        {
            Name = name;
            ImageSource = imageSource;
        }

        public Level()
        {
        }
    }
}
