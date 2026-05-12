using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class BaccaratPage : ContentPage
{
    public BaccaratPage(BaccaratViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}