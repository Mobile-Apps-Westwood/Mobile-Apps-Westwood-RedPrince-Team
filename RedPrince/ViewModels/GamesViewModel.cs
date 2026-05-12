using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using RedPrince.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class GamesViewModel
    {
        public string Title => TitleGames.Title;

        public string BlackJackPage => TitleGames.BlackJack;

        public string BaccaratPage => TitleGames.Baccarat;

        public string ImageSource => "redprince_logo.png";

        [RelayCommand]
        private async Task  BlackJackPageClicked()
        {
            await Shell.Current.GoToAsync(nameof(RedPrince.Views.BlackJackPage));
        }

        [RelayCommand]
        private async Task BaccaratPageClicked()
        {
            await Shell.Current.GoToAsync(nameof(BaccaratPage));
        }
    }
}
