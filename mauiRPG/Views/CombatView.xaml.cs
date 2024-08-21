using mauiRPG.ViewModels;

namespace mauiRPG.Views
{
    public partial class CombatView : ContentView
    {
        public CombatView()
        {
            InitializeComponent();
        }

        public void SetCombatViewModel(CombatViewModel viewModel)
        {
            BindingContext = viewModel;
            viewModel.AnimationRequested += OnAnimationRequested;
        }

        private async void OnAnimationRequested(object? sender, string animationType)
        {
            switch (animationType)
            {
                case "PlayerAttack":
                    await AnimateAttack(PlayerPlaceholder, EnemyPlaceholder);
                    break;
                case "EnemyAttack":
                    await AnimateAttack(EnemyPlaceholder, PlayerPlaceholder);
                    break;
            }
        }

        private async Task AnimateAttack(VisualElement attacker, VisualElement target)
        {
            // Create a projectile
            var projectile = new BoxView
            {
                Color = Colors.Yellow,
                WidthRequest = 20,
                HeightRequest = 20,
                CornerRadius = 10,
            };
            AnimationLayer.Children.Add(projectile);

            // Set initial position
            AbsoluteLayout.SetLayoutBounds(projectile, new Rect(
                attacker.X + attacker.Width / 2,
                attacker.Y + attacker.Height / 2,
                projectile.WidthRequest,
                projectile.HeightRequest));

            // Animate projectile
            await projectile.TranslateTo(
                target.X - attacker.X,
                target.Y - attacker.Y,
                250, Easing.CubicOut);

            // Flash the target
            await target.FadeTo(0.5, 100);
            await target.FadeTo(1, 100);

            // Remove projectile
            AnimationLayer.Children.Remove(projectile);
        }
    }
}