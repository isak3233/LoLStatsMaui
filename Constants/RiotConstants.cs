using LoLStatsMaui.Models;
using LoLStatsMaui.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoLStatsMaui.Constants
{
    public static class RiotConstants
    {
        public static readonly Dictionary<string, string> QueueTypeMap = new()
        {
            { "RANKED_SOLO_5x5", "Ranked Solo/Duo" },
            { "RANKED_FLEX_SR", "Ranked Flex" }
        };

        public static readonly Dictionary<string, string> RegionMap = new()
        {
                { "euw1", "EUW" },
                { "eun1", "EUNE" },
                { "na1", "NA" },
                { "kr", "KR" },
                { "br1", "BR" },
                { "la1", "LAN" },
                { "la2", "LAS" },
                { "oc1", "OCE" },
                { "tr1", "TR" },
                { "ru", "RU" },
                { "jp1", "JP" }
        };
        public static readonly Dictionary<string, string> RegionToRoutingMap = new()
        {
            { "na1", "americas" },
            { "br1", "americas" },
            { "la1", "americas" },
            { "la2", "americas" },
            { "kr", "asia" },
            { "jp1", "asia" },
            { "eun1", "europe" },
            { "euw1", "europe" },
            { "me1", "europe" },
            { "tr1", "europe" },
            { "ru", "europe" },
            { "oc1", "sea" },
            { "sg2", "sea" },
            { "tw2", "sea" },
            { "vn2", "sea" }
        };
        public static string GetRouting(string region)
        {
            return RegionToRoutingMap.TryGetValue(region.ToLower(), out var routing) ? routing : "europe";
        }
        public static string GetRegion(string region)
        {
            return RegionMap.TryGetValue(region.ToLower(), out var readable) ? readable : region.ToUpper();
        }
        public static string GetQueueType(string queueType)
        {
            return QueueTypeMap.TryGetValue(queueType, out var readable) ? readable : queueType;
        }
        public static List<RankEntry> GetRankEntries(List<RankEntryDto> rankEntriesDto)
        {
            List<RankEntry> rankEntries = new List<RankEntry>
            {
                new RankEntry { Tier = "Unranked", QueueType = QueueTypeMap["RANKED_SOLO_5x5"], ImageString = "RankIcons/Rank=Unranked.png" },
                new RankEntry { Tier = "Unranked", QueueType = QueueTypeMap["RANKED_FLEX_SR"], ImageString = "RankIcons/Rank=Unranked.png" }
            };

            foreach (var rankEntryDto in rankEntriesDto)
            {
                int index = rankEntryDto.queueType == "RANKED_SOLO_5x5" ? 0 :
                            rankEntryDto.queueType == "RANKED_FLEX_SR" ? 1 : -1;

                if (index == -1) continue;

                int winRate = (int)Math.Round((double)rankEntryDto.wins / (rankEntryDto.wins + rankEntryDto.losses) * 100);
                string[] noRankTiers = { "MASTER", "GRANDMASTER", "CHALLENGER" };
                string rank = noRankTiers.Contains(rankEntryDto.tier) ? "" : rankEntryDto.rank;

                rankEntries[index] = new RankEntry
                {
                    QueueType = GetQueueType(rankEntryDto.queueType),
                    Tier = rankEntryDto.tier,
                    Rank = rank,
                    LeaguePoints = rankEntryDto.leaguePoints,
                    Wins = rankEntryDto.wins,
                    Losses = rankEntryDto.losses,
                    WinRate = winRate,
                    ImageString = $"RankIcons/Rank={rankEntryDto.tier}.png"
                };
            }
            return rankEntries;
        }


    }
}
