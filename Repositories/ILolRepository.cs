using LoLStatsMaui.Models;
using LoLStatsMaui.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
namespace LoLStatsMaui.Repositories
{
    public interface ILolRepository
    {
        Task<SummonerOverview> GetSummonerOverviewAsync(string gameName, string tagLine);
        Task<List<LolMatch>> GetLolMatchesAsync(MatchQueryRequest request);

    }
}
