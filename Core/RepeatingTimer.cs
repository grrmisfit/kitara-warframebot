using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Newtonsoft.Json.Linq;
using Warframebot.Modules;
using Warframebot.Modules.Warframe;
using Warframebot.Core.UserAccounts;


namespace Warframebot.Core
{

    internal static class RepeatingTimer
    {
        
       private static Timer loopingTimer;
       private static SocketTextChannel channel;
        
       internal static Task StartTimer()
        {
            loopingTimer = new Timer()
            {
                Interval = 10000,
                AutoReset = true,
                Enabled = true
            };
            loopingTimer.Elapsed += OnTimerTicked;
            return Task.CompletedTask;
        }

        private static async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {

            await CheckGuildAlerts();
            await Task.Delay(1000);
            await CheckGuildFissures();
            await Task.Delay(1000);
            await CheckCetusTime();
            await Task.Delay(1000);
            await CheckAlarms();
        }

        private static async Task CheckAlertRewards(ulong id, ulong alertchan)
        {
          
            var warframe = Warframe.FromJson(Utilities.GetWarframeInfo());
            var checkAlerts = warframe.Alerts;
            var json = File.ReadAllText("SystemLang/WFsettings.json");
            var wfSettings = GuildAccounts.FromJson(json);
            
            int dacount = 0;
            string reward = "";

           
            foreach (Alert alert in checkAlerts)
            {
                if (dacount > wfSettings.Count - 1) break;
                var rewardcount= wfSettings[dacount].WantedRewards.Count;
                for (int i = 0; i < rewardcount; i++)
                reward = wfSettings[dacount].WantedRewards[i];
                var curreward = Utilities.ReplaceRewardInfo(alert.MissionInfo.MissionReward.Items[0]);
                curreward = curreward.ToLowerInvariant();
                if (curreward.Contains(reward))
                {
                    await Misc.SendMessageChannel(alertchan, Utilities.ReplaceRewardInfo(reward) + "Has been found, type !alerts to see which alert contains it");
                }

                dacount = dacount + 1;
            }
        }
        public static async Task CheckForAcolytes()

        {
            var warframe = Warframe.FromJson(Utilities.GetWarframeInfo());
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

        private static async Task  CheckFissures(ulong guildid )
        {
            var fisjson = string.Empty;
            fisjson = File.ReadAllText("SystemLang/WFsettings.json");
            if (string.IsNullOrEmpty(fisjson)) return;
            var guildAccounts = GuildAccounts.FromJson(fisjson);
           
            var warframe = Warframe.FromJson(Utilities.GetWarframeInfo());
            var checkFissAlerts = warframe.ActiveMissions;
        
            int dacount = 0;

            bool alerted = false; // initilaize so we can set it true/false later if the guild has been alerted
            var embed = new EmbedBuilder();
            var foundcount = 1; //variable to label each found fissure in numeric order
            bool fissureFound = false;

            foreach (GuildAccounts guild in guildAccounts) // cycle thru each guild
            {
                alerted = false;
                var curtime = DateTime.Now;
                var announcedtime = guild.TimeChecked;
                var checktime = curtime.Subtract(announcedtime).TotalMinutes;
                if (checktime < guild.AlertDelay) // check and see how long its been since last alerted ( in minutes )
                {
                    alerted = true;
                }
                for (int i = 0; i < checkFissAlerts.Count; i++)

                {
                    if (guild.WantedFissures.Count < 1)
                    {
                        return;
                    }

                    for (int i2 = 0; i2 < guild.WantedFissures.Count; i2++)
                    {
                        if (dacount > guildAccounts.Count ) break;
                    

                        var curFissure = Utilities.ReplaceInfo(checkFissAlerts[i].MissionType).ToLower();
                        var  wantedFissure = guild.WantedFissures[i2].ToLower();

                        if (curFissure != wantedFissure) continue;
                        
                       
                           embed.AddField( $"Found Fissure **{foundcount}**", $"**{curFissure}** on **{Utilities.ReplaceInfo(checkFissAlerts[i].Node)}** and **Relic** type is **{Utilities.ReplaceInfo(checkFissAlerts[i].Modifier)}**",true);
                        foundcount = foundcount + 1;
                        
                        fissureFound = true;
                    }
            }
                
                if (fissureFound)
                {
                    var chnl = Global.Client.GetChannel(guildAccounts[dacount].AlertsChannel) as IMessageChannel;
                    embed.WithTitle("**Fissure Alerts**");
                    embed.WithDescription("**These fissures matched wanted fissures**");
                    embed.WithColor(new Color(188, 66, 244));



                    if (alerted == false)
                    {
                        if (chnl != null) await chnl.SendMessageAsync("", false, embed.Build());
                        var thetime = DateTime.Now;
                        var accounts = UserAccounts.UserAccounts.GetAccount(guildid);
                        accounts.TimeChecked = thetime;
                        UserAccounts.UserAccounts.SaveAccounts();
                    }
                }
                dacount = dacount + 1;
            }
            
        }
        
        private static async Task CheckGuildAlerts()
        {

            var json = string.Empty;
            json = File.ReadAllText("SystemLang/WFsettings.json");
            if (string.IsNullOrEmpty(json)) return;
             var guildAccounts = GuildAccounts.FromJson(json);

            for (int i = 0; i < guildAccounts.Count; i++)
            {

                if (guildAccounts[i].AlertsChannel == 0) return;
                if (!guildAccounts[i].CheckAlerts == false)
                {
                    ulong tempguild = guildAccounts[i].Guild;
                    ulong tempchan = guildAccounts[i].AlertsChannel;
                    await CheckAlertRewards(tempguild, tempchan);
                }
            }
            
        }
        private static async Task CheckGuildFissures()
        {
            if (!File.Exists("SystemLang/WFsettings.json")) return;
            var json = File.ReadAllText("SystemLang/WFsettings.json");
            if (string.IsNullOrEmpty(json)) return;
            var guildAccounts = GuildAccounts.FromJson(json);

            for (int i = 0; i < guildAccounts.Count; i++)
            {

                if (guildAccounts[i].AlertsChannel == 0) return;
                if ((!guildAccounts[i].CheckFissures == false))
                {
                    await CheckFissures(guildAccounts[i].Guild);
                }
            }
        }

        private static async Task CheckAlarms()
        {
            var thetime = Utilities.GetCetusTime();
            if (string.IsNullOrEmpty(thetime)) return;
            long ignoreMe;
            if (!Int64.TryParse(thetime, out ignoreMe)) return;
            var timecheck = Int64.Parse(thetime);

            timecheck = timecheck - 50;
            var json = string.Empty;
            json = File.ReadAllText("SystemLang/Alarm.json");
            JArray a = JArray.Parse(json);
            List<UserAccount> tmpdata = new List<UserAccount>();
            var alarmUsers = a.ToObject<List<UserAccount>>();
            foreach (var users in alarmUsers)
            {
                if (users.AlarmOn)
                {
                    var curtime = DateTime.Now;
                    var announcedtime = users.TimeAlerted;
                    var checktime = curtime.Subtract(announcedtime).TotalMinutes;
                    if (checktime < 55) return; // check and see how long its been since last alerted ( in minutes )
                    if (timecheck <= users.AlarmDelay)
                    {
                        await Misc.SendMessageChannel(users.AlarmChannel,
                            $"<@{users.DiscordId}> this is your **{timecheck}** minute warning till Cetus nighttime!");
                        var theuser = UserAccounts.UserAccounts.GetAlarmUser(users.DiscordId, users.AlarmDelay);
                        var datime = DateTime.Now;
                        theuser.TimeAlerted = datime;
                        UserAccounts.UserAccounts.SaveAlarmUser();
                    }
                }
            }
            
        }

        private static async Task CheckCetusTime()
        {
            bool fifteentil = false;
            bool fivetil =false;
            var thetime = Utilities.GetCetusTime();
            if (string.IsNullOrEmpty(thetime)) return;
            long ignoreMe;
            if (!Int64.TryParse(thetime, out ignoreMe)) return;
            var timecheck = Int64.Parse(thetime);

            var json = string.Empty;
            json = File.ReadAllText("SystemLang/WFsettings.json");
            var guildAccounts =  GuildAccounts.FromJson(json);
            if (timecheck  > 70)
            {
                foreach (var accounts in guildAccounts)
                {
                    

                    if (accounts.CetusTimeAlerted)
                    {
                        var account = UserAccounts.UserAccounts.GetAccount(accounts.Guild);
                        account.CetusTimeAlerted = false;
                        UserAccounts.UserAccounts.SaveAccounts();
                    }
                }
            }
            if (timecheck > 67) return;
            if (timecheck <= 67 && timecheck > 64)
            {
                fifteentil = true;
            }

            if (timecheck  <= 55 && timecheck > 50)
            {
                fivetil = true;
            }

            if (fifteentil)
            {
                foreach (var accounts in guildAccounts)
                {
                    if (accounts.CetusTimeAlerted)
                    {
                        return;
                    }

                    if (accounts.CetusTime)
                    {
                        var timeleft = timecheck -50;
                        await Misc.SendMessageChannel(accounts.AlertsChannel, $"**It is approximately {timeleft} minutes till nighttime!**");
                        var account = UserAccounts.UserAccounts.GetAccount(accounts.Guild);
                        account.CetusTimeAlerted = true;
                        UserAccounts.UserAccounts.SaveAccounts();
                    }
                }
            }

            if (fivetil)
            {
                foreach (var accounts in guildAccounts)
                {
                    if (accounts.CetusTimeAlerted)
                    {
                        return;
                    }

                    if (accounts.CetusTime)
                    {
                        var timeleft = timecheck - 50;
                        await Misc.SendMessageChannel(accounts.AlertsChannel, $"**It is approximately {timeleft} minutes till nighttime!**");

                        var account = UserAccounts.UserAccounts.GetAccount(accounts.Guild);
                        account.CetusTimeAlerted = true;
                        UserAccounts.UserAccounts.SaveAccounts();
                       
                    }
                }
            }
            /* save this for later
            if (Utilities.CetusTimeCheck() == "nighttime")
            {
                foreach (var accounts in guildAccounts)
                {
                    if (accounts.CetusTimeAlerted)
                    {
                        return;
                    }

                    if (accounts.CetusTime)
                    {
                        var account = UserAccounts.UserAccounts.GetAccount(accounts.Guild);
                        account.CetusTimeAlerted = true;
                        UserAccounts.UserAccounts.SaveAccounts();
                        await Misc.SendMessageChannel(accounts.AlertsChannel, $"**It is now nighttime!**");

                    }
                }
            }*/
        }

    }
}