using CommunityToolkit.Mvvm.ComponentModel;
using RedPrince.Models.Titles;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedPrince.ViewModels
{
    public partial class ChangeUserPassViewModel : ObservableObject
    {
        public string Title => TitleSettings.ChangeUserPass;

    }
}
