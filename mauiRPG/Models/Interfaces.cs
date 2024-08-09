using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Models
{
    public interface ICharacter
    {
        string Name { get; set; }
        int Health { get; set; }
        int Level { get; set; }
    }

    public interface IRace
    {
    }

    public interface IClass
    {
    }
}
