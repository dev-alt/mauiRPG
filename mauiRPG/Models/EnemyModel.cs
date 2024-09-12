using CommunityToolkit.Mvvm.ComponentModel;

namespace mauiRPG.Models
{
    public partial class EnemyModel : Character
    {
        [ObservableProperty] private bool _isDefending;

        public EnemyModel()
        {
            Defense = 5;
        }

        public void Defend()
        {
            IsDefending = true;
            Defense += 5;
        }

        public void StopDefending()
        {
            IsDefending = false;
            Defense -= 5;
        }
    }

    public class CombatLogEntryModel
    {
        public required string Message { get; set; }
        public bool IsPlayerAction { get; set; }
    }
}