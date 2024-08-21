using mauiRPG.ViewModels;
using mauiRPG.Services;

namespace mauiRPG.Views
{
    public partial class InventoryView : ContentView
    {
        private readonly InventoryViewModel _viewModel;

        public InventoryView(GameStateService gameStateService)
        {
            InitializeComponent();
            _viewModel = new InventoryViewModel(gameStateService);
            BindingContext = _viewModel;
        }
    }
}