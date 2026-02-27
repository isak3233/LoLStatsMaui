using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoLStatsMaui.Exceptions;
using LoLStatsMaui.Models;
using LoLStatsMaui.Models.Requests;
using LoLStatsMaui.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private SummonerOverview _summonerOverview;

        [ObservableProperty]
        private ObservableCollection<LolMatch> _matchList = new();

        [ObservableProperty]
        private ImageSource _profileImage;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _hasError;

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
                await LoadSummonerOverview();
            }
            catch (NotFoundException e)
            {
                ErrorMessage = "Kontot hittades inte";
                Debug.WriteLine(e);
                return;
            }
            catch (UnauthorizedException e)
            {
                ErrorMessage = "Otillåten API nyckel";
                Debug.WriteLine(e);
                return;
            }
            catch (ServerException e)
            {
                ErrorMessage = "Riots API har just nu problem";
                Debug.WriteLine(e);
                return;
            }
            catch (Exception e)
            {
                ErrorMessage = "Något gick fel!";
                Debug.WriteLine(e);
                return;
            } finally
            {
                HasError = true;
            }
            HasError = false;
            LoadImage();
            await LoadMatches();
        }
        
        private async Task LoadSummonerOverview()
        {
            string[] splitName = _lolName.Split('#');
            if (splitName.Length != 2) return;
            string gameName = splitName[0];
            string tagLine = splitName[1];
            SummonerOverview = await _lolRepository.GetSummonerOverviewAsync(gameName, tagLine);

        }
        private void LoadImage()
        {
            ProfileImage = ImageSource.FromFile($"ProfileIcons/{SummonerOverview.ProfileIconId}.png");
        }
        private async Task LoadMatches()
        {
            var request = new MatchQueryRequest
            {
                Uuid = SummonerOverview.Uuid,
                Region = SummonerOverview.Region,
                Count = 5,
            };
            var matches = await _lolRepository.GetLolMatchesAsync(request);
            MatchList = new ObservableCollection<LolMatch>(matches);
        }
    }
}
