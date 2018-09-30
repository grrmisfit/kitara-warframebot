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
            mystart:
            try
            {
                await CheckGuildAlerts();
                await Task.Delay(1000);
                 CheckGuildFissures();
                await Task.Delay(1000);
                await CheckCetusTime();
                await Task.Delay(1000);
                await CheckAlarms();
                await Task.Delay(1000);
                await CheckInvasionRewards();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                goto mystart;
            }
            
        }

        private static async Task CheckAlertRewards(ulong id, ulong alertchan)
        {

            var json = Utilities.GetWarframeInfo();
            if (string.IsNullOrEmpty(json)) return;
            var warframe = Warframe.FromJson(json);
            var checkAlerts = warframe.Alerts;
            var wfset = Utilities.GetWfSettings();
            var wfSettings = GuildAccounts.FromJson(wfset);
            List<string> alertList = new List<string>();
            int dacount = 0;
            string reward = "";
            var embed = new EmbedBuilder();

            foreach (Alert alert in checkAlerts)
            {
                
                foreach (var account in wfSettings)
                {
                    for (int i = 0; i < account.WantedRewards.Count; i++)

                    {
                        reward = account.WantedRewards[i];
                        if (reward == "") continue;
                        if (alert.MissionInfo.MissionReward.Items == null) continue;
                        var curreward = Utilities.ReplaceRewardInfo(alert.MissionInfo.MissionReward.Items[0]).ToLower();
                        var curtime = DateTime.Now;
                        var announcedtime = account.AlertTimeChecked;
                        var checktime = curtime.Subtract(announcedtime).TotalMinutes;

                        //if (checktime < account.AlertDelay) continue;
                        if (curreward.Contains(reward))
                        {

                            if (account.KnownAlerts.Contains(alert.Id.Oid))
                            {
                                goto nextalert;

                            }
                            
                            
                            alertList.Add(alert.Id.Oid);
                            embed.WithTitle($"{curreward} has been found!");
                            embed.AddField($"Location info:\n",
                                $"{Utilities.ReplaceInfo(alert.MissionInfo.Location)}\n {Utilities.ReplaceInfo(alert.MissionInfo.MissionType)}\n {Utilities.ReplaceInfo(alert.MissionInfo.Faction)}");
                            embed.AddField("Expires: ", $"{Utilities.ExpireFisTime(alert.Expiry.Date.NumberLong)}");
                            if (Global.Client.GetChannel(account.AlertsChannel) is IMessageChannel chnl) await chnl.SendMessageAsync("", false, embed.Build());
                            // curreward + " Has been found, type !alerts to see which alert contains it");
                            var thetime = DateTime.Now;
                            var accounts = UserAccounts.UserAccounts.GetAccount(account.Guild);
                            accounts.AlertTimeChecked = thetime;
                            accounts.KnownAlerts.Add(alert.Id.Oid);
                            UserAccounts.UserAccounts.SaveAccounts();
                            nextalert:;
                        }

                        dacount = dacount + 1;
                    }
                }
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

        private static async Task CheckFissures()
        {
            var fisjson = string.Empty;
            fisjson = Utilities.GetWfSettings();
            if (string.IsNullOrEmpty(fisjson)) return;
            var guildAccounts = GuildAccounts.FromJson(fisjson);
            
            var json =Utilities.GetWarframeInfo();
            if (string.IsNullOrEmpty(json)) return;
            
            var warframe = Warframe.FromJson(json);
            
            var checkFissAlerts = warframe.ActiveMissions;
        
            int dacount = 0;


            List<string> fisList = new List<string>();
            foreach (GuildAccounts guild in guildAccounts) // cycle thru each guild
            {
                var knownFis = guild.KnownFissures;
                var foundcount = 1; //variable to label each found fissure in numeric order
                var embed = new EmbedBuilder();
                var lastmissioniD = "";
                var fissureFound = false;
                var alerted = false; // initilaize so we can set it true/false later if the guild has been alerted
                var curtime = DateTime.Now;
                var announcedtime = guild.TimeChecked;
                var checktime = curtime.Subtract(announcedtime).TotalMinutes;
                if (checktime < guild.AlertDelay) // check and see how long its been since last alerted ( in minutes )
                {
                    alerted = true;
                }
                foreach (var t in checkFissAlerts)
                {
                    if (guild.WantedFissures.Count < 1)
                    {
                        break;
                    }
                    
                    foreach (var wantedfis in guild.WantedFissures)
                    {
                       var curFissure = Utilities.ReplaceInfo(t.MissionType).ToLower();
                        var  wantedFissure = wantedfis.ToLower();
                        
                        if (curFissure == wantedFissure)
                        {
                            foreach (var t1 in guild.KnownFissures)
                            {
                                if (t1 == t.Id.Oid)
                                {
                                    goto thenext;
                                }
                            }
                            if (t.Id.Oid == lastmissioniD) continue;
                            var exptime = Utilities.ExpireFisTime(t.Expiry.Date.NumberLong);
                            embed.WithDescription($"{Utilities.FissureLink(curFissure)}");
                            embed.AddField($"Found Fissure **{foundcount}**",
                                $"**{Utilities.ReplaceInfo(t.Node)}** | **Relic** **{Utilities.ReplaceInfo(t.Modifier)}** \n Expires in {exptime}",
                                true);
                            foundcount = foundcount + 1;
                            fisList.Add(t.Id.Oid);
                             lastmissioniD = t.Id.Oid;
                            fissureFound = true;
                            thenext:;
                        }
                    }
                }

                if (fissureFound)
                {
                    
                    var chnl = Global.Client.GetChannel(guild.AlertsChannel) as IMessageChannel;
                    embed.WithTitle("**Fissure Alerts**");
                    embed.WithFooter("", "https://n9e5v4d8.ssl.hwcdn.net/images/headerLogo.png");
                    embed.WithColor(new Color(188, 66, 244));
                    //embed.WithThumbnailUrl("http://3rdshifters.org/voidtear.png");


                    if (alerted == false)
                    {
                        
                        if (chnl != null) await chnl.SendMessageAsync("", false, embed.Build());
                        var thetime = DateTime.Now;
                        var accounts = UserAccounts.UserAccounts.GetAccount(guild.Guild);
                        accounts.TimeChecked = thetime;
                        foreach (var t in fisList)
                        {
                            accounts.KnownFissures.Add(t);
                        }
                        
                        UserAccounts.UserAccounts.SaveAccounts();
                    }
                }
                dacount = dacount + 1;
            }

        }

        private static async Task CheckGuildAlerts()
        {

            var json = string.Empty;
            json = Utilities.GetWfSettings();
            if (string.IsNullOrEmpty(json)) {return;}
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
        private static  async Task CheckGuildFissures()
        {
            if (!File.Exists("SystemLang/WFsettings.json")) return;
            var json = Utilities.GetWfSettings();
            if (string.IsNullOrEmpty(json)) return;
            var guildAccounts = GuildAccounts.FromJson(json);

            foreach (var t in guildAccounts)
            {
                if (t.AlertsChannel == 0) return;
                if ((!t.CheckFissures == false))
                {
                   await CheckFissures();
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
            var alarmUsers = a.ToObject<List<UserAccount>>();
            for (int i = 0; i < alarmUsers.Count; i++)
             {
                if (alarmUsers[i].AlarmOn)
                {
                    var curtime = DateTime.Now;
                    var announcedtime = alarmUsers[i].TimeAlerted;
                    var checktime = curtime.Subtract(announcedtime).TotalMinutes;
                    if (timecheck < 0) return;
                    if (checktime < 55) continue; // check and see how long its been since last alerted ( in minutes )
                    if (timecheck <= alarmUsers[i].AlarmDelay)
                    {
                        await Misc.SendMessageChannel(alarmUsers[i].AlarmChannel,
                            $"<@{alarmUsers[i].DiscordId}> this is your **{timecheck}** minute warning till Cetus nighttime!");
                        var theuser = UserAccounts.UserAccounts.GetAlarmUser(alarmUsers[i].DiscordId, alarmUsers[i].AlarmDelay);
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
            json = Utilities.GetWfSettings();
            var guildAccounts =  GuildAccounts.FromJson(json);
            if (timecheck  > 70)
            {
                foreach (var accounts in guildAccounts)
                {
                    

                    if (accounts.Cetus15TimeAlerted)
                    {
                        var account = UserAccounts.UserAccounts.GetAccount(accounts.Guild);
                        account.Cetus15TimeAlerted = false;
                        account.Cetus5TimeAlerted = false;
                        UserAccounts.UserAccounts.SaveAccounts();
                    }
                }
            }
            if (timecheck > 67) return;
            if (timecheck <= 65 && timecheck > 64)
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
                    if (accounts.Cetus15TimeAlerted)
                    {
                        return;
                    }

                    if (accounts.CetusTime)
                    {
                        var timeleft = timecheck -50;
                        await Misc.SendMessageChannel(accounts.AlertsChannel, $"**It is approximately {timeleft} minutes till nighttime!**");
                        var account = UserAccounts.UserAccounts.GetAccount(accounts.Guild);
                        account.Cetus15TimeAlerted = true;
                        UserAccounts.UserAccounts.SaveAccounts();
                    }
                }
            }

            if (fivetil)
            {
                foreach (var accounts in guildAccounts)
                {
                    if (accounts.Cetus5TimeAlerted)
                    {
                        return;
                    }

                    if (accounts.CetusTime)
                    {
                        var timeleft = timecheck - 50;
                        await Misc.SendMessageChannel(accounts.AlertsChannel, $"**It is approximately {timeleft} minutes till nighttime!**");

                        var account = UserAccounts.UserAccounts.GetAccount(accounts.Guild);
                        account.Cetus5TimeAlerted = true;
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

        private static async Task CheckInvasionRewards()
        {
            bool foundrewards = false;
            var apiresponse = Utilities.GetWarframeInfo();
            if (string.IsNullOrEmpty(apiresponse)) return;
            var invasions = Warframe.FromJson(apiresponse);
            var invasionRewards = invasions.Invasions;
            var json = Utilities.GetWfSettings();
            var wfSettings = GuildAccounts.FromJson(json);
            var embed = new EmbedBuilder();
            string atkReward = "";
            string defReward = "";
            bool alerted = false;
            foreach (var reward in invasionRewards)
            {
                if (reward.Completed) return;
                if (reward.AttackerReward.AnythingArray == null)
                {
                    atkReward = "Nothing";
                }
                else if(reward.AttackerReward.ErReward == null)
                {
                    atkReward = "Nothing";
                }
                else
                {
                    atkReward = Utilities.ReplaceRewardInfo(reward.AttackerReward.ErReward.CountedItems[0].ItemType);
                    
                }

                if (reward.AttackerReward.AnythingArray == null)
                {
                    defReward = Utilities.ReplaceRewardInfo(reward.DefenderReward.CountedItems[0].ItemType);
                }
                
                foreach (GuildAccounts guild in wfSettings)
                {
                    
                    var curtime = DateTime.Now;
                    var announcedtime = guild.TimeChecked;
                    var checktime = curtime.Subtract(announcedtime).TotalMinutes;
                    if (checktime < guild.AlertDelay) // check and see how long its been since last alerted ( in minutes )
                    {
                        alerted = true;
                    }
                    if (guild.WantedRewards.Contains(atkReward))
                    {

                        embed.AddField($"{atkReward} Found!",
                            $"On {Utilities.ReplaceInfo(reward.Node)} Faction: {reward.AttackerMissionInfo.Faction} vs {reward.DefenderMissionInfo.Faction}");
                        foundrewards = true;
                    }

                    if (guild.WantedRewards.Contains(defReward))
                    {
                        embed.AddField($"{defReward} Found!",
                            $"On {Utilities.ReplaceInfo(reward.Node)} Faction: {reward.DefenderMissionInfo.Faction} vs {reward.AttackerMissionInfo.Faction}");
                        foundrewards = true;
                    }
                    if (alerted == false)
                    {
                       
                       
                        var thetime = DateTime.Now;
                        var accounts = UserAccounts.UserAccounts.GetAccount(guild.Guild);
                        accounts.TimeChecked = thetime;
                        UserAccounts.UserAccounts.SaveAccounts();
                    }
                    if ((foundrewards) && (alerted == false))
                    {

                        embed.WithTitle("Rewards Alert");
                        embed.WithDescription("Rewards wanted were found in the following invasions.");
                        var chnl = Global.Client.GetChannel(guild.AlertsChannel) as IMessageChannel;
                        if (chnl != null) await chnl.SendMessageAsync("", false, embed.Build());
                    }
                }

            }

            await Task.Delay(1);
        }
    }
}