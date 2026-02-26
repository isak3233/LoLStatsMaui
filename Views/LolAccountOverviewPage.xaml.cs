using System;
using System.Collections.Generic;
using System.Text;
using LoLStatsMaui.ViewModels;

namespace LoLStatsMaui.Views
{
    public partial class LolAccountOverviewPage : ContentPage
    {
        public LolAccountOverviewPage(string searchedAccount)
        {
            InitializeComponent();
            BindingContext = new LolAccountOverViewModel(searchedAccount);
        }
        
    }
}
