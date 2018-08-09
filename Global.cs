using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warframebot
{
    internal static class Global
    {
        internal static DiscordSocketClient Client { get; set; }
        internal static ulong MessageIdToTrack { get; set; }
    }
    public class SolNodeTmp
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class SortieData
    {
        public string Name { get; set; }
        public string Seed { get; set; }
        public string ModType { get; set; }
        public string ModDesc { get; set; }

    }
   
   
}
