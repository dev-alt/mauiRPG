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
    }
}
