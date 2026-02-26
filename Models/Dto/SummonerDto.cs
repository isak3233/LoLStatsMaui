using System;
using System.Collections.Generic;
using System.Text;

namespace LoLStatsMaui.Models.Dto
{
    internal class SummonerDto
    {
        public int profileIconId { get; set; }
        public long revisionDate { get; set; }
        public string puuid { get; set; }
        public long summonerLevel { get; set; }
    }
}
