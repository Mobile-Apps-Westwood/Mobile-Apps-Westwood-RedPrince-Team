using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class LeaderboardPage : ContentPage
{
	public LeaderboardPage()
	{
		InitializeComponent();
        BindingContext = new LeaderboardViewModel();

    }
}