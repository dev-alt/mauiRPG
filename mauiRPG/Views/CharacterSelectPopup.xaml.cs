using CommunityToolkit.Maui.Views;
using mauiRPG.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Maui.Core;

namespace mauiRPG.Views
{
    public partial class CharacterSelectPopup : Popup
    {
        private readonly CharacterSelectViewModel _viewModel;

        public event EventHandler<string>? ErrorOccurred;

        public CharacterSelectPopup(CharacterSelectViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
            _viewModel.CloseRequested += (sender, args) => Close();
            _viewModel.ErrorOccurred += OnErrorOccurred;
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (Handler != null)
            {
                this.Closed += OnPopupClosed;
            }
        }

        private void OnPopupClosed(object? sender, PopupClosedEventArgs e)
        {
            WeakReferenceMessenger.Default.Send(new PopupClosedMessage());
        }

        private void OnErrorOccurred(object? sender, string errorMessage)
        {
            ErrorOccurred?.Invoke(this, errorMessage);
        }
    }
}