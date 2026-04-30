using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class SettingsChangeUserPage : ContentPage
{
	public SettingsChangeUserPage(SettingsChangeUserViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}