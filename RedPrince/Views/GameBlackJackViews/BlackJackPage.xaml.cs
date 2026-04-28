namespace RedPrince.Views.GameBlackJackViews;

using RedPrince.ViewModels.GameBlackJackViewModels;

public partial class BlackJackPage : ContentPage
{
    public BlackJackPage(GameViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

