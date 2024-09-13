using CommunityToolkit.Maui.Views;
using mauiRPG.Services;
using mauiRPG.ViewModels;
using Microsoft.Extensions.Logging;

namespace mauiRPG.Views;

public partial class CharacterCreate : ContentPage
{

    private readonly CharacterCreationViewModel _viewModel;

    public CharacterCreate(CharacterService characterService, GameStateService gameStateService, ILogger<CharacterCreationViewModel> logger)
    {
        InitializeComponent();
        _viewModel = new CharacterCreationViewModel(characterService, gameStateService, logger);
        _viewModel.ShowErrorRequested += OnShowErrorRequested;
        _viewModel.ShowSuccessRequested += OnShowSuccessRequested;
        BindingContext = _viewModel;
    }

    private async void OnShowErrorRequested(object? sender, string message)
    {
        var errorPopup = new ErrorPopup(message);
        await this.ShowPopupAsync(errorPopup);
    }

    private async void OnShowSuccessRequested(object? sender, string message)
    {
        var successPopup = new ErrorPopup(message);
        await this.ShowPopupAsync(successPopup);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.ShowErrorRequested -= OnShowErrorRequested;
        _viewModel.ShowSuccessRequested -= OnShowSuccessRequested;
    }
}