using mauiRPG.ViewModels;
using System.ComponentModel;
using System.Diagnostics;

namespace mauiRPG.Views
{
    public partial class CombatView : ContentView
    {

        private const int MaxCombatLogEntries = 10;

        public CombatView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ViewModelProperty =
            BindableProperty.Create(nameof(ViewModel), typeof(CombatViewModel), typeof(CombatView), null,
                propertyChanged: OnViewModelChanged);

        public CombatViewModel ViewModel
        {
            get => (CombatViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        private static void OnViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var combatView = (CombatView)bindable;
            if (newValue is CombatViewModel viewModel)
            {
                combatView.BindingContext = viewModel;
                viewModel.PropertyChanged += combatView.OnViewModelPropertyChanged;
                viewModel.AnimationRequested += combatView.OnAnimationRequested;
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CombatViewModel.CombatLog))
            {
                UpdateCombatLog();
            }
        }
        private void UpdateCombatLog()
        {
            var combatLog = (ViewModel.CombatLog ?? "").Split('\n');
            CombatLogContainer.Children.Clear();

            foreach (var entry in combatLog.TakeLast(MaxCombatLogEntries))
            {
                var label = new Label
                {
                    Text = entry,
                    TextColor = entry.StartsWith(ViewModel.PlayerName) ? Colors.LightBlue : Colors.LightPink
                };
                CombatLogContainer.Children.Add(label);
            }

            Dispatcher.Dispatch(() => CombatLogScrollView.ScrollToAsync(0, CombatLogContainer.Height, false));
        }
        protected override void OnParentSet()
        {
            base.OnParentSet();
            Debug.WriteLine($"CombatView ParentSet. IsVisible: {IsVisible}, BindingContext: {BindingContext}");
        }

        private async void OnAnimationRequested(object? sender, string animationType)
        {
            switch (animationType)
            {
                case "PlayerAttack":
                    await AnimateAttack(PlayerInfoFrame, EnemyInfoFrame);
                    break;
                case "EnemyAttack":
                    await AnimateAttack(EnemyInfoFrame, PlayerInfoFrame);
                    break;
            }
        }

        public void SetCombatViewModel(CombatViewModel viewModel)
        {
            Debug.WriteLine("SetCombatViewModel called");
            BindingContext = viewModel;
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

            // Add the projectile to the main grid
            MainGrid.Children.Add(projectile);

            // Get the positions of the attacker and target
            var attackerBounds = attacker.Bounds;
            var targetBounds = target.Bounds;

            // Set initial position
            AbsoluteLayout.SetLayoutBounds(projectile, new Rect(
                attackerBounds.Center.X - projectile.Width / 2,
                attackerBounds.Center.Y - projectile.Height / 2,
                projectile.Width,
                projectile.Height));

            // Animate projectile
            await projectile.TranslateTo(
                targetBounds.Center.X - attackerBounds.Center.X,
                targetBounds.Center.Y - attackerBounds.Center.Y,
                250, Easing.CubicOut);

            // Flash the target
            await target.FadeTo(0.5, 100);
            await target.FadeTo(1, 100);

            // Remove projectile
            MainGrid.Children.Remove(projectile);
        }
    }
}