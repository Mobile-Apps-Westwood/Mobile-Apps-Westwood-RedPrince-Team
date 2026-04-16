using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new HomeViewModel();

    }
}