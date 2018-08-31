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
            if (ScramData.GamePause == true) return Task.CompletedTask;
            scramTimer = new Timer()
            {
                Interval = 10000,
                AutoReset = true,
                Enabled = true,
            };


            scramTimer.Elapsed += OnScramTimerTicked;

            return Task.CompletedTask;
        }

        private static async void OnScramTimerTicked(object sender, ElapsedEventArgs e)
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
                await Misc.SendMessageChannel(ScramData.ScramChannel, "**Current word: " + ScramData.ScrambledWord + "**");
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

        private static async void OnDelayTimerTicked(object sender, ElapsedEventArgs e)
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
        public static async void CheckAlertRewards(ulong id, ulong alertchan)
        {
          //  if (WFSettings.CheckAlerts == false) return;
            var warframe = Warframe.FromJson(Utilities.GetWarframeInfo());
            var checkAlerts = warframe.Alerts;
            var json = File.ReadAllText("SystemLang/WFsettings.json");
            var wfSettings = GuildAccounts.FromJson(json);
            int rewardcount = 0;
            int dacount = 0;
            string curreward = "";
            string reward = "";

           //// if (reward == null) return;
           // if (alertchan == 0) return;
            foreach (Alert alert in checkAlerts)
            {
                if (dacount > wfSettings.Count - 1) break;
               rewardcount= wfSettings[dacount].WantedRewards.Count;
                for (int i = 0; i < rewardcount; i++)
                   
                // if(wfSettings[i].WantedRewards[i] == "");
                reward = wfSettings[dacount].WantedRewards[i];
                curreward = Utilities.ReplaceInfo2(alert.MissionInfo.MissionReward.Items[0]);
                //   if (curreward == null) return;
                curreward = curreward.ToLowerInvariant();
                if (curreward.Contains(reward))
                {
                    await Misc.SendMessageChannel(id, Utilities.ReplaceInfo2(reward) + "Has been found, type !alerts to see which alert contains it");
                }

                dacount = dacount + 1;
            }
        }
        public static async Task CheckForAcolytes()

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

        private static async void  CheckFissures(ulong guildid )
        {
            var json = File.ReadAllText("SystemLang/WFsettings.json");
            var guildAccounts = GuildAccounts.FromJson(json);
           
            //for (int guildcheck = 0; guildcheck  < guildAccounts.Count; guildcheck++)


            var warframe = Warframe.FromJson(Utilities.GetWarframeInfo());
            var checkFissAlerts = warframe.ActiveMissions;
            int fissCount = 0; // WFSettings.WantedFissures.Length;
            string wantedFissure = "";//WFSettings.WantedFissures[0];
            string curFissure = "";

            int dacount = 0;
           // if (wantedFissure == null) return;
           // if (WFSettings.AlertsChannel == 0) return;




            foreach (GuildAccounts fissure in guildAccounts)
            {
                
                fissCount = fissure.WantedFissures.Count;

                for (int i = 0; i < checkFissAlerts.Count; i++)

                {
                    for (int i2 = 0; i2 < fissure.WantedFissures.Count; i2++)
                    {
                        if (dacount > guildAccounts.Count - 1) break;
                    

                    curFissure = Utilities.ReplaceInfo(checkFissAlerts[i].MissionType).ToLower();
                    if (curFissure == null) return;
                    wantedFissure = fissure.WantedFissures[i2].ToLower();
                        if (curFissure != wantedFissure) continue;
                        var curtime = DateTime.Now;
                        var announcetime = guildAccounts[dacount].TimeChecked;
                        var checktime = curtime.Subtract(announcetime).Minutes;
                        //     if (checktime < 5) return;
                        await Misc.SendMessageChannel(guildAccounts[dacount].AlertsChannel,
                            "A **"+ curFissure +
                            $"** type, has been found, on **{Utilities.ReplaceInfo(checkFissAlerts[i].Node)}** and **Relic** type is **{Utilities.ReplaceInfo(checkFissAlerts[i].Modifier)}**");
                        var thetime = DateTime.Now;
                        var accounts = UserAccounts.UserAccounts.GetAccount(guildid);
                        accounts.TimeChecked = thetime;
                        //guildAccounts[dacount].TimeChecked = thetime;
                        UserAccounts.UserAccounts.SaveAccounts();

                    }
            }
                dacount = dacount + 1;
            }
        }


        private static async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {

             CheckGuildAlerts();
            await Task.Delay(1);
             CheckGuildFissures();



        }
        private static async void CheckGuildAlerts()
        {
            var json = File.ReadAllText("SystemLang/WFsettings.json");
            var guildAccounts = GuildAccounts.FromJson(json);

            for (int i = 0; i < guildAccounts.Count; i++)
            {

                if (guildAccounts[i].AlertsChannel == 0) return;
                if (guildAccounts[i].CheckAlerts == true)
                {
                    ulong tempguild = guildAccounts[i].Guild;
                    ulong tempchan = guildAccounts[i].AlertsChannel;
                     CheckAlertRewards(tempguild, tempchan);
                }
            }
            
        }
        private static  void CheckGuildFissures()
        {
            if (!File.Exists("SystemLang/WFsettings.json")) return;
            var json = File.ReadAllText("SystemLang/WFsettings.json");
            var guildAccounts = GuildAccounts.FromJson(json);

            for (int i = 0; i < guildAccounts.Count; i++)
            {

                if (guildAccounts[i].AlertsChannel == 0) return;
                if (guildAccounts[i].CheckFissures == true)
                {
                     CheckFissures(guildAccounts[i].Guild);
                }
            }
        }

    }
}