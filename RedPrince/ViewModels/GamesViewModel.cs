using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using RedPrince.Views;
using System;
using System.Collections.Generic;

using System.Text;
using RedPrince.Views.GameBlackJackViews;

namespace RedPrince.ViewModels
{
    public partial class GamesViewModel
    {
        public string Title => TitleGames.Title;

        public string BlackJackPage => TitleGames.BlackJack;

        [RelayCommand]
        private async Task  BlackJackPageClicked()
        {
            await Shell.Current.GoToAsync(nameof(BlackJackPage));
        }
    }
}
