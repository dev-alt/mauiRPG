using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class LevelSelectView : ContentPage
{
	public LevelSelectView()
	{
		InitializeComponent();
        BindingContext = new StageSelectViewModel();
    }
}