using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using RedPrince.Models.Titles;
using RedPrince.Views;
using RedPrince.Services;
using RedPrince.Models;
using System.Collections.ObjectModel;

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
        [NotifyCanExecuteChangedFor(nameof(CreateCommand))]
        private string hintEntry;

        private bool CanCreate()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(HintEntry);
        }

        public CreateAccountViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        partial void OnPasswordChanged(string value)
        {
            // Validation moved to separate method called on unfocus
            // No longer validates on every character change
        }

        [RelayCommand]
        public async Task ValidatePasswordEntry()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                return;
            }

            var result = PasswordValidator.ValidatePassword(Password);

            if (!result.IsValid)
            {
                // Build error message from validation errors only
                string errorMessage = string.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessage += "✗ " + error + "\n";
                }

                // Show error popup
                await App.Current.MainPage.DisplayAlert(
                    "Password Requirements Error",
                    errorMessage.Trim(),
                    "OK");
            }
        }

        [RelayCommand(CanExecute = nameof(CanCreate))]
        private async Task Create()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Username is required.", "OK");
                return;
            }

            // Final validation before creating account
            var validationResult = PasswordValidator.ValidatePassword(Password);
            if (!validationResult.IsValid)
            {
                string errorMessage = string.Empty;
                foreach (var error in validationResult.Errors)
                {
                    errorMessage += "✗ " + error + "\n";
                }

                await App.Current.MainPage.DisplayAlert(
                    "Password Requirements Error",
                    errorMessage.Trim(),
                    "OK");
                return;
            }

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
