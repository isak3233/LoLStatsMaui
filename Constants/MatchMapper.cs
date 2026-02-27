using LoLStatsMaui.Models;
using LoLStatsMaui.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoLStatsMaui.Constants
{
    public static class MatchMapper
    {
        public static LolMatch Map(LolMatchDto dto, string puuid)
        {
            var participant = dto.Info.Participants.First(p => p.Puuid == puuid);
            return new LolMatch
            {
                Win = participant.Win,
                ChampLevel = participant.ChampLevel,
            };
        }
    }
}
