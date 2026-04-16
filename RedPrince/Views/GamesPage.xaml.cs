using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class GamesPage : ContentPage
{
	public GamesPage()
	{
		InitializeComponent();
        BindingContext = new GamesViewModel();

    }
}