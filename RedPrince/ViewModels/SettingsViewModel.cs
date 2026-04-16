using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using RedPrince.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        public string Title => TitleSettings.Title;

        public string ChangeUserPass => TitleSettings.ChangeUserPass;

        public string ToggleSound => TitleSettings.ToggleSound;



        [RelayCommand]
        private async Task ChangeUserPassClicked()
        {
            await Shell.Current.GoToAsync(nameof(SettingsChangeUserPassPage));
        }

        [RelayCommand]
        private async Task ToggleSoundClicked()
        {
            //activate the switch
        }
    }
}
