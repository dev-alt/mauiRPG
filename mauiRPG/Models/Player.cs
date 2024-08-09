using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public class Player : Character
    {
        [JsonConverter(typeof(RaceConverter))]
        public required Race Race { get; set; }
        [JsonConverter(typeof(ClassConverter))]
        public required Class Class { get; set; }

    }
}
