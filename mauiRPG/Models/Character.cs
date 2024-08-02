using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace mauiRPG.Models
{
    internal class Character
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string RaceName { get; set; }
        public required string ClassName { get; set; }
    }
}
