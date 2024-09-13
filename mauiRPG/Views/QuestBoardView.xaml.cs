using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class QuestBoardView : ContentPage
{
    private readonly QuestBoardViewModel _viewModel;
    public QuestBoardView(QuestBoardViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnDisappearing();
    }
}
