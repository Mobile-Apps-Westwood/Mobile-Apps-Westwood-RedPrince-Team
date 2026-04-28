using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using RedPrince.Services;
using RedPrince.Models;

namespace RedPrince.ViewModels
{
    public partial class HintViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public string Title => TitleHint.Title;
        public string Submit => TitleHint.Submit;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
        private string username;

        [ObservableProperty]
        private string hintText;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool showHint;

        [ObservableProperty]
        private string errorMessage;

        private User _currentUser;

        public HintViewModel()
        {
            _databaseService = new DatabaseService();
            showHint = false;
            errorMessage = string.Empty;
        }

        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(Username) && !IsLoading;
        }

        [RelayCommand(CanExecute = nameof(CanSearch))]
        private async Task Search()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Please enter a username.";
                ShowHint = false;
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                _currentUser = await _databaseService.GetUserByUsernameAsync(Username.Trim());

                if (_currentUser == null)
                {
                    ErrorMessage = "Username not found.";
                    ShowHint = false;
                    HintText = string.Empty;
                }
                else if (string.IsNullOrWhiteSpace(_currentUser.Hint))
                {
                    ErrorMessage = "No hint set for this account.";
                    ShowHint = false;
                    HintText = string.Empty;
                }
                else
                {
                    HintText = _currentUser.Hint;
                    ShowHint = true;
                    ErrorMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                ShowHint = false;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task ClearHint()
        {
            Username = string.Empty;
            HintText = string.Empty;
            ShowHint = false;
            ErrorMessage = string.Empty;
            _currentUser = null;
        }
    }
}
