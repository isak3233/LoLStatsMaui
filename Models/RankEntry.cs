using System;
using System.Collections.Generic;
using System.Text;

namespace LoLStatsMaui.Models
{
    public class RankEntry
    {
        public string QueueType { get; set; }

        public string Tier { get; set; }
        public string Rank { get; set; }
        public int LeaguePoints { get; set; }
        public int WinRate { get; set; }
        public int Wins { get; set; }
        public int Losses {  get; set; }
        public string ImageString { get; set; }
    }
}
