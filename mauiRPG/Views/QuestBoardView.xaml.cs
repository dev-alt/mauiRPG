using mauiRPG.Services;
using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class QuestBoardView : ContentPage
{
    public QuestBoardView(GameStateService gameStateService, IQuestService questService, IDialogService dialogService)
    {
        InitializeComponent();
        BindingContext = new QuestBoardViewModel(gameStateService, questService, dialogService, this);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}