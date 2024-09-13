using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class QuestBoardView : ContentPage
{
    public QuestBoardView(QuestBoardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is QuestBoardViewModel viewModel)
        {
            viewModel.OnAppearing();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is QuestBoardViewModel viewModel)
        {
            viewModel.OnDisappearing();
        }
    }
}