using System;
using System.Collections.Generic;
using System.Text;

namespace LoLStatsMaui.Models.Requests
{
    public class MatchQueryRequest
    {
        public string Uuid { get; set; }
        public string Region { get; set; }
        public long? StartTime { get; set; }
        public long? EndTime { get; set; }
        public int? Queue { get; set; }
        public string? Type { get; set; }
        public int? Start { get; set; }
        public int Count { get; set; } = 20;
    }
}
