using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class SettingsChangeUserPage : ContentPage
{
	public SettingsChangeUserPage()
	{
		InitializeComponent();
		BindingContext = new SettingsChangeUserViewModel();
    }
}