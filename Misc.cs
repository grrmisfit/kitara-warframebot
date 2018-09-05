using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using Warframebot.Core;
using Warframebot.Core.UserAccounts;
using Warframebot.Modules.Warframe;
using Dropbox.Api;

namespace Warframebot
{
    public static class Extensions
    {
        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                    ? value
                    : value.Substring(0, maxLength)
                );
        }
    }

    public class Misc : ModuleBase<SocketCommandContext>
    {

        public async Task SendMessage(string msg)
        {
            await Context.Channel.SendMessageAsync(msg);
        }

        public static async Task SendMessageChannel(ulong chanId, string msg)
        {
            if (Global.Client.GetChannel(chanId) is IMessageChannel chnl) await chnl.SendMessageAsync(msg);
        }

       

        [Command("Kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickUser(IGuildUser user, string reason = "No reason given.")
        {
            await user.KickAsync(reason);
        }

        [Command("Ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUser(IGuildUser user, string reason = "No reason given.")
        {
            await user.Guild.AddBanAsync(user, 5, reason);

        }



        [Command("echo")]
        public async Task Echo([Remainder] string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Message by " + Context.User.Username);
            embed.WithDescription(message);
            embed.WithColor(new Color(0, 255, 0));

            await SendMessage(message);

        }



        [Command("die")]

        public async Task KillBot()
        {
            var killUser = Context.User.Username;
            if (Context.User.Id == Config.bot.ownerId)
            {
                // var dmChannel = await Context.User.GetOrCreateDMChannelAsync();


                await Context.Channel.SendMessageAsync($"Kill command given by {killUser}");
                await Task.Delay(2);
                await Global.Client.LogoutAsync();
                await Global.Client.StopAsync();
               // Environment.Exit(0);
            }
            else
            {
                await SendMessage($"Sorry {killUser} you do not have permission to kill me!");
            }
        }



        private bool IsUserOwner(SocketGuildUser user)
        {
            string targetRoleName = "BotOwner";
            var result = from r in user.Guild.Roles
                where r.Name == targetRoleName
                select r.Id;
            ulong roleId = result.FirstOrDefault();

            if (roleId == 0) return false;
            var targetRole = user.Guild.GetRole(roleId);
            return user.Roles.Contains(targetRole);
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
                await SendMessage("No acolytes found, are you sure they are currently spawning right now?");
                return;
            }
            for (int i = 0; i < activeAcolytes.Count; i++)
            {
                if (activeAcolytes[i].AgentType == "") break;
                string acofound = "true";
                if (!warframe.PersistentEnemies[i].Discovered == true)
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


        [Command("alerts")]
        public async Task GetAlerts()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse = "";
            //apiClient.Encoding = Encoding.UTF8;

            using (WebClient client = new WebClient())

                apiresponse = client.DownloadString(url);


            var warframe = Modules.Warframe.Warframe.FromJson(apiresponse); //Warframe.Warframe.FromJson(apiresponse);

            var activeAlerts = warframe.Alerts;
            string[] alertstor = new string[20];
            var embed = new EmbedBuilder();

            for (int i = 0; i < activeAlerts.Count; i++) //each ( Alert am in activeAlerts)
            {
                string tmpalert1 = "";
                string tmpalert2 = "";
                long tmpalert3 = 0;
                string tmpalert4 = "";

                tmpalert1 = Utilities.ReplaceInfo(activeAlerts[i].MissionInfo.Location);
                tmpalert2 = Utilities.ReplaceInfo(activeAlerts[i].MissionInfo.MissionType);
                tmpalert3 = activeAlerts[i].MissionInfo.MissionReward.Credits;
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

                alertstor[i] = tmpalert1 + " | " + tmpalert2 + " | **Credits**: " + tmpalert3 + " | **Items**: " +
                               tmpalert4;

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

        [Command("fissures")]
        public async Task GetMissions()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse;


            using (WebClient client = new WebClient())
                //  client.Encoding = Encoding.UTF8;
                apiresponse = client.DownloadString(url);

            if (string.IsNullOrEmpty(apiresponse))
            {
                await SendMessage("There was an error, please try again later");
                return;
            }
            var warframe = Modules.Warframe.Warframe.FromJson(apiresponse);
            //var seed = warframe.WorldSeed;
            var activeMissions = warframe.ActiveMissions;
            string[] fissurestor = new string[20];
            var embed = new EmbedBuilder();
            int dacount = 0;
            int fiscount = 1;
            foreach (ActiveMission am in activeMissions)

            {

                string type = activeMissions[dacount].MissionType;
                //Date activationDate = am.Activation.Date;
                string damsg = Utilities.GetMissions(type) + " " + Utilities.ReplaceInfo(activeMissions[dacount].Node) +
                               " " + Utilities.ReplaceInfo(activeMissions[dacount].Modifier);

                fissurestor[dacount] = damsg;
                if (fissurestor[dacount] == null) break;
                embed.AddField("**Fissure " + fiscount + "**", fissurestor[dacount], true);
                dacount = dacount + 1;
                fiscount = fiscount + 1;
            }

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
                await SendMessage("There was an error, please try again later");
                return;
            }
            var warframe = Modules.Warframe.Warframe.FromJson(apiresponse);
            //var seed = warframe.WorldSeed;
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



        [Command("scram")]
        public async Task ScrambleCommands([Remainder] string message)
        {
            if (ScramData.GameStarted == true) return;

            ScramData.GameStarted = true;
            string command = message;
            switch (command)
            {
                case "random":
                    ScramData.Random = true;
                    break;
                case "stop":
                    ScramData.GameStarted = false;
                    await SendMessageChannel(ScramData.ScramChannel, "**Scramble game stopping**");
                    break;
                case "start":
                    ScramData.Random = true;
                    ScramData.GameStarted = true;
                    ScramData.WordGuessed = true;
                    ScramData.ScramChannel = Context.Channel.Id;
                    await SendMessageChannel(ScramData.ScramChannel, "**Scramble game starting**");
                    break;

            }

            await RepeatingTimer.StartScramTimer();


        }



        [Command("react")]
        public async Task HandleReactionMessage()
        {
            RestUserMessage msg = await Context.Channel.SendMessageAsync("React to me!");
            Global.MessageIdToTrack = msg.Id;
        }

        [Command("help")]
        public async Task HelpList([Remainder] string arg = "")
        {
            string value = arg;
            switch (value)
            {
                case "":
                    var helpembed = new EmbedBuilder();
                    helpembed.WithTitle("**Help topics**");
                    helpembed.WithDescription("**List of current help topics.**");
                    helpembed.AddField("Topic 1", "scramble", true);
                    helpembed.AddField("Topic 2", "warframe", true);
                    await Context.Channel.SendMessageAsync("", false, helpembed.Build());
                    break;
                case "scramble":

                    var embed = new EmbedBuilder();
                    embed.WithTitle("Current commands");
                    embed.WithDescription("use @botname or ! (depending on settings) with following commands");
                    embed.AddField("scram start",
                        "Starts a scramble game, words are delayed by 10-15 secs to prevent spam", true);
                    embed.AddField("scram stop", "Stops current game", true);
                    embed.AddField("mystats", "Shows a player their current points and games won", true);

                    embed.WithColor(new Color(188, 66, 244));
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "warframe":
                    var embed2 = new EmbedBuilder();
                    embed2.WithTitle("Current commands");
                    embed2.WithDescription("use @Kitara or ! (depending on settings) with following commands.");

                    embed2.AddField("acolytes",
                        "Calls up information on current Acolytes and if found their location and health.", true);
                    embed2.AddField("sortie", "provides current known sortie and related information.", true);
                    embed2.AddField("fissures", "current fissures and type.", true);
                    embed2.AddField("alerts", "current alerts plus rewards.", true);
                    embed2.AddField("Set", "use set(settings) help for help with set command.", true);
                    embed2.AddField("cetustime",
                        "tells you the current time on cetus and how long before day/night starts/ends.");
                    embed2.AddField("list rewards",
                        "list all current wanted rewards that are searched for in alerts and invasions.");
                    embed2.AddField("list fissures", "Lists all the saved wanted fissures",true);

                    embed2.WithColor(new Color(0, 255, 0));

                    await Context.Channel.SendMessageAsync("", false, embed2.Build());
                    break;
            }
        }

       
        [Command("getfile")]
        public async Task GetTheFiles()
        {
            string thepath = "H:/Music";
            DirectoryInfo d = new DirectoryInfo(thepath); //Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.mp3", SearchOption.AllDirectories); //Getting Text files
            string str = "{";
            // var dalist = new Mp3List();
            char thechr = (char) 34;
            char tmpchar2 = (char) 123;
            char tempchar3 = (char) 125;
            int songcount = 0;
            string dasong = "Song";
            string dafile = "File Name";
            string dapath = "Path";

            foreach (FileInfo file in Files)
            {

                str = str + ", " + thechr + dasong + songcount + thechr + ":" + tmpchar2 + thechr + dafile + thechr +
                      ":" + thechr + file.Name + thechr + "," + thechr + dapath + thechr + ":" + thechr +
                      file.DirectoryName + thechr + tempchar3;
                songcount = songcount + 1;
            }

            string newstring = str + "}";
            await SendMessage("test");
            System.IO.File.WriteAllText("mp3.json", newstring);
        }

        [Command("music")]
        public async Task LoadMusic()
        {
            string dafile = System.IO.File.ReadAllText("h:/mp3.json");


            var mp3List = Mp3List.FromJson(dafile);
            Random ransong = new Random();
            int songnum = ransong.Next(mp3List.Count);
            string picksong = "Song" + songnum;
            string dasong = mp3List[picksong].FileName;
            TagLib.File damp3 = TagLib.File.Create(mp3List[picksong].Path + "/" + dasong);
            string title = damp3.Tag.Title;
            string artist = damp3.Tag.FirstPerformer;
            await SendMessage("**Artist:** " + artist + " **Song Name:** " + title);

        }

        [Command("music count")]
        public async Task GetMusicCount()
        {
            string dafile = System.IO.File.ReadAllText("h:/mp3.json");
            if (string.IsNullOrEmpty(dafile))
            {
                await SendMessage("mp3 data not found!");
                return;
            }
            
            var mp3List = Mp3List.FromJson(dafile);
            string musicount = mp3List.Count.ToString();
            await SendMessage($"There are currently **{musicount}** known songs.");
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

        [Command("add reward")]
        public async Task SetReward([Remainder] string msg)
        {
            if (msg == "")
            {
                await SendMessage("No input detected");
                return;
            }

            var rewardCheck = Utilities.AddRewards(Context.Guild.Id, msg);
            if (rewardCheck == "added")
            {
                await SendMessage($"{msg} has been added! Use remove reward to delete.");
            }
            else
            {
                await SendMessage(
                    $"**{msg}** is already in the list, or something went wrong! use **!list rewards** to check if its in the list");
            }
            
        }

        [Command("remove reward")]
        public async Task DelReward([Remainder] string msg)
        {
            bool addCheck = false;
            var theAccount = UserAccounts.GetAccount(Context.Guild.Id);
            for (int i = 0; i < theAccount.WantedRewards.Count; i++)
            {
                if (theAccount.WantedRewards[i].ToLower().Contains(msg.ToLower()))
                {

                    theAccount.WantedRewards.Remove(msg);
                    UserAccounts.SaveAccounts();
                    addCheck = true;
                    break;
                }
            }

            if (addCheck == false)
            {
                await SendMessage($"{msg} not found in list!");
                return;
            }

            await SendMessage($"{msg} removed from list!");
        }

        [Command("remove fissure")]
        public async Task DelFissure([Remainder] string msg)
        {
            bool addCheck = false;
            var theAccount = UserAccounts.GetAccount(Context.Guild.Id);
            for (int i = 0; i < theAccount.WantedFissures.Count; i++)
            {
                if (theAccount.WantedFissures[i].ToLower().Contains(msg.ToLower()))
                {

                    theAccount.WantedFissures.Remove(msg);
                    UserAccounts.SaveAccounts();
                    addCheck = true;
                    break;
                }
            }

            if (addCheck == false)
            {
                await SendMessage($"{msg} not found in list!");
                return;
            }

            await SendMessage($"{msg} removed from list!");
        }

        [Command("add fissure")]
        public async Task AddFissureAlerts([Remainder] string msg)
        {
            if (msg == "")
            {
                await SendMessage("You must enter a fissure name");
                return;
            }

            var addCheck = Utilities.AddFissures(Context.Guild.Id, msg);
            if (addCheck == "added")
            {
                await SendMessage($"{msg} has been added! Use remove fissure to delete");
            }
            else
            {
                await SendMessage(
                    $"**{msg}** is already in the list, or something went wrong! use **!list fissures** to check if its in the list");
            }

        }

        [Command("set")]
        public async Task SetCommands([Remainder] string arg = "")
        {
            if (arg == "")
            {
                await SendMessage("You must enter a valid set command. Type @help set for further instructions.");
                return;
            }

            switch (arg)
            {
                case "alert channel":

                    if (Context.User.Id == Config.bot.ownerId)
                    {
                        WFSettings.AlertsChannel = Context.Channel.Id;
                        var thealertAccount = UserAccounts.GetAccount(Context.Guild.Id);
                        thealertAccount.AlertsChannel = Context.Channel.Id;
                        UserAccounts.SaveAccounts();
                        await SendMessage(
                            $"Alert channel has been set to {Context.Channel.Name} on server {Context.Guild.Name}");
                        break;
                    }

                    await SendMessage($"{Context.User.Username}, you do not have proper privileges to set that!");
                    break;
                    
                case "check alerts":

                    var theAccount = UserAccounts.GetAccount(Context.Guild.Id);
                    if (theAccount.CheckAlerts)
                    {
                        theAccount.CheckAlerts = false;
                        UserAccounts.SaveAccounts();
                        await SendMessage("Alert rewards messages are now turned off!");
                        break;
                    }

                    theAccount.CheckAlerts = true;
                    UserAccounts.SaveAccounts();
                    await SendMessage($"You will now get alerts for saved rewards every {theAccount.AlertDelay} mins");
                    break;
                case "check fissures":

                    var fissureCheck = UserAccounts.GetAccount(Context.Guild.Id);
                    if (fissureCheck.CheckFissures)
                    {
                        fissureCheck.CheckFissures = false;
                        UserAccounts.SaveAccounts();
                        await SendMessage("Fissures alerts are now turned off!");
                        break;
                    }

                    fissureCheck.CheckFissures = true;
                    UserAccounts.SaveAccounts();
                    await SendMessage(
                        $"You will now get alerts for saved fissures every {fissureCheck.AlertDelay} mins");
                    break;
                case "alert cetus":
                    var cetuscheck = UserAccounts.GetAccount(Context.Guild.Id);
                    if (cetuscheck.CetusTime == false)
                    {
                        cetuscheck.CetusTime = true;
                        UserAccounts.SaveAccounts();
                        await SendMessage("Cetus nightime alerts are now on!");
                        break;
                    }
                    else
                    {
                        cetuscheck.CetusTime = false;
                        UserAccounts.SaveAccounts();
                        await SendMessage("Cetus nightime alerts are now off!");
                        break;
                    }

                default:
                    await SendMessage("Command not recognized ");
                    break;
            }
        }

        [Command("delay")]
        public async Task SetDelay([Remainder] string msg)
        {
            int delaytime = 5;
            if (Int32.TryParse(msg, out delaytime))
            {
                var delayaccount = UserAccounts.GetAccount(Context.Guild.Id);
                var checkedtime = Int32.Parse(msg);
                delayaccount.AlertDelay = checkedtime;
                UserAccounts.SaveAccounts();
                await SendMessage($"Messages will be sent every **{msg}** minutes!");
            }
        }


        [Command("searchsongs")]
        public async Task SearchMusic([Remainder] string arg = "")

        {
            if (arg == "") await SendMessage("Must enter some words to search for!");
            string dafile = System.IO.File.ReadAllText("mp3.json");

            var mp3List = Mp3List.FromJson(dafile);
            int dasongcount = 0;
            string dasong = "Song" + dasongcount;
            int songsfound = 0;
            int searchcount = 0;
            string[] multisongs = new string[25];
            for (int i = 0; i < mp3List.Count; i++)
            {

                if (songsfound == 5)
                {
                    break;
                }

                string searchstring = mp3List[dasong].FileName;
                searchstring = searchstring.ToLower();
                arg = arg.ToLower();
                if (arg.Contains(" "))
                {

                    string[] words = arg.Split(' ');

                    foreach (var word in words)
                    {
                        int wordcount = 0;
                        int wordcount2 = 1;
                        if (wordcount > words.Length) goto foundpart;
                        if (wordcount2 > words.Length) goto foundpart;
                        if (searchstring.Contains(words[wordcount]) && searchstring.Contains(words[wordcount2]))
                        {
                            TagLib.File damp3 =
                                TagLib.File.Create(mp3List[dasong].Path + "/" + mp3List[dasong].FileName);
                            string artist = damp3.Tag.FirstPerformer;

                            if (artist == "") artist = "Artist name not listed";
                            multisongs[songsfound] = "**Artist**: " + artist + " **Song Name**: " + damp3.Tag.Title;
                            songsfound = songsfound + 1;
                            if (songsfound == 5) goto foundpart;
                            wordcount = wordcount + 1;
                            wordcount2 = wordcount2 + 1;
                        }

                    }
                }
                else
                {
                    if (searchstring.Contains(arg))
                    {
                        TagLib.File damp3 = TagLib.File.Create(mp3List[dasong].Path + "/" + mp3List[dasong].FileName);
                        string artist = damp3.Tag.FirstPerformer;

                        if (artist == "") artist = "Artist name not listed";
                        multisongs[songsfound] = "**Artist**: " + artist + " **Song Name**: " + damp3.Tag.Title;
                        songsfound = songsfound + 1;

                    }
                }

                dasongcount = dasongcount + 1;
                searchcount = searchcount + 1;
                dasong = "Song" + dasongcount;
            }

            if (songsfound == 0) return;
            foundpart:
            var embed = new EmbedBuilder();
            embed.WithTitle("**Songs found in search**");
            embed.WithDescription("**More then 5 songs found, try a different search to reduce amount**");
            embed.WithColor(new Color(188, 66, 244));
            for (int i = 0; i < songsfound; i++)
            {
                string fndsong = multisongs[i];
                embed.AddField("**Song Name**: ", fndsong, true);
            }

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("cetustime")]
        public async Task TimeTest()
        {
            string thetime = Utilities.GetCetusTime();
            if (string.IsNullOrEmpty(thetime)) return;
            if(!Int64.TryParse(thetime,out long ignoreme))return;
           if( Int64.Parse(thetime) > 50)
            {
                long timeremain = Int64.Parse(thetime) - 50;
                await SendMessage($"It is currently daytime on Cetus, night will be here in **{timeremain.ToString()} minutes**");
            }
            else
            {
                if(!string.IsNullOrEmpty(thetime)) await SendMessage(
                    $"It is currently nighttime on Cetus, you have roughly **{thetime} minutes** remaining till daylight!");
            }
        }

        [Command("alarm")]
        public async  Task SetCetusAlarm([Remainder] string msg)
        {
            var user = Context.User.Id;
            var json = string.Empty;
            
            json = Utilities.LoadJsonData("SystemLang/Alarm.json");
            if (string.IsNullOrEmpty(json))
            {
                await SendMessage("There was an error, please try again later!");
                return;
            }
           
            JArray a = JArray.Parse(json);
            List<UserAccount> tmpdata = new List<UserAccount>();
            var alarmUsers = a.ToObject<List<UserAccount>>();
            var alarmUser = UserAccounts.GetAlarmUser(user, 15);
            foreach (var users in alarmUsers)
            {
                if (users.DiscordId == user)
                {
                    if (msg.ToLower() == "off")
                    {
                        alarmUser.AlarmOn = false;
                        UserAccounts.SaveAlarmUser();
                        return;
                    }

                    {
                        if (msg.ToLower() == "on")
                        {
                            alarmUser.AlarmOn = true;
                            UserAccounts.SaveAlarmUser();
                            return;
                        }
                    }
                }
            }

            int delaytime = 10;
            if (Int32.TryParse(msg, out delaytime))
            {
                var wanteddelay = Int32.Parse(msg);
                var theuser = UserAccounts.GetAlarmUser(user, wanteddelay);
                
                theuser.AlarmDelay = wanteddelay;
                theuser.AlarmChannel = Context.Channel.Id;
                UserAccounts.SaveAlarmUser();
                await SendMessage(
                    $"<@{Context.User.Id}> I have added an alarm for cetus nighttime with a delay of **{wanteddelay}**!");
            }
        }

        [Command("test")]
        public async Task TestCommand()
        {
            var test = Context.User.Id;
            if (test == Config.bot.ownerId)
            {
                 await Context.Channel.SendMessageAsync($"Update command given by {test}, I will now shutdown and update in 5 secs");
                 await Task.Delay(1000);
                 await Global.Client.LogoutAsync();
                 await Global.Client.StopAsync();
                 Process.Start("UpdateBot.exe");
                 //await Task.Delay(3000);
                 
                 Environment.Exit(0);
                  
                
              
            }
               // await SendMessage($"<@{test}> ");
        }
       
    }
}