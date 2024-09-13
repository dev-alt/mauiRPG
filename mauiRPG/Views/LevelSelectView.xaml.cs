using mauiRPG.Services;
using mauiRPG.ViewModels;
using Microsoft.Extensions.Logging;

namespace mauiRPG.Views;

public partial class LevelSelectView : ContentPage
{
    public LevelSelectView(GameStateService gameStateService, ILogger<PlayerInfoPopup> logger)
    {
        InitializeComponent();
        BindingContext = new StageSelectViewModel(gameStateService, logger, this);
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