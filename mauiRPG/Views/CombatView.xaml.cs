using mauiRPG.ViewModels;
using System.ComponentModel;
using System.Diagnostics;

namespace mauiRPG.Views
{
    public partial class CombatView : ContentView
    {
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
            Debug.WriteLine($"CombatViewModel property changed: {e.PropertyName}");
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
                    await AnimateAttack(PlayerPlaceholder, EnemyPlaceholder);
                    break;
                case "EnemyAttack":
                    await AnimateAttack(EnemyPlaceholder, PlayerPlaceholder);
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