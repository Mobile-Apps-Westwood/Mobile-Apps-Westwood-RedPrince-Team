using CommunityToolkit.Mvvm.ComponentModel;
using RedPrince.Models.Titles;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        public string Title => TitleLogin.Title;

        public string Hint => TitleLogin.Hint;

        public string Create => TitleLogin.Create;

        public string Home => TitleLogin.Home;


        [RelayCommand]
        private async Task LoginHintClicked()
        {
            await Shell.Current.GoToAsync(nameof());
        }

        [RelayCommand]
        private async Task LoginCreateClicked()
        {
            await Shell.Current.GoToAsync(nameof());
        }

        [RelayCommand]
        private async Task LoginHomeClicked()
        {
            await Shell.Current.GoToAsync(nameof(LoginHomeClicked));
        }

        public LoginViewModel()
        {

        }
    }
}
