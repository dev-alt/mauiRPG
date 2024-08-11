using mauiRPG.Models;

namespace mauiRPG.Services
{
    public class CombatService
    {    
        private readonly Random _random = new Random();

        public int CalculateDamage(int attackStat, int defenseStat)
        {
            int baseDamage = _random.Next(attackStat / 2, attackStat);
            return Math.Max(0, baseDamage - defenseStat);
        }
    }
}