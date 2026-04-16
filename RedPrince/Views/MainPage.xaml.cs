using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class LoginPage : ContentView
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel();

    }
}