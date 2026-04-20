
using RedPrince.Views;

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


            //Home
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));

            Routing.RegisterRoute(nameof(GamesPage), typeof(GamesPage));
            Routing.RegisterRoute(nameof(LeaderboardPage), typeof(LeaderboardPage));
            Routing.RegisterRoute(nameof(StorePage), typeof(StorePage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(SettingsChangeUserPassPage), typeof(SettingsChangeUserPassPage));

        }
    }
}
