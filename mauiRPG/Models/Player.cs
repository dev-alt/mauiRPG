using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public class Player : Character
    {
        public required Race Race { get; set; }
        public required Class Class { get; set; }

    }
}
