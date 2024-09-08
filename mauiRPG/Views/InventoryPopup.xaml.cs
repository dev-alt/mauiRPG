using mauiRPG.ViewModels;
using mauiRPG.Services;
using CommunityToolkit.Maui.Views;

namespace mauiRPG.Views
{
    public partial class InventoryPopup : Popup
    {
        private readonly InventoryViewModel _viewModel;

        public InventoryPopup(InventoryViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
            _viewModel.CloseRequested += OnCloseRequested;
        }

        private void OnCloseRequested(object? sender, EventArgs e)
        {
            Close();
        }

    }
}
