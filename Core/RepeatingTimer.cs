using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using warframebot.Core.UserAccounts;
using warframebot.Core;
using System.IO;

namespace warframebot.Core
{

    internal static class RepeatingTimer
    {
        private static string accountsFile = "players.json";
        private static Timer loopingTimer;
        private static SocketTextChannel channel;
        internal static Task StartTimer()
        {
            channel = Global.Client.GetGuild(293720753185226752).GetTextChannel(471312780079923210);

            loopingTimer = new Timer()
            {
                Interval = 5000,
                AutoReset = true,
                Enabled = true
            };
            loopingTimer.Elapsed += OnTimerTicked;


            return Task.CompletedTask;
        }
        
        public static void ThePlayerData()
        {
            
            
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("http://45.58.114.154:26916/api/getplayersonline?adminuser=" + Config.bot.webtoken + "&admintoken=" + Config.bot.webtokenpass);
            }
            if (json == "") return;
            if (json == "[]") return;
            
            JArray a = JArray.Parse(json);
            var onlinePlayers = a.ToObject<List<GetPlayerOnlineResult>>();
            string savedaccounts = File.ReadAllText("players.json");
            JArray b = JArray.Parse(savedaccounts);
            var theaccounts = b.ToObject<List<GetPlayerOnlineResult>>();
            List<GetPlayerOnlineResult> tmp = new List<GetPlayerOnlineResult>();
            foreach (GetPlayerOnlineResult onlinePlayer in onlinePlayers)
            {
                bool playerfound = false;

                foreach (GetPlayerOnlineResult theaccount in theaccounts)
                {
                    if (onlinePlayer.Steamid.Equals(theaccount.Steamid))
                    {
                        //player found so break second loop for next comparisson
                        playerfound = true;
                        break;
                    }
                }
                if (!playerfound)
                {
                    //player was in onlineList but not in savedlist
                    // do something
                    tmp.Add(onlinePlayer);
                }
            }
            foreach (GetPlayerOnlineResult res in tmp)
            {
                theaccounts.Add(res);
               DataStorage.SaveTmpAccounts(theaccounts,accountsFile);
            }



        }


                private static void OnTimerTicked(object sender, ElapsedEventArgs e)
                {

                    ThePlayerData();


                }
            }
}
