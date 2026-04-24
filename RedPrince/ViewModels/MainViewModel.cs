using CommunityToolkit.Mvvm.ComponentModel;
using RedPrince.Models.Titles;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using RedPrince.Views;

namespace RedPrince.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public string Title => TitleMain.Title;

        public string Hint => TitleMain.Hint;

        public string Create => TitleMain.Create;

        public string Home => TitleMain.Home;

        public ImageSource ImageSource => "redprince_logo.png";


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(HomeClickedCommand))]
        private string username;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(HomeClickedCommand))]
        private string password;

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        [RelayCommand]
        private async Task HintClicked()
        {
            await Shell.Current.GoToAsync(nameof(HintPage));
        }

        [RelayCommand]
        private async Task CreateClicked()
        {
            await Shell.Current.GoToAsync(nameof(CreateAccountPage));
        }

        private readonly RedPrince.Services.DatabaseService _databaseService;

        public MainViewModel(RedPrince.Services.DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task HomeClicked()
        {
            var user = await _databaseService.GetUserAsync(Username, Password);
            if (user != null)
            {
                Preferences.Set("CurrentUser", user.Username);
                await Shell.Current.GoToAsync(nameof(HomePage));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Invalid username or password.", "OK");
            }
        }
    }
}
