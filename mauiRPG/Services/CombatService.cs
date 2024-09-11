using mauiRPG.Models;

namespace mauiRPG.Services
{
    public interface ICombatService
    {
        CombatResult ExecutePlayerAttack(Player player, CombatantModel enemy);
        CombatResult ExecuteEnemyAttack(CombatantModel enemy, Player player);
        CombatResult ExecuteAttack(Character attacker, Character defender, bool isDefending);
        CombatResult ExecuteSpecialAttack(Character attacker, Character defender);
        CombatResult Defend(Character character);
        bool AttemptEscape();
    }

    public class CombatService : ICombatService
    {
        private readonly Random _random = new();

        public CombatResult ExecutePlayerAttack(Player player, CombatantModel enemy)
        {
            return ExecuteAttack(player, enemy, enemy.IsDefending);
        }

        public CombatResult ExecuteEnemyAttack(CombatantModel enemy, Player player)
        {
            return ExecuteAttack(enemy, player, player.IsDefending);
        }

        public CombatResult ExecuteAttack(Character attacker, Character defender, bool isDefending = false)
        {
            int damage = CalculateDamage(attacker.Attack, defender.Defense, isDefending);
            defender.TakeDamage(damage);
            return new CombatResult
            {
                Attacker = attacker.Name,
                Defender = defender.Name,
                Damage = damage,
                RemainingHealth = defender.CurrentHealth,
                Message = GenerateAttackMessage(attacker.Name, defender.Name, damage)
            };
        }

        public CombatResult ExecuteSpecialAttack(Character attacker, Character defender)
        {
            int damage = CalculateDamage(attacker.Attack * 1.5, defender.Defense, false);
            defender.TakeDamage(damage);
            return new CombatResult
            {
                Attacker = attacker.Name,
                Defender = defender.Name,
                Damage = damage,
                RemainingHealth = defender.CurrentHealth,
                Message = $"{attacker.Name} uses a powerful attack against {defender.Name} for {damage} damage!"
            };
        }

        public CombatResult Defend(Character character)
        {
            return new CombatResult
            {
                Attacker = character.Name,
                Defender = character.Name,
                Damage = 0,
                RemainingHealth = character.CurrentHealth,
                Message = $"{character.Name} takes a defensive stance."
            };
        }

        public bool AttemptEscape()
        {
            return _random.Next(100) < 50;  // 50% chance of escape
        }

        private int CalculateDamage(double attack, int defense, bool isDefending)
        {
            double damageReduction = defense / (defense + 100.0);
            int baseDamage = (int)Math.Max(2, attack * (1 - damageReduction));
            int variability = _random.Next(-2, 3); // -2 to +2 damage variability
            int finalDamage = Math.Max(0, baseDamage + variability);
            return isDefending ? finalDamage / 2 : finalDamage;
        }

        private string GenerateAttackMessage(string attacker, string defender, int damage)
        {
            string[] attackVerbs = ["strikes", "hits", "slashes at", "bashes"];
            string verb = attackVerbs[_random.Next(attackVerbs.Length)];
            return $"{attacker} {verb} {defender} for {damage} damage!";
        }
    }
}