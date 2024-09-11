using CommunityToolkit.Maui.Views;
using mauiRPG.ViewModels;

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
