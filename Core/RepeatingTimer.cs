using Discord.WebSocket;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Warframebot.Modules;
using Warframebot.Modules.Warframe;
using Warframebot.Core.UserAccounts;

namespace Warframebot.Core
{

    internal static class RepeatingTimer
    {
        //private static string accountsFile = "players.json";
        private static Timer loopingTimer;
        public static Timer scramTimer;
        private static Timer delayTimer;
        private static SocketTextChannel channel;

        internal static Task StartScramTimer()
        {
            
            if (ScramData.GameStarted == false) return Task.CompletedTask;
            if(ScramData.GamePause == true) return Task.CompletedTask;
            scramTimer = new Timer()
            {
                Interval = 10000,
                AutoReset = true,
                Enabled = true,
            };
           
            
            scramTimer.Elapsed += OnScramTimerTicked;
           
            return Task.CompletedTask;
        }

        private async static void OnScramTimerTicked(object sender, ElapsedEventArgs e)
        {
            if (ScramData.GameWait == true) return;
            if (ScramData.GameStarted == false) return;
            if (ScramData.GamePause == true) return;
            Console.WriteLine(DateTime.Today);
            if (ScramData.WordGuessed == true)
            {
                string daword = Scramble.GetScramWord();
                string cheatword = ScramData.ScramWord;
               // await Misc.SendMessageChannel(ScramData.ScramChannel, "**" + cheatword + "**");
                // await Misc.SendMessageChannel(471312780079923210, "**" + daword + "**");
                var embed = new EmbedBuilder();
                embed.WithTitle("Scrambled Word");
                embed.WithDescription("Category: **" + ScramData.Category + "**");
                embed.AddField("**Word***: **", daword + "**", true);
                embed.WithColor(new Color(188, 66, 244));
                var chnl = Global.Client.GetChannel(ScramData.ScramChannel) as IMessageChannel; // 4
                await chnl.SendMessageAsync("", false, embed.Build());
                ScramData.WordGuessed = false;
            }
            else
            {
                if (ScramData.GameWait == true) return;
                if (ScramData.GameStarted == false) return;
                string cheatword = ScramData.ScramWord;
                //await Misc.SendMessageChannel(ScramData.ScramChannel, "**" + cheatword + "**");
                await Misc.SendMessageChannel(ScramData.ScramChannel, "**Current word: " +  ScramData.ScrambledWord + "**");
            }

        }
        internal static Task DelayScramTimer()
        {
            if (ScramData.GameStarted == false) return Task.CompletedTask;
            if (ScramData.GamePause == false) return Task.CompletedTask;
            delayTimer = new Timer()
            {
                Interval = 10000,
                AutoReset = false,
                Enabled = true,
            };

            delayTimer.Elapsed += OnDelayTimerTicked;
            return Task.CompletedTask;
        }

        private async static void OnDelayTimerTicked(object sender, ElapsedEventArgs e)
        {
            if (ScramData.GameStarted == false) return;
            await Misc.SendMessageChannel(ScramData.ScramChannel, "**" + "New word coming in 10 secs!" + "**");
            delayTimer.Enabled = false;
            ScramData.GamePause = false;
            ScramData.GameWait = false;
            scramTimer.Enabled = true;
        }
        internal static Task StartTimer()
        {
             loopingTimer = new Timer()
            {
                Interval = 60000,
                AutoReset = true,
                Enabled = true
            };
            loopingTimer.Elapsed += OnTimerTicked;
            return Task.CompletedTask;
        }
        public async  static Task CheckAlertRewards()
        {
            if (WFSettings.CheckAlerts == false) return;
            var warframe = Warframe.FromJson(Utilities.GetWarframeInfo());
            var checkAlerts = warframe.Alerts;
            int rewardcount = WFSettings.WantedRewards.Length;
            string reward = WFSettings.WantedRewards[0];
            string curreward = "";
            if (reward == null) return;
            if (WFSettings.AlertsChannel == 0) return;
            foreach (Alert alert in checkAlerts)
            {
                for (int i = 0; i < rewardcount; i++)
                    reward = WFSettings.WantedRewards[i];
                    curreward = Utilities.ReplaceInfo2(alert.MissionInfo.MissionReward.Items[0]);
                if (curreward == null) return;
              if (Utilities.ReplaceInfo2(alert.MissionInfo.MissionReward.Items[0]) == reward)
                {
                    await Misc.SendMessageChannel(WFSettings.AlertsChannel, Utilities.ReplaceInfo2(reward) + "Has been found, type !alerts to see which alert contains it");
                }
            }
        }
        public async static Task  CheckForAcolytes()

        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse = "";
            using (WebClient client = new WebClient())
            apiresponse = client.DownloadString(url);

            var warframe = Warframe.FromJson(apiresponse);
            var activeAcolytes = warframe.PersistentEnemies;

            for (int i = 0; i < activeAcolytes.Count; i++)
            {
                ulong id = 471312780079923210;
                if (activeAcolytes[i].AgentType == "") break;
                string acolytename = warframe.PersistentEnemies[i].LastDiscoveredLocation;
                if (String.IsNullOrEmpty(acolytename)) return;
                {
                    await Misc.SendMessageChannel(id, "Acolyte Found!");
                    break;

                }
            }
        }

        private async static Task CheckFissures()
        {
            var json = File.ReadAllText("SystemLang/WFsettings.json");
            var guildAccounts = GuildAccounts.FromJson(json);

            for (int guildcheck = 0; guildcheck  < guildAccounts.Count; guildcheck++)
            {
                if (guildAccounts[guildcheck].CheckAlerts == false) return;
                if (guildAccounts[guildcheck].AlertsChannel == 0) return;

                var warframe = Warframe.FromJson(Utilities.GetWarframeInfo());
                var checkFissAlerts = warframe.ActiveMissions;
                int fissCount = WFSettings.WantedFissures.Length;
                string wantedFissure = WFSettings.WantedFissures[0];
                string curFissure = "";

                int dacount = 0;
                if (wantedFissure == null) return;
                if (WFSettings.AlertsChannel == 0) return;




                foreach (ActiveMission fissure in checkFissAlerts)
                {
                    for (int i = 0; i < fissCount; i++)


                        curFissure = Utilities.ReplaceInfo(checkFissAlerts[i].MissionType);
                    if (curFissure == null) return;
                    wantedFissure = WFSettings.WantedFissures[dacount];
                    if (curFissure == wantedFissure)

                    {
                        await Misc.SendMessageChannel(WFSettings.AlertsChannel,
                            curFissure +
                            $"Has been found, on {Utilities.ReplaceInfo(checkFissAlerts[dacount].Node)} and Relic type is {Utilities.ReplaceInfo(checkFissAlerts[dacount].Modifier)}");
                    }

                    dacount = dacount + 1;

                }
            }
        }

        private async static void OnTimerTicked(object sender, ElapsedEventArgs e)
                {

                    await   CheckAlertRewards();
                    await Task.Delay(1);
                    await CheckFissures();



                }
            }
}
