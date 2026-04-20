namespace RedPrince.Views.GameBlackJackViews;

using RedPrince.ViewModels.GameBlackJackViewModels;

public partial class BlackJackPage : ContentPage
{
    public BlackJackPage()
    {
        InitializeComponent();
        BindingContext = new GameViewModel();
    }
}

