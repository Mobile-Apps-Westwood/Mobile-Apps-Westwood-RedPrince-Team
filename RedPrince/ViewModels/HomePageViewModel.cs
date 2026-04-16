using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {

        public string Title => HomePageTitles.Title;
        public string GamesButton => HomePageTitles.GamesButton;
        public string LeaderboardButton => HomePageTitles.LeaderboardButton;
        public string StoreButton => HomePageTitles.StoreButton;
        public string SettingsButton => HomePageTitles.SettingsButton;

        [RelayCommand]
        private async Task GamesButtonClicked()
        {
            await Shell.Current.GoToAsync(nameof());
        }

        [RelayCommand]
        private async Task LeaderBoardsButtonClicked()
        {
            await Shell.Current.GoToAsync(nameof());
        }

        [RelayCommand]
        private async Task StoreButtonClicked()
        {
            await Shell.Current.GoToAsync(nameof());
        }
        [RelayCommand]
        private async Task SettingsButtonClicked()
        {
            await Shell.Current.GoToAsync(nameof());
        }

        public HomePageViewModel()
        {
        }

    }
}
