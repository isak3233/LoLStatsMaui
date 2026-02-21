using LoLStatsMaui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LoLStatsMaui.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {
        public ICommand SubmitCommand { get; }

        public Array Regions { get; }

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private string _lolName;

        [ObservableProperty]
        private Models.Region _selectedRegion;


        public MainViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
            Regions = Enum.GetValues(typeof(Models.Region));
        }
        private void OnSubmit()
        {
            if (LolName == null)
            {
                ErrorMessage = "Du skrev inget!";
                return;
            }
            if(!LolName.Contains("#"))
            {
                ErrorMessage = "Det sökta kontot behöver innehålla #";
                return;
            }
            ErrorMessage = "";

        }

    }
}
