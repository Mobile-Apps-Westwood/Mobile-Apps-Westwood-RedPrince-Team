
using RedPrince.Views;
using RedPrince.Views.GameBlackJackViews;

namespace RedPrince
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRouting();
        }

        public void RegisterRouting()
        {

            //Login
            Routing.RegisterRoute(nameof(HintPage), typeof(HintPage));
            Routing.RegisterRoute(nameof(CreateAccountPage), typeof(CreateAccountPage));

            //Home
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));

            Routing.RegisterRoute(nameof(GamesPage), typeof(GamesPage));
            Routing.RegisterRoute(nameof(LeaderboardPage), typeof(LeaderboardPage));
            Routing.RegisterRoute(nameof(StorePage), typeof(StorePage));

            //Settings
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(SettingsChangeUserPage), typeof(SettingsChangeUserPage));
            Routing.RegisterRoute(nameof(SettingsChangePassPage), typeof(SettingsChangePassPage));
            Routing.RegisterRoute(nameof(BlackJackPage), typeof(BlackJackPage));

            Routing.RegisterRoute(nameof(StorePage), typeof(StorePage));
        }
    }
}
