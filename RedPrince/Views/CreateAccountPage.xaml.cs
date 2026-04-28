using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class CreateAccountPage : ContentPage
{
	public CreateAccountPage(CreateAccountViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private async void OnPasswordUnfocused(object sender, FocusEventArgs e)
	{
		if (BindingContext is CreateAccountViewModel viewModel)
		{
			await viewModel.ValidatePasswordEntryCommand.ExecuteAsync(null);
		}
	}
}