using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class QuestBoardView : ContentPage
{
    public QuestBoardView(QuestBoardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        Console.WriteLine("QuestBoardView constructor called");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("QuestBoardView OnAppearing called");
    }
}