using mauiRPG.ViewModels;
using mauiRPG.Views;

namespace mauiRPG
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            Routing.RegisterRoute("MainMenu", typeof(MainMenuView));
            Routing.RegisterRoute("LevelSelect", typeof(LevelSelectView));
            Routing.RegisterRoute("CharacterSelect", typeof(CharacterSelect));
            Routing.RegisterRoute("QuestBoard", typeof(QuestBoardView));
            Routing.RegisterRoute("LevelPage", typeof(LevelPage));
        }
    }
}