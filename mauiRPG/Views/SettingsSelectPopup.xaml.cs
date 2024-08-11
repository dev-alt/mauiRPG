using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using mauiRPG.Services;
using mauiRPG.ViewModels;

namespace mauiRPG.Views
{
    public partial class SettingsSelectPopup : Popup
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsSelectPopup(ISettingsService settingsService)
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel(settingsService);
            BindingContext = _viewModel;

            _viewModel.CloseRequested += (sender, args) => Close();
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            if (Handler != null)
            {
                this.Closed += OnPopupClosed;
            }
        }

        private void OnPopupClosed(object sender, PopupClosedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new PopupClosedMessage());
        }
    }

    public class PopupClosedMessage { }
}