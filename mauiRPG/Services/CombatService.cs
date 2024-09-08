using mauiRPG.Models;

namespace mauiRPG.Services;

public interface ICombatService
{
    CombatResult ExecutePlayerAttack(Player player, CombatantModel enemy);
    CombatResult ExecuteEnemyAttack(CombatantModel enemy, Player player);
    bool AttemptEscape();
}
public class CombatService : ICombatService
{
    private readonly Random _random = new();

    public CombatResult ExecutePlayerAttack(Player player, CombatantModel enemy)
    {
        int damage = CalculateDamage(player.Attack, enemy.Defense);
        enemy.CurrentHealth -= damage;
        return new CombatResult
        {
            Attacker = player.Name,
            Defender = enemy.Name,
            Damage = damage,
            RemainingHealth = enemy.CurrentHealth
        };
    }

    public CombatResult ExecuteEnemyAttack(CombatantModel enemy, Player player)
    {
        int damage = CalculateDamage(enemy.Attack, player.Defense);
        if (player.IsDefending) damage /= 2;  // Reduce damage if player is defending
        player.CurrentHealth -= damage;
        return new CombatResult
        {
            Attacker = enemy.Name,
            Defender = player.Name,
            Damage = damage,
            RemainingHealth = player.CurrentHealth
        };
    }

    public bool AttemptEscape()
    {
        return _random.Next(100) < 50;  // 50% chance of escape
    }

    private int CalculateDamage(int attack, int defense)
    {
        return Math.Max(0, attack - defense + _random.Next(-2, 3));
    }
}