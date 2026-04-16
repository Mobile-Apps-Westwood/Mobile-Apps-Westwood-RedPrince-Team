using CommunityToolkit.Mvvm.ComponentModel;
using RedPrince.Models.Titles;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public string Title => TitleMain.Title;

        public string Hint => TitleMain.Hint;

        public string Create => TitleMain.Create;

        public string Home => TitleMain.Home;


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

        public MainViewModel()
        {

        }
    }
}
