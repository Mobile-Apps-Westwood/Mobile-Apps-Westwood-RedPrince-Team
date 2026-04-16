using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class MainPage : ContentView
{
	public MainPage()
	{
		InitializeComponent();
        BindingContext = new MainPageViewModel();

    }
}