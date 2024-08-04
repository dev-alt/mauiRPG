using mauiRPG.ViewModels;

namespace mauiRPG.Views
{
    public partial class MainMenuView : ContentPage
    {
        public MainMenuView()
        {
            InitializeComponent();
        }

        private void OnBackgroundTapped(object sender, TappedEventArgs e)
        {
            var viewModel = (MainViewModel)BindingContext;

            if (viewModel.IsCharacterListVisible)
            {
                viewModel.CancelLoadCharacterCommand.Execute(null);
            }
            else if (viewModel.IsSettingsVisible)
            {
                viewModel.CancelSettingsCommand.Execute(null);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CharacterPopup.InputTransparent = false;
            SettingPopup.InputTransparent = false;
        }
    }
}
