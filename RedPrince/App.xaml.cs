using Microsoft.Extensions.DependencyInjection;

namespace RedPrince
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            LoginPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}