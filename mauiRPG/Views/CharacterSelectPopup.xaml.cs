using CommunityToolkit.Maui.Views;
using mauiRPG.Services;
using mauiRPG.ViewModels;
using Microsoft.Extensions.Logging;

namespace mauiRPG.Views
{
    public partial class CharacterSelectPopup : Popup
    {
        public CharacterSelectPopup(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}