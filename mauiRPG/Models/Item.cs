using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public class Item
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ItemType Type { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }

        public virtual void Use(Player player) { }
    }

    public enum ItemType
    {
        Consumable,
        Equipment,
        QuestItem
    }
}