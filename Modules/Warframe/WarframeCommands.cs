﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Warframebot.Core.UserAccounts;
using Warframebot.Modules.Market;

namespace Warframebot.Modules.Warframe
{
    public class WarframeCommands :  InteractiveBase
    {
        [Command("markets")]
        public async Task MarketCommand([Remainder] string msg)
        {
            var json = File.ReadAllText("SystemLang/Items.json");
            var market = Items.FromJson(json);
            var items = market.Payload.Items.En;
            var embed = new EmbedBuilder();
            embed.WithTitle("Search results");
            embed.WithDescription("Please pick one");
            int thecount = 1;
            int dacount = 0;
            string[] theitem = new string[10];
            for (int i = 0; i < items.Count(); i++)
            {
                var theitems = items[i].ItemName.ToLower();
                if (theitems.Contains(msg))
                {

                    embed.AddField($"Item {thecount}", theitems, true);
                    theitem[dacount] = theitems;
                    if (theitem.Count() > 5)
                    {
                        await Context.Channel.SendMessageAsync("", false, embed.Build());
                        break;
                    }

                    var theitemUrl = items[i].UrlName;
                    await Context.Channel.SendMessageAsync(theitemUrl);
                    break;

                }
            }
        }

        [Command("acolytes")]
        public async Task GetAcolytes()
        {

            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse = "";

            try
            {
                using (WebClient client = new WebClient())
                    // client.Encoding = Encoding.UTF8;
                    apiresponse = client.DownloadString(url);
            }
            catch (WebException exception)
            {
                return;
            }


            var warframe = Modules.Warframe.Warframe.FromJson(apiresponse);

            var activeAcolytes = warframe.PersistentEnemies;
            if (activeAcolytes.Count == 0)
            {
                await Context.Channel.SendMessageAsync("No acolytes found, are you sure they are currently spawning right now?");
                return;
            }
            for (int i = 0; i < activeAcolytes.Count; i++)
            {
                if (activeAcolytes[i].AgentType == "") break;
                string acofound = "true";
                if (!warframe.PersistentEnemies[i].Discovered)
                {
                    acofound = "false";
                    string tmpaco2 = activeAcolytes[i].AgentType;
                    var embed2 = new EmbedBuilder();
                    embed2.WithTitle("Current Acolytes");
                    embed2.AddField("Found: ", acofound, true);
                    embed2.AddField("Name: ", Utilities.ReplaceInfo(tmpaco2), true);
                    embed2.AddField("Location: ", Utilities.ReplaceInfo(activeAcolytes[i].LastDiscoveredLocation),
                        true);
                    embed2.AddField("Time Found: ", activeAcolytes[i].LastDiscoveredTime.Date, true);
                    embed2.AddField("Health till flees: ", activeAcolytes[i].HealthPercent, true);
                    embed2.AddField("Flee Damage:", activeAcolytes[i].FleeDamage, true);

                    embed2.WithColor(new Color(0, 255, 0));

                    await Context.Channel.SendMessageAsync("", false, embed2.Build());

                }
                else
                {
                    acofound = "true";
                    var timefound = Convert.ToInt64(activeAcolytes[i].LastDiscoveredTime.Date.NumberLong);
                    var curtime = DateTimeOffset.FromUnixTimeMilliseconds(timefound).DateTime.ToLocalTime();
                    string tmpaco = activeAcolytes[i].AgentType;
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Current Acolytes");
                    embed.AddField("Found: ", acofound, true);
                    embed.AddField("Name: ", Utilities.ReplaceInfo(tmpaco), true);
                    embed.AddField("Location: ", Utilities.ReplaceInfo(activeAcolytes[i].LastDiscoveredLocation), true);
                    embed.AddField("Time Found: ", curtime, true);
                    embed.AddField("Health till flees: ", activeAcolytes[i].HealthPercent, true);
                    embed.AddField("Flee Damage:", activeAcolytes[i].FleeDamage, true);

                    embed.WithColor(new Color(0, 255, 0));

                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }

            }

        }


        [Command("alerts"),Alias("al")]
        [Remarks("Lists all current alerts")]
        public async Task GetAlerts()
        {


            var apiresponse = Utilities.GetWarframeInfo();
            if (string.IsNullOrEmpty(apiresponse)) return;


            var warframe = Warframe.FromJson(apiresponse); 

            var activeAlerts = warframe.Alerts;
            string[] alertstor = new string[20];
            var embed = new EmbedBuilder();

            for (int i = 0; i < activeAlerts.Count; i++) 
            {
                string tmpalert4="None";

                var tmpalert1 = Utilities.ReplaceInfo(activeAlerts[i].MissionInfo.Location);
                var tmpalert2 = Utilities.ReplaceInfo(activeAlerts[i].MissionInfo.MissionType);
                var tmpalert3 = activeAlerts[i].MissionInfo.MissionReward.Credits;
                if (activeAlerts[i].MissionInfo.MissionReward.Items == null)
                {
                    tmpalert4 = "None";
                }
                else
                {
                    if (activeAlerts[i].MissionInfo.MissionReward.Items.Count == 1)
                    {

                        tmpalert4 = Utilities.ReplaceRewardInfo(activeAlerts[i].MissionInfo.MissionReward.Items[0]);
                    }
                }

                alertstor[i] = $"{tmpalert1} | {tmpalert2} | **Credits**: {tmpalert3} | **Items**: {tmpalert4}";

            }

            for (int i = 0; i < alertstor.Length; i++)
            {
                if (alertstor[i] == null) break;

                embed.AddField("**Alert " + i + "**:", alertstor[i], true);
            }

            embed.WithTitle("**Current Alerts**");
            embed.WithColor(new Color(188, 66, 244));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("fissures"),Alias("fis")]
        [Remarks("List all current fissures")]

        public async Task GetMissions()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse;


            using (WebClient client = new WebClient())
                //  client.Encoding = Encoding.UTF8;
                apiresponse = client.DownloadString(url);

            if (string.IsNullOrEmpty(apiresponse))
            {
                await Context.Channel.SendMessageAsync("There was an error, please try again later");
                return;
            }
            var warframe = Warframe.FromJson(apiresponse);
            var activeMissions = warframe.ActiveMissions;
            string[] fissurestor = new string[20];
            var embed = new EmbedBuilder();
            int dacount = 0;
            int fiscount = 1;
            foreach (ActiveMission am in activeMissions)

            {

                string type = am.MissionType;
                
                string damsg = $"{Utilities.GetMissions(type)}\n{Utilities.ReplaceInfo(am.Node)}\n{Utilities.ReplaceInfo(am.Modifier)}";

                fissurestor[dacount] = damsg;
                if (fissurestor[dacount] == null) break;
                var exptime = Utilities.ExpireFisTime(am.Expiry.Date.NumberLong);
                embed.AddField($"**Fissure** **{fiscount}**", $"{damsg} \n Expires in: {exptime}" , true);
                dacount = dacount + 1;
                fiscount = fiscount + 1;
            }
            embed.WithThumbnailUrl("http://3rdshifters.org/voidtear.png");
            embed.WithTitle("**Current Fissures**");
            embed.WithColor(new Color(188, 66, 244));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("sortie")]
        public async Task CurrentSortie()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse;


            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                apiresponse = client.DownloadString(url);
            }
            if (string.IsNullOrEmpty(apiresponse))
            {
                await Context.Channel.SendMessageAsync("There was an error, please try again later");
                return;
            }
            var warframe = Modules.Warframe.Warframe.FromJson(apiresponse);
            var activeSortie = warframe.Sorties; //this is a List<Sorties> 

            //set the time from unix to current
            var activationDate = activeSortie[0].Activation.Date;
            var datime = Convert.ToInt64(activationDate.NumberLong);
            var curtime = DateTimeOffset.FromUnixTimeMilliseconds(datime).DateTime.ToLocalTime();
            //set variables with all the info needed. probably not the cleanest way
            string bossname = Utilities.GetSortieBoss(activeSortie[0].Boss);
            string firstmisnode = Utilities.ReplaceInfo(activeSortie[0].Variants[0].Node);
            string firstmistype = Utilities.GetSortieType(activeSortie[0].Variants[0].ModifierType);
            string firstmis = Utilities.ReplaceInfo(activeSortie[0].Variants[0].MissionType);
            string secmistype = Utilities.ReplaceInfo(activeSortie[0].Variants[1].ModifierType);
            string secmis = Utilities.ReplaceInfo(activeSortie[0].Variants[1].MissionType);
            string secmisnode = Utilities.ReplaceInfo(activeSortie[0].Variants[1].Node);
            string thirdmis = Utilities.ReplaceInfo(activeSortie[0].Variants[2].MissionType);
            string thirdmistype = Utilities.ReplaceInfo(activeSortie[0].Variants[2].ModifierType);
            string thirdmisnode = Utilities.ReplaceInfo(activeSortie[0].Variants[2].Node);

            var embed = new EmbedBuilder();
            embed.WithTitle("Sortie Information");
            embed.WithDescription("Start Time: " + curtime);
            embed.AddField("First Mission", firstmis, true);
            embed.AddField("Modifier", firstmistype, true);
            embed.AddField("Planet", firstmisnode, true);
            embed.AddField("Second Mission", secmis, true);
            embed.AddField("Modifier", secmistype, true);
            embed.AddField("Planet", secmisnode, true);
            embed.AddField("Third Mission", thirdmis, true);
            embed.AddField("Modifier", thirdmistype, true);
            embed.AddField("Planet", thirdmisnode, true);
            embed.AddField("Final Boss", bossname, true);
            embed.WithColor(new Color(0, 255, 0));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("list fissures")]
        public async Task ListFissures()
        {
            ulong guildid = Context.Guild.Id;
            var embed = new EmbedBuilder();
            var theaccounts = UserAccounts.GetAccount(guildid);

            if (theaccounts.WantedFissures.Count == 0) return;

            for (int i = 0; i < theaccounts.WantedFissures.Count; i++)
            {
                embed.AddField($"Fissure {i + 1} : **", $"{theaccounts.WantedFissures[i]}**");

            }

            embed.WithTitle($"Current list of wanted items for **{Context.Guild.Name}");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("fissure")]
        [Remarks("Lists all known fissures found from search string")]
        public async Task ListFissure([Remainder] string msg)
        {
            var json = Utilities.GetWarframeInfo();
            var embed = new EmbedBuilder();
            if (string.IsNullOrEmpty(json))
            {
                await Context.Channel.SendMessageAsync("There was an error, please try again later!");
                return;
            }

            bool fisfound = false;
            string damsg = "";
            string exptime = "";

            var warinfo = Warframe.FromJson(json);
            var fissures = warinfo.ActiveMissions;
            embed.WithFooter("warframe alert ver1.0", "https://n9e5v4d8.ssl.hwcdn.net/images/headerLogo.png");
            
            embed.WithColor(new Color(188, 66, 244));
            foreach (var fissure in fissures)
            {
                if (Utilities.GetMissions(fissure.MissionType).ToLower().Contains(msg.ToLower()))
                {
                    damsg =
                        $"Planet: {Utilities.ReplaceInfo(fissure.Node)} Relic: {Utilities.ReplaceInfo(fissure.Modifier)}\n{exptime}";
                    exptime = Utilities.ExpireFisTime(fissure.Expiry.Date.NumberLong);
                    embed.AddField($"{Utilities.GetMissions(fissure.MissionType)}", damsg);
                    fisfound = true;
                }
            }

            if (fisfound)
            {
                embed.WithDescription($"{Utilities.FissureLink(msg.ToLower())}");
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            else
            {
                await Context.Channel.SendMessageAsync("No fissures found matching your request!");
            }



        }

    

        [Command("list rewards")]
        public async Task ListRewards()
        {

            ulong guildid = Context.Guild.Id;
            var embed = new EmbedBuilder();
            var theaccounts = UserAccounts.GetAccount(guildid);

            if (theaccounts.WantedRewards.Count == 0) return;

            for (int i = 0; i < theaccounts.WantedRewards.Count; i++)
            {
                embed.AddField($"Item {i + 1} : **", $"**{theaccounts.WantedRewards[i]}**");

            }

            embed.WithTitle($"Current list of wanted items for **{Context.Guild.Name}");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("cetustime")]
        public async Task TimeTest()
        {
            string thetime = Utilities.GetCetusTime();
            if (string.IsNullOrEmpty(thetime)) return;
            if (!Int64.TryParse(thetime, out long ignoreme)) return;
            if (Int64.Parse(thetime) > 50)
            {
                long timeremain = Int64.Parse(thetime) - 50;
                await Context.Channel.SendMessageAsync($"It is currently daytime on Cetus, night will be here in **{timeremain.ToString()} minutes**");
            }
            else
            {
                if (!string.IsNullOrEmpty(thetime)) await Context.Channel.SendMessageAsync(
                    $"It is currently nighttime on Cetus, you have roughly **{thetime} minutes** remaining till daylight!");
            }
        }
    }
}