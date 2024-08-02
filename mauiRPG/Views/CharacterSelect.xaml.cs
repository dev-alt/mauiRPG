using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class CharacterSelect : ContentPage
{
	public CharacterSelect()
	{
		InitializeComponent();
        BindingContext = new CharacterCreationViewModel();
    }

}