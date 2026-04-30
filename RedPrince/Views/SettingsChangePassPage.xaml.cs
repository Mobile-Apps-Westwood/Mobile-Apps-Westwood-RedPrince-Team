using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class SettingsChangePassPage : ContentPage
{
	public SettingsChangePassPage(SettingsChangePassViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}