using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class LevelSelectView : ContentPage
{
    public LevelSelectView(StageSelectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is StageSelectViewModel viewModel)
        {
            viewModel.LoadData();
        }
    }
}