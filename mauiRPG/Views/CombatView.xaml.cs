using mauiRPG.ViewModels;

namespace mauiRPG.Views
{
    public partial class CombatView : ContentView
    {
        public CombatView()
        {
            InitializeComponent();
            BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object? sender, EventArgs e)
        {
            if (BindingContext is CombatViewModel viewModel)
            {
                viewModel.SetCombatView(this);
            }
        }

        public async Task ShowPlayerDamage(int damage)
        {
            await PlayerDamageSplash.ShowDamage(damage);
        }

        public async Task ShowEnemyDamage(int damage)
        {
            await EnemyDamageSplash.ShowDamage(damage);
        }
        public async Task ShowPlayerBattleZoneDamage(int damage)
        {
            await PlayerBattleZoneDamageSplash.ShowDamage(damage);
        }

        public async Task ShowEnemyBattleZoneDamage(int damage)
        {
            await EnemyBattleZoneDamageSplash.ShowDamage(damage);
        }
    }
}