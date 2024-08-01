using mauiRPG.Models;
using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class LevelPage : ContentPage
{
	public LevelPage(Level level)
	{
		InitializeComponent();
        BindingContext = new LevelDetailsViewModel(level);
    }
}