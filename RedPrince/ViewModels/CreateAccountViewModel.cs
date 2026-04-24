using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using RedPrince.Models.Titles;
using RedPrince.Views;
using RedPrince.Services;
using RedPrince.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedPrince.ViewModels
{
    public partial class CreateAccountViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public string Title => TitleCreate.Title;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
        private string username;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
        private string password;

        [ObservableProperty]
        private string hintEntry;

        private bool CanCreate()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        public CreateAccountViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand(CanExecute = nameof(CanCreate))]
        private async Task Create()
        {
            var existingUser = await _databaseService.GetUserByUsernameAsync(Username);
            if (existingUser != null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Username already exists.", "OK");
                return;
            }

            var newUser = new User
            {
                Username = Username,
                Password = Password,
                Hint = HintEntry,
                Money = 1000 // Starting money
            };

            await _databaseService.CreateUserAsync(newUser);
            await App.Current.MainPage.DisplayAlert("Success", "Account created successfully!", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
