using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using RedPrince.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace RedPrince.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        public string Title => TitleSettings.Title;

        public string ChangeUserPass => TitleSettings.ChangeUserPass;

        public string ToggleSound => TitleSettings.ToggleSound;

        public string ToggleTheme => TitleSettings.ToggleSound;

        [ObservableProperty]
        private bool isSoundOn;

        [ObservableProperty]
        private bool theme;

        public SettingsViewModel()
        {
            // initialize the theme switch from the current app theme
            IsDarkTheme = Application.Current?.RequestedTheme == AppTheme.Dark;
            // default for sound switch
            IsSoundOn = false;
        }



        [RelayCommand]
        private async Task ChangeUserPassClicked()
        {
            await Shell.Current.GoToAsync(nameof(SettingsChangeUserPassPage));
        }

        [RelayCommand]
        private async Task ToggleSoundClicked()
        {
            IsSoundOn = !IsSoundOn;
        }

        [RelayCommand]
        private async Task ToggleThemeClicked()
        {
            IsDarkTheme = !IsDarkTheme;
        }

        partial void OnIsDarkThemeChanged(bool value)
        {
            Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
        }
    }
}
