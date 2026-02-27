using System;
using System.Collections.Generic;
using System.Text;
using LoLStatsMaui.Models.Dto;

namespace LoLStatsMaui.Models
{
    public class SummonerOverview
    {
        public string Uuid { get; set; }
        public string SummonerName { get; set; }
        public string TagLine { get; set; }
        public string Region { get; set; }
        public long Level { get; set; }
        public int ProfileIconId { get; set; }
        public List<RankEntry> RankEntries { get; set; }
    }
}
