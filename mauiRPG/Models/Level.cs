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
        public string Name { get; set; }
        public bool IsUnlocked { get; set; }
        public string ImageSource { get; set; }
    }
}
