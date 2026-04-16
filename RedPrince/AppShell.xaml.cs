
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

            //Home
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));


        }
    }
}
