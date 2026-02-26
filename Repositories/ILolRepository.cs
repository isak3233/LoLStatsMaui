using System;
using System.Collections.Generic;
using System.Text;
using LoLStatsMaui.Models;
namespace LoLStatsMaui.Repositories
{
    public interface ILolRepository
    {
        Task<Summoner> GetSummonerAsync(string gameName, string tagLine);

    }
}
