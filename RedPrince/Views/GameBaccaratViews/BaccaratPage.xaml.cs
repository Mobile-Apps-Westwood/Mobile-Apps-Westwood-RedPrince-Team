using RedPrince.ViewModels.GameBaccaratViewModels;

namespace RedPrince.Views.GameBaccaratViews;

public partial class BaccaratPage : ContentPage
{
    public BaccaratPage(BaccaratViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}