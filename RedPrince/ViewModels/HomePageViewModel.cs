using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {

        //public string Title => ;
        //public string Collection => ;
        //public string CollectionImage => ;
        //public string CollectionButton => ;

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
