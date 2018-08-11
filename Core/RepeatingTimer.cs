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
using warframebot.Modules.Warframe;
using warframebot.Modules;

namespace warframebot.Core
{

    internal static class RepeatingTimer
    {
        //private static string accountsFile = "players.json";
        private static Timer loopingTimer;
        private static SocketTextChannel channel;
        internal static Task StartTimer()
        {
            channel = Global.Client.GetGuild(293720753185226752).GetTextChannel(471312780079923210);

            loopingTimer = new Timer()
            {
                Interval = 60000,
                AutoReset = true,
                Enabled = true
            };
            loopingTimer.Elapsed += OnTimerTicked;


            return Task.CompletedTask;
        }

        public async static Task  CheckForAcolytes()
        {


            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse = "";
            //apiClient.Encoding = Encoding.UTF8;

            using (WebClient client = new WebClient())

                apiresponse = client.DownloadString(url);


            var warframe = Warframe.FromJson(apiresponse); //Warframe.Warframe.FromJson(apiresponse);

            var activeAcolytes = warframe.PersistentEnemies;

            for (int i = 0; i < activeAcolytes.Count; i++)
            {
                ulong id = 471312780079923210;
                if (activeAcolytes[i].AgentType == "") break;

                if (warframe.PersistentEnemies[i].Discovered == true)
                {
                    await Misc.SendMessageChannel(id, "Acolyte Found!");
                    break;

                }
            }
        }

                private static void OnTimerTicked(object sender, ElapsedEventArgs e)
                {

                         CheckForAcolytes();


                }
            }
}
