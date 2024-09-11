using mauiRPG.ViewModels;
using mauiRPG.Views;

namespace mauiRPG
{
    public partial class App : Application
    {
        public App(MainMenuView mainMenuView, AppShellViewModel appShellViewModel)
        {
            InitializeComponent();
            MainPage = new AppShell(appShellViewModel);
            Shell.Current.Navigation.PushAsync(mainMenuView);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

#if WINDOWS
            if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            {
                window.Width = 800;
                window.Height = 1200;
            }
#endif

            return window;
        }
    }
}