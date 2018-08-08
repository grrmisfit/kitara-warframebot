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
    public class PositionOnline
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
    }

    public class GetPlayerOnlineResult
    {
        public string Steamid { get; set; }


        public int Entityid { get; set; }


        public string Ip { get; set; }

        public string Name { get; set; }

        public bool Online { get; set; }

        public PositionOnline Position { get; set; }

        public int Experience { get; set; }

        public double Level { get; set; }

        public int Health { get; set; }

        public int Stamina { get; set; }


        public int Zombiekills { get; set; }


        public int Playerkills { get; set; }


        public int Playerdeaths { get; set; }


        public int Score { get; set; }


        public int Totalplaytime { get; set; }


        public string Lastonline { get; set; }


        public int Ping { get; set; }
    }
   
}
