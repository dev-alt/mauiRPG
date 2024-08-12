using mauiRPG.Views;

namespace mauiRPG
{
    public partial class App : Application
    {
        public App(MainMenuView mainMenuView)
        {
            InitializeComponent();

            MainPage = new AppShell();
            Shell.Current.Navigation.PushAsync(mainMenuView);

        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

#if WINDOWS
            if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            {
                window.Width = 600;
                window.Height = 800;
            }
#endif

            return window;
        }
    }
}