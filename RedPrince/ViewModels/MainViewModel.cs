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


        [RelayCommand]
        private async Task HintClicked()
        {
            await Shell.Current.GoToAsync(nameof(HintPage));
        }

        //[RelayCommand]
        //private async Task LoginCreateClicked()
        //{
        //    await Shell.Current.GoToAsync(nameof());
        //}

        [RelayCommand]
        private async Task HomeClicked()
        {
            await Shell.Current.GoToAsync(nameof(HomePage));
        }

        public MainViewModel()
        {

        }
    }
}
