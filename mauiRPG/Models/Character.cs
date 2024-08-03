using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace mauiRPG.Models
{
    public class Character : ICharacter
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
    }
}
