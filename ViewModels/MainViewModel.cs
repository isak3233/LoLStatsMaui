
using CommunityToolkit.Mvvm.ComponentModel;
using LoLStatsMaui.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace LoLStatsMaui.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ICommand SubmitCommand { get; }


        private INavigation _navigation;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private string _lolName;


        public MainViewModel(INavigation navigation)
        {
            SubmitCommand = new Command(OnSubmit);
           
            _navigation = navigation;
        }
        private async void OnSubmit()
        {
            if (LolName == null || LolName == "")
            {
                ErrorMessage = "Du skrev inget!";
                return;
            }
            if(!(LolName.Count(c => c == '#') == 1))
            {
                ErrorMessage = "Det sökta kontot måste innehålla bara ETT #";
                return;
            }
            if (LolName.Split('#')[0].Length < 3)
            {
                ErrorMessage = "Det sökta spel namnet är mindre än 3 tecken";
                return;
            }
            if (LolName.Split('#')[1].Length < 3)
            {
                ErrorMessage = "Det sökta spel tagen är mindre än 3 tecken";
                return;
            }
            ErrorMessage = "";
            await _navigation.PushAsync(new LolAccountOverviewPage(LolName));

        }

    }
}
