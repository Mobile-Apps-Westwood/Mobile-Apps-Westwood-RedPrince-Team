using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class StorePage : ContentPage
{
	public StorePage()
	{
		InitializeComponent();
		BindingContext = new StoreViewModel();
	}
}