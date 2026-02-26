
using LoLStatsMaui.Models;
using LoLStatsMaui.Models.Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using static System.Net.WebRequestMethods;


namespace LoLStatsMaui.Repositories
{
    internal class LolApiRepository : ILolRepository
    {
        private HttpClient _httpClient;

        public LolApiRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(AppConfig.Configuration["RiotApi:BaseUrl"]);
            _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", AppConfig.Configuration["RiotApi:ApiKey"]);
        }
        public async Task<Summoner> GetSummonerAsync(string gameName, string tagLine)
        {

            var accountResponse = await _httpClient.GetAsync($"riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            accountResponse.EnsureSuccessStatusCode();
            var accountJson = await accountResponse.Content.ReadAsStringAsync();
            var account = JsonSerializer.Deserialize<LolAccountDto>(accountJson);

            var accountRegionResponse = await _httpClient.GetAsync($"riot/account/v1/region/by-game/lol/by-puuid/{account.puuid}");
            accountRegionResponse.EnsureSuccessStatusCode();
            var accountRegionJson = await accountRegionResponse.Content.ReadAsStringAsync();
            var accountRegion = JsonSerializer.Deserialize<AccountRegionDto>(accountRegionJson);

            var summonerResponse = await _httpClient.GetAsync($"https://{accountRegion.region}.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{account.puuid}");
            summonerResponse.EnsureSuccessStatusCode();
            var summonerJson = await summonerResponse.Content.ReadAsStringAsync();
            var summonerData = JsonSerializer.Deserialize<SummonerDto>(summonerJson); 
            return new Summoner
            {
                Uuid = account.puuid,
                SummonerName = account.gameName,
                TagLine = account.tagLine,
                Level = summonerData.summonerLevel,
                ProfileIconId = summonerData.profileIconId,
                Region = accountRegion.region
            };
        }
    }
}
