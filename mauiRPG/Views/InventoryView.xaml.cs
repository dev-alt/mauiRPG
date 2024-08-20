

using mauiRPG.ViewModels;
namespace mauiRPG.Views;

public partial class InventoryView : ContentView
{
	public InventoryView()
	{
        InitializeComponent();
        BindingContext = new InventoryViewModel();
    }
}
