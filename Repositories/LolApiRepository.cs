
using LoLStatsMaui.Constants;
using LoLStatsMaui.Exceptions;
using LoLStatsMaui.Models;
using LoLStatsMaui.Models.Dto;
using LoLStatsMaui.Models.Requests;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Timers;
using static Microsoft.Maui.ApplicationModel.Permissions;
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
        public async Task<SummonerOverview> GetSummonerOverviewAsync(string gameName, string tagLine)
        {

            var account = await GetAsync<LolAccountDto>($"riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}");
            var accountRegion = await GetAsync<AccountRegionDto>($"riot/account/v1/region/by-game/lol/by-puuid/{account.puuid}");

            var summonerDataTask = GetAsync<SummonerDto>($"https://{accountRegion.region}.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{account.puuid}");
            var rankEntriesTask = GetAsync<List<RankEntryDto>>($"https://{accountRegion.region}.api.riotgames.com/lol/league/v4/entries/by-puuid/{account.puuid}");

            await Task.WhenAll(summonerDataTask, rankEntriesTask);

            var summonerData = await summonerDataTask;
            var rankEntries = await rankEntriesTask;
            return new SummonerOverview
            {
                Uuid = account.puuid,
                SummonerName = account.gameName,
                TagLine = account.tagLine,
                Level = summonerData.summonerLevel,
                ProfileIconId = summonerData.profileIconId,
                Region = RiotConstants.GetRegion(accountRegion.region),
                RankEntries = RiotConstants.GetRankEntries(rankEntries),

            };
        }
        public async Task<List<LolMatch>> GetLolMatchesAsync(MatchQueryRequest request)
        {
            string routing = RiotConstants.GetRouting(request.Region);
            string? startTime = request.StartTime == null ? "" : "startTime=" + request.StartTime.ToString() + "&";
            string? endTime = request.EndTime == null ? "" : "endTime=" + request.EndTime.ToString() + "&";
            string? queue = request.Queue == null ? "" : "queue=" + request.Queue.ToString() + "&";
            string? type = request.Type == null ? "" : "type=" + request.Type.ToString() + "&";
            string? start = request.Start == null ? "" : "start=" + request.Start.ToString() + "&";
            string count = "count=" + request.Count.ToString();

            string url = $"https://{routing}.api.riotgames.com/lol/match/v5/matches/by-puuid/{request.Uuid}/ids?{startTime}{endTime}{queue}{type}{start}{count}";
            var matchIds = await GetAsync<List<string>>(url);


            List<Task<LolMatchDto>> matchDtoTasks = new List<Task<LolMatchDto>>();
            foreach (var matchId in matchIds)
            {
                matchDtoTasks.Add(GetAsync<LolMatchDto>($"https://{routing}.api.riotgames.com/lol/match/v5/matches/{matchId}"));
            }
            LolMatchDto[] matchesDto = await Task.WhenAll(matchDtoTasks);

            List<LolMatch> matches = new List<LolMatch>();
            foreach(var match in matchesDto)
            {
                matches.Add(MatchMapper.Map(match, request.Uuid));
            }
            return matches;
        }


        private async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            var errorJson = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(response.StatusCode);
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    throw new NotFoundException(errorJson);
                case System.Net.HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(errorJson);
                case System.Net.HttpStatusCode.InternalServerError:
                    throw new ServerException(errorJson);
                default:
                    response.EnsureSuccessStatusCode();
                    break;
            }
            int statusCode = (int)response.StatusCode;
            if(statusCode >= 500)
            {
                throw new ServerException(errorJson);
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
