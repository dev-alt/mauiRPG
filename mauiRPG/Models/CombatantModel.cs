using CommunityToolkit.Mvvm.ComponentModel;

namespace mauiRPG.Models
{
    public partial class CombatantModel : Character
    {
        [ObservableProperty] private bool _isDefending;

        public CombatantModel()
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