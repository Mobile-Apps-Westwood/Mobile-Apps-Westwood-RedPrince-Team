using RedPrince.ViewModels;

namespace RedPrince.Views;

public partial class LeaderboardPage : ContentPage
{
	public LeaderboardPage(LeaderboardViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }
}