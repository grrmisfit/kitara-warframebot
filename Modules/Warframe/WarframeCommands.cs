using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using Warframebot.Core.UserAccounts;
using Warframebot.Data;
using Warframebot.Storage;


namespace Warframebot.Modules.Warframe
{
    public class WarframeCommands :  InteractiveBase
    {
        private readonly IDataStorage _storage;
        /*
        [Command("acolytes")]
        [Remarks("Search for acolytes location")]
        [Summary("Only useful when acolytes are actually in game.")]
        public async Task GetAcolytes()
        {

            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse;

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
            var warframe = Warframe.FromJson(apiresponse);

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

                    embed2.WithColor(new Color(188, 66, 244));

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

                    embed.WithColor(new Color(188, 66, 244));

                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }

            }

        }
        */
        [Command("news")]
        [Summary("Gets last 6 news stories and posts them in order of newest to latest")]
        public async Task CheckNews()
        {
            var json = Utilities.GetWarframeInfo();

            if (string.IsNullOrEmpty(json))
            {
                return;
            }
            var warframe = Warframe.FromJson(json);
            var news = warframe.Events;
            var embed = new EmbedBuilder();
            string newsLink = "";
            string newsMsg = "";
            List<string> newsList1 = new List<string>();
            List<string> newsList2 = new List<string>();

            foreach (var n in news)
            {

                var curtime = Utilities.TimeSince(n.Date.Date.NumberLong);
                var time = Int64.Parse(curtime);
               
                    foreach (var t in n.Messages)
                    {
                        if (t.LanguageCode == "en")
                        {
                            DateTimeOffset timePosted = DateTimeOffset.FromUnixTimeMilliseconds(Int64.Parse(n.Date.Date.NumberLong));
                        newsMsg = t.MessageMessage;
                            newsLink = n.Prop.AbsoluteUri;
                            newsList1.Add($"{newsMsg}: ");
                            newsList2.Add($"{newsLink}\n Posted on: {timePosted.LocalDateTime}");

                            break;
                        }
                    }





                if (embed.Fields.Count > 6)
                {
                    break;
                }

            }
            for (int i = newsList1.Count - 1; i >= 0; i--)
            {
                embed.AddField(newsList1[i], newsList2[i]);
            }
            embed.WithTitle("News Alerts");
            embed.WithFooter("warframe alert ver 1.0", "http://3rdshifters.org/headerLogo.png");
            embed.WithColor(new Color(188, 66, 244));

            await Context.Channel.SendMessageAsync("", false, embed.Build());

        }
        [Command("relic")]
        public async Task DropsFromRelics([Remainder] string item)
        {
            var json = File.ReadAllText("SystemLang/relics.json");
            var therelics = RelicsData.FromJson(json);
            var embed  = new EmbedBuilder();
            embed.WithTitle($"Drops from relic {item}");
            embed.WithFooter("warframe alert ver 1.0", "http://3rdshifters.org/headerLogo.png");
            embed.WithColor(new Color(188, 66, 244));
            foreach (var relic in therelics.Relics)
            {
                var relicname = relic.Tier + " " + relic.RelicName;
                if (item.ToLower() == relicname.ToLower())
                {
                    for (int i = 0; i < relic.Rewards.Count; i++)
                    {

                        embed.AddField($"Item {i + 1} : ",$"{relic.Rewards[i].ItemName}\nRarity: {relic.Rewards[i].Rarity}\nChance: {relic.Rewards[i].Chance}%");

                    }

                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                }
                
            }
        }

        [Command("wrelic")]
        public async Task WhichRelic([Remainder] string item)
        {
            bool isVaulted = false;
            
            var embedcount = 0;
            var vJson = File.ReadAllText("Data/vaulted.json");
           // JArray a = JArray.Parse(vJson);
            
            var json = File.ReadAllText("SystemLang/relics.json");
            var therelics = RelicsData.FromJson(json);
            var embed = new EmbedBuilder();
            embed.WithTitle($"Relics found containing {item}.");
            embed.WithFooter("warframe alert ver 1.0", "http://3rdshifters.org/headerLogo.png");
            embed.WithColor(new Color(188, 66, 244));
            foreach (var relic in therelics.Relics)
            {
                var relicname = relic.Tier + " " + relic.RelicName;
                
                {
                    for (int i = 0; i < relic.Rewards.Count; i++)
                    {
                        var currelic = relic.Rewards[i].ItemName.ToLower();
                        if (currelic.Contains(item.ToLower()) && relic.State == State.Intact)
                        {
                            if (embedcount > 5)
                            {
                                await Context.Channel.SendMessageAsync(
                                    "More then 5 results found, first 5 listed next.");
                                goto sendme;
                            }
                           embed.AddField($"Relic {embedcount + 1}: ",$" {relic.Tier} {relic.RelicName}\nRarity: {relic.Rewards[i].Rarity}" );
                            embedcount = embedcount + 1;
                        }
                            
                    }

                   
                    
                }

            }
            sendme:
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("alerts"),Alias("al")]
        [Remarks("Lists all current alerts currently in game")]
        [Summary("Example: !alerts will display each alert with location, type of mission and reward in credits or items.")]
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

                alertstor[i] =
                    $"**Location**: {tmpalert1}\n **Mission Type**: {tmpalert2}\n**Credits**: {tmpalert3}\n**Items**: {tmpalert4}\n**Expires**: {Utilities.ExpireFisTime(activeAlerts[i].Expiry.Date.NumberLong)}";

            }

            for (int i = 0; i < alertstor.Length; i++)
            {
                if (alertstor[i] == null)
                {
                    break;
                }
                var alertcount = i + 1;
                embed.AddField("**Alert " + alertcount + "**:", alertstor[i], true);
            }

            embed.WithTitle("**Current Alerts**");
            embed.WithFooter("warframe alert ver 1.0", "http://3rdshifters.org/headerLogo.png");
            embed.WithColor(new Color(188, 66, 244));
            //embed.WithThumbnailUrl("http://3rdshifters.org/voidtear.png");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("fissures"),Alias("fis")]
        [Remarks("List all current fissures currently in game")]
        [Summary("Example: !fissures or !fis will display current known fissures with location, type and enemy.")]
        public async Task GetMissions()
        {
            
            string apiresponse = Utilities.GetWarframeInfo();

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
                if (fissurestor[dacount] == null)
                {
                    break;
                }
                var exptime = Utilities.ExpireFisTime(am.Expiry.Date.NumberLong);
                embed.AddField($"**Fissure** **{fiscount}**", $"{damsg} \n Expires in: {exptime}" , true);
                dacount = dacount + 1;
                fiscount = fiscount + 1;
            }
            
            embed.WithTitle("**Current Fissures**");
            embed.WithFooter("warframe alert ver 1.0", "http://3rdshifters.org/headerLogo.png");
            embed.WithColor(new Color(188, 66, 244));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("sortie")]
        [Summary("Gives information on current sortie")]
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
            var warframe = Warframe.FromJson(apiresponse);
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
            embed.WithColor(new Color(188, 66, 244));
            embed.WithFooter("warframe alert ver 1.0", "http://3rdshifters.org/headerLogo.png");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("wanted fissures"),Alias("wf")]
        [Remarks("Lists wanted fissures for current Discord server")]
        [Summary("Search results are specific to a specific discord server. To remove an item do !remove fissure fissurename")]
        public async Task ListWantedFissures()
        {
            ulong guildid = Context.Guild.Id;
            var embed = new EmbedBuilder();
            var theaccounts = DbStorage.GetGuildInfo(guildid);
            var curcnt = 1;
            if (theaccounts.Fissures.WantedFissures.Count == 0)
            {
                return;
            }

            for (int i = 0; i < theaccounts.Fissures.WantedFissures.Count; i++)
            {
                embed.AddField($"Fissure {i + 1} : ", $"**{theaccounts.Fissures.WantedFissures[i]}**");

            }
            embed.WithColor(new Color(188, 66, 244));
            embed.WithFooter("warframe alert ver 1.0", "http://3rdshifters.org/headerLogo.png");
            embed.WithTitle($"Current list of wanted fissures for **{Context.Guild.Name}**");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("fissure")]
        [Remarks("Lists all known fissures found from search string")]
        [Summary("Example: !fissure defense will tell you any current known fissures that are of defense type.")]
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
            embed.WithFooter("warframe alert ver 1.0", "http://3rdshifters.org/headerLogo.png");
            
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

    

        [Command("wanted rewards"),Alias("wr")]
        [Remarks("Lists all wanted rewards.")]
        [Summary("To remove a specific reward use !remove reward rewardname")]
        
        public async Task ListRewards()
        {

            ulong guildid = Context.Guild.Id;
            var embed = new EmbedBuilder();
            var theaccounts = DbStorage.GetGuildInfo(guildid);

            if (theaccounts.Rewards.WantedRewards.Count == 0)
            {
                return;
            }

            for (int i = 0; i < theaccounts.Rewards.WantedRewards.Count; i++)
            {
              embed.AddField($"Item {i + 1} : ", $"**{theaccounts.Rewards.WantedRewards[i]}**");

            }

            embed.WithTitle($"Current list of wanted items for **{Context.Guild.Name}");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("invasions"), Alias("inv")]
        public async Task ListInvasions()
        {
            var apiresponse = Utilities.GetWarframeInfo();
            if (string.IsNullOrEmpty(apiresponse))
            {
                return;
            }
            var invasions = Warframe.FromJson(apiresponse);
            var invasionRewards = invasions.Invasions;
            var dacount = 1;
            var embed = new EmbedBuilder();
            string defReward = "";
            foreach (var reward in invasionRewards)
            {
                if (reward.Completed) continue;
                var atkReward = "";
                if (reward.AttackerReward.AnythingArray == null)
                {
                    atkReward = "Nothing";
                }

                atkReward = reward.AttackerReward.ErReward == null ? "Nothing" : Utilities.ReplaceRewardInfo(reward.AttackerReward.ErReward.CountedItems[0].ItemType);

                if (reward.AttackerReward.AnythingArray == null)
                {
                    defReward = Utilities.ReplaceRewardInfo(reward.DefenderReward.CountedItems[0].ItemType);
                }
                if(!atkReward.Contains("Nothing"))
                {
                    embed.AddField($"Invasion {dacount}: Attack Reward: {atkReward} ",
                $"On {Utilities.ReplaceInfo(reward.Node)} Faction: {reward.AttackerMissionInfo.Faction} vs {reward.DefenderMissionInfo.Faction}");

                }

                var faction1 = Utilities.ReplaceInfo(reward.DefenderMissionInfo.Faction.ToString());
                embed.AddField($"Invasion {dacount}: Defender Reward: {defReward} ",
                    $"On {Utilities.ReplaceInfo(reward.Node)} Faction: {Utilities.ReplaceInfo(reward.DefenderMissionInfo.Faction.ToString())} vs {Utilities.ReplaceInfo(reward.AttackerMissionInfo.Faction.ToString())}");
                dacount = dacount + 1;
            }

            embed.WithTitle("Current Invasions");
            embed.WithColor(new Color(188, 66, 244));
            embed.WithFooter("warframe alert ver 1.0", "http://3rdshifters.org/headerLogo.png");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("cetustime")]
        [Remarks("Cetus night//day check")]
        [Summary("Tells you if its day or night on Cetus and how long you have till next cycle")]
        public async Task TimeTest()
        {
            string thetime = Utilities.GetCetusTime();
            if (string.IsNullOrEmpty(thetime))
            {
                return;
            }
            if (!Int64.TryParse(thetime, out long ignoreme))
            {
                return;
            }
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