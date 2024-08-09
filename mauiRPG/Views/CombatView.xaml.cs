using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class CombatView : ContentView
{
	public CombatView()
	{
		InitializeComponent();
	}
    public void SetCombatViewModel(CombatViewModel viewModel)
    {
        BindingContext = viewModel;
    }
}