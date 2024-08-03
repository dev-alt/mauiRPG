using mauiRPG.Views;

namespace mauiRPG
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainMenuView), typeof(MainMenuView));
            Routing.RegisterRoute(nameof(LevelPage), typeof(LevelPage));
            Routing.RegisterRoute(nameof(PlayerInfoView), typeof(PlayerInfoView));
            Routing.RegisterRoute(nameof(CharacterSelect), typeof(CharacterSelect));
            Routing.RegisterRoute(nameof(LevelSelectView), typeof(LevelSelectView));
        }
    }
}
