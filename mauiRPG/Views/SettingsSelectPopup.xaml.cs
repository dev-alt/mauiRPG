using mauiRPG.Services;
using mauiRPG.ViewModels;

namespace mauiRPG.Views;

public partial class SettingsSelectPopup : ContentView
{
	public SettingsSelectPopup()
    {
        InitializeComponent();
        if (Application.Current == null) return;
        if (Application.Current.Handler != null && Application.Current.Handler.MauiContext != null)
            BindingContext =
                new SettingsViewModel(
                    Application.Current.Handler.MauiContext.Services.GetService<ISettingsService>() ??
                    throw new InvalidOperationException());
    }
}