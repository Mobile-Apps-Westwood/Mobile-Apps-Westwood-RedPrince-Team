using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class HintViewModel : ObservableObject
    {
        public string Title => TitleHint.Title;

        public string Submit => TitleHint.Submit;

        //[RelayCommand]
        //private async Task LoginSubmitClicked()
        //{
        //    await Shell.Current.GoToAsync(nameof());
        //}

    }
}
