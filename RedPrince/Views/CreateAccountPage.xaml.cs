using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class CreateAccountPage : ContentPage
{
	public CreateAccountPage()
	{
		InitializeComponent();
        BindingContext = new CreateAccountViewModel();
    }
}