using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class CreateAccountPage : ContentPage
{
	public CreateAccountPage(CreateAccountViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}