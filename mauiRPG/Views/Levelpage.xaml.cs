using mauiRPG.ViewModels;

namespace mauiRPG.Views;

[QueryProperty(nameof(LevelNumber), "levelNumber")]
public partial class LevelPage : ContentPage
{
    public LevelPage(LevelPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    public int LevelNumber
    {
        set
        {
            if (BindingContext is LevelPageViewModel viewModel)
            {
                viewModel.LevelNumber = value;
            }
        }
    }

}