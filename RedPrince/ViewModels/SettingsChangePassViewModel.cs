using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using RedPrince.Models.Titles;
using RedPrince.Services;
using System.Threading.Tasks;

namespace RedPrince.ViewModels
{
    public partial class SettingsChangePassViewModel : ObservableObject
    {
        public string Title => TitleSettings.ChangePass;

        private readonly Services.DatabaseService _databaseService;

        public SettingsChangePassViewModel(Services.DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string currentPassword;

        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmNewPassword;

        [RelayCommand]
        private async Task SubmitChangePass()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmNewPassword))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            if (NewPassword != ConfirmNewPassword)
            {
                await Shell.Current.DisplayAlert("Error", "New passwords do not match.", "OK");
                return;
            }

            var validationResult = PasswordValidator.ValidatePassword(NewPassword);
            if (!validationResult.IsValid)
            {
                string errorMessage = string.Empty;
                foreach (var error in validationResult.Errors)
                {
                    errorMessage += "✗ " + error + "\n";
                }

                await Shell.Current.DisplayAlert(
                    "Password Requirements Error",
                    errorMessage.Trim(),
                    "OK");
                return;
            }

            var user = await _databaseService.GetUserAsync(Username, CurrentPassword);
            if (user == null)
            {
                await Shell.Current.DisplayAlert("Error", "Invalid username or password.", "OK");
                return;
            }

            user.Password = NewPassword;
            await _databaseService.UpdateUserAsync(user);

            await Shell.Current.DisplayAlert("Success", "Password updated successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
