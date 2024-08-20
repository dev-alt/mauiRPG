using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class CombatView : ContentView
{
	public CombatView()
	{
		InitializeComponent();
        AnimateEnemyAppearance();
    }
    private async void AnimateEnemyAppearance()
    {
        // Fade in animation
        await EnemyImage.FadeTo(1, 1000);

        // Scale up animation
        await EnemyImage.ScaleTo(1, 1000, Easing.SpringOut);
    }
    public void SetCombatViewModel(CombatViewModel viewModel)
    {
        BindingContext = viewModel;
    }
}