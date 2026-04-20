using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class HintPage : ContentPage
{
	public HintPage()
	{
        InitializeComponent();
        BindingContext = new HintViewModel();
    }
}