using mauiRPG.ViewModels;
using mauiRPG.Services;

namespace mauiRPG.Views
{
    public partial class LevelSelectView : ContentPage
    {
        public LevelSelectView(GameStateService gameStateService, StageSelectViewModel viewModel)
        {
            InitializeComponent();
            Console.WriteLine("LevelSelectView constructor called");

            if (gameStateService.CurrentPlayer == null)
            {
                Console.WriteLine("CurrentPlayer is null in GameStateService");
            }
            else
            {
                Console.WriteLine($"CurrentPlayer in GameStateService: {gameStateService.CurrentPlayer.Name}");
            }

            viewModel.CurrentPlayer = gameStateService.CurrentPlayer;
            BindingContext = viewModel;

            Console.WriteLine($"BindingContext set to: {BindingContext}");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("LevelSelectView OnAppearing called");

            if (BindingContext is StageSelectViewModel viewModel)
            {
                Console.WriteLine($"CurrentPlayer in ViewModel: {viewModel.CurrentPlayer?.Name ?? "null"}");
                Console.WriteLine($"Number of Levels: {viewModel.Levels?.Count ?? 0}");
            }
            else
            {
                Console.WriteLine("BindingContext is not StageSelectViewModel");
            }
        }
    }
}