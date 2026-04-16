using CommunityToolkit.Mvvm.Input;
using RedPrince.Models.Titles;
using RedPrince.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class CreateAccountViewModel
    {
        public string Title => TitleCreate.Title;

        public CreateAccountViewModel()
        {

        }
    }
}
