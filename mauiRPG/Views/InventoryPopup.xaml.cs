using mauiRPG.ViewModels;
using mauiRPG.Services;
using CommunityToolkit.Maui.Views;

namespace mauiRPG.Views
{
    public partial class InventoryPopup : Popup
    {
        public InventoryPopup(InventoryViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}