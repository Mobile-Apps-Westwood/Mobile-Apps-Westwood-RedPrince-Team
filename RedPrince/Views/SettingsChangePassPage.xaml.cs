using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class SettingsChangePassPage : ContentPage
{
	public SettingsChangePassPage()
	{
		InitializeComponent();
		BindingContext = new SettingsChangePassViewModel();
    }
}