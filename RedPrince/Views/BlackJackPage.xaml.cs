namespace RedPrince.Views;

using RedPrince.ViewModels;

public partial class BlackJackPage : ContentPage
{
    public BlackJackPage(GameViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

