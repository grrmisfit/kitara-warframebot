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
            
            
            


        }


                private static void OnTimerTicked(object sender, ElapsedEventArgs e)
                {

                    


                }
            }
}
