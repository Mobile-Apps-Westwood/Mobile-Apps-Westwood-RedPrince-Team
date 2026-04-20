using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using RedPrince.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {

        public string Title => TitleHome.Title;
        public string GamesButton => TitleHome.GamesButton;
        public string LeaderboardButton => TitleHome.LeaderboardButton;
        public string StoreButton => TitleHome.StoreButton;
        public string SettingsButton => TitleHome.SettingsButton;

        [RelayCommand]
        private async Task GamesButtonClicked()
        {
            await Shell.Current.GoToAsync(nameof(GamesPage));
        }

        [RelayCommand]
        private async Task LeaderBoardsButtonClicked()
        {
            await Shell.Current.GoToAsync(nameof(LeaderboardPage));
        }

        [RelayCommand]
        private async Task StoreButtonClicked()
        {
            await Shell.Current.GoToAsync(nameof(StorePage));
        }
        [RelayCommand]
        private async Task SettingsButtonClicked()
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }

        public HomeViewModel()
        {
        }

    }
}
