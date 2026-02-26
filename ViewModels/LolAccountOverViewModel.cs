using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoLStatsMaui.Models;
using LoLStatsMaui.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;

namespace LoLStatsMaui.ViewModels
{
    public partial class LolAccountOverViewModel : ObservableObject
    {
        private string _lolName;
        private ILolRepository _lolRepository;

        [ObservableProperty]
        private Summoner _summoner;

        [ObservableProperty]
        private ImageSource _profileImage;

        public LolAccountOverViewModel(string lolName)
        {
            ProfileImage = ImageSource.FromFile("none.png");
            _lolName = lolName;
            _lolRepository = new LolApiRepository(new HttpClient());
            LoadPage();

        }
        private async void LoadPage()
        {
            try
            {
                await LoadSummoner();
            } catch
            {
                Console.WriteLine("Hittade inte personen du sökte efter");
                return;
            }

            
            LoadImage();
        }
        private async Task LoadSummoner()
        {
            string[] splitName = _lolName.Split('#');
            if (splitName.Length != 2) return;
            string gameName = splitName[0];
            string tagLine = splitName[1];
            Summoner = await _lolRepository.GetSummonerAsync(gameName, tagLine);

        }
        private void LoadImage()
        {
            ProfileImage = ImageSource.FromFile($"ProfileIcons/{Summoner.ProfileIconId}.png");
        }
    }
}
