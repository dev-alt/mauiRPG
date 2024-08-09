using mauiRPG.Services;
using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class SettingsSelectPopup : ContentView
{
	public SettingsSelectPopup()
	{
		InitializeComponent();
        BindingContext = new SettingsViewModel(Application.Current.Handler.MauiContext.Services.GetService<ISettingsService>());
    }
}