using mauiRPG.ViewModels;
using mauiRPG.Views;
using Microsoft.Extensions.Logging;

namespace mauiRPG
{
    public partial class App : Application
    {
        public App(MainMenuView mainMenuView, AppShellViewModel appShellViewModel, ILogger<AppShell> logger)
        {
            InitializeComponent();
            MainPage = new AppShell(appShellViewModel, logger);
            Shell.Current.Navigation.PushAsync(mainMenuView);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

#if WINDOWS
            if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            {
                window.Width = 800;
                window.Height = 900;
            }
#endif

            return window;
        }
    }
}