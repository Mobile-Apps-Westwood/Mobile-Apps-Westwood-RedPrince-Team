
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
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));

            

            //Settings
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(SettingsChangeUserPassPage), typeof(SettingsChangeUserPassPage));

        }
    }
}
