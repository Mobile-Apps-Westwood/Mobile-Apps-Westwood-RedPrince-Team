using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using RedPrince.Models.Titles;
using System.Threading.Tasks;

namespace RedPrince.ViewModels
{
    public partial class SettingsChangeUserViewModel : ObservableObject
    {
        public string Title => TitleSettings.ChangeUser;

        private readonly Services.DatabaseService _databaseService;

        public SettingsChangeUserViewModel(Services.DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string newUsername;

        [RelayCommand]
        private async Task SubmitChangeUser()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(NewUsername))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            var user = await _databaseService.GetUserAsync(Username, Password);
            if (user == null)
            {
                await Shell.Current.DisplayAlert("Error", "Invalid username or password.", "OK");
                return;
            }

            var existingUser = await _databaseService.GetUserByUsernameAsync(NewUsername);
            if (existingUser != null)
            {
                await Shell.Current.DisplayAlert("Error", "New username already exists.", "OK");
                return;
            }

            user.Username = NewUsername;
            await _databaseService.UpdateUserAsync(user);

            var currentUser = Preferences.Get("CurrentUser", string.Empty);
            if (currentUser == Username)
            {
                Preferences.Set("CurrentUser", NewUsername);
            }

            await Shell.Current.DisplayAlert("Success", "Username updated successfully.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
