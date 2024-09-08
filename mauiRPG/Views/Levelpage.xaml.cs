using mauiRPG.ViewModels;

namespace mauiRPG.Views
{
    [QueryProperty(nameof(LevelNumber), "levelNumber")]
    public partial class LevelPage : ContentPage
    {
        private readonly LevelPageViewModel _viewModel;

        public int LevelNumber
        {
            set => _viewModel.LoadLevelCommand.Execute(value);
        }

        public LevelPage(LevelPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
}