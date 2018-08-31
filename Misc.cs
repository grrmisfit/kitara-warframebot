using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net;
using Discord.Rest;
using System.IO;
using System.Threading;
using Warframebot.Core;
using Warframebot.Core.UserAccounts;
using Warframebot.Modules.Warframe;
using Newtonsoft.Json;

namespace Warframebot.Modules
{

    public class Misc : ModuleBase<SocketCommandContext>
    {

        public async Task SendMessage(string msg)
        {
            await Context.Channel.SendMessageAsync(msg);
        }

        public async static Task SendMessageChannel(ulong chanId, string msg)
        {
            var chnl = Global.Client.GetChannel(chanId) as IMessageChannel; 
            await chnl.SendMessageAsync(msg);
        }
        public string themsg;
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
        public async Task Echo([Remainder]string message)
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
                await Global.Client.LogoutAsync();
                await Global.Client.StopAsync();
                Environment.Exit(0);
            }
            else
            {
                await SendMessage($"Sorry {killUser} you do not have permission to kill me!");
               
                return;
            }
        }

    
         
        private bool IsUserOwner(SocketGuildUser user)
        {
            string targetRoleName = "BotOwner";
            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
                         select r.Id;
            ulong roleID = result.FirstOrDefault();

            if (roleID == 0) return false;
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
        }

       
        [Command("acolytes")]
        public async Task GetAcolytes()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse = "";
            

            using (WebClient client = new WebClient())
              // client.Encoding = Encoding.UTF8;
                apiresponse = client.DownloadString(url);


            var warframe = Warframe.Warframe.FromJson(apiresponse); 

            var activeAcolytes = warframe.PersistentEnemies;

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
                    embed2.AddField("Location: ", Utilities.ReplaceInfo(activeAcolytes[i].LastDiscoveredLocation), true);
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


            var warframe = Warframe.Warframe.FromJson(apiresponse); //Warframe.Warframe.FromJson(apiresponse);

            var activeAlerts = warframe.Alerts;
            string[] alertstor = new string[20];
            var embed = new EmbedBuilder();

            for (int i = 0; i < activeAlerts.Count; i++)  //each ( Alert am in activeAlerts)
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

                        tmpalert4 = Utilities.ReplaceInfo2(activeAlerts[i].MissionInfo.MissionReward.Items[0]);
                    }
                }
                alertstor[i] = tmpalert1 + " | " + tmpalert2 + " | **Credits**: " + tmpalert3 + " | **Items**: " + tmpalert4;

            }
            for (int i = 0; i < alertstor.Length; i++)
            {
                if (alertstor[i] == null) break;
                
                embed.AddField("**Alert " + i + "**:",alertstor[i],true);
            }
            embed.WithTitle("**Current Alerts**");
            embed.WithColor(new Color(188, 66, 244));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
        [Command("fissures")]
        public async Task GetMissions()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse = "";
            

            using (WebClient client = new WebClient())
              //  client.Encoding = Encoding.UTF8;
                apiresponse = client.DownloadString(url);

            
            var warframe = Warframe.Warframe.FromJson(apiresponse);
            var seed = warframe.WorldSeed;
            var activeMissions = warframe.ActiveMissions;
            string[] fissurestor = new string[20];
            var embed = new EmbedBuilder();
            int dacount = 0;
            int fiscount = 1;
            foreach (ActiveMission am in activeMissions)

            {

                string type = activeMissions[dacount].MissionType;
                Date activationDate = am.Activation.Date;
                string damsg = Utilities.GetMissions(type) + " " + Utilities.ReplaceInfo(activeMissions[dacount].Node) + " " + Utilities.ReplaceInfo(activeMissions[dacount].Modifier);
                
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
            string apiresponse = "";
            

            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                apiresponse = client.DownloadString(url);
            }

            var warframe = Warframe.Warframe.FromJson(apiresponse);
            var seed = warframe.WorldSeed;
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
        public async Task ScrambleCommands([Remainder]string message)
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
        public async Task HelpList([Remainder]string arg = "")
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
                    embed.AddField("scram start", "Starts a scramble game, words are delayed by 10-15 secs to prevent spam", true);
                    embed.AddField("scram stop", "Stops current game", true);
                    embed.AddField("mystats", "Shows a player their current points and games won", true);

                    embed.WithColor(new Color(188, 66, 244));
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "warframe":
                    var embed2 = new EmbedBuilder();
                    embed2.WithTitle("Current commands");
                    embed2.WithDescription("use @botname or ! (depending on settings) with following commands");

                    embed2.AddField("acolytes", "Calls up information on current Acolytes and if found their location and health", true);
                    embed2.AddField("sortie", "provides current known sortie and related information", true);
                    embed2.AddField("fissures", "current fissures and type", true);
                    embed2.AddField("alerts", "current alerts plus rewards", true);
                    embed2.WithColor(new Color(0, 255, 0));

                    await Context.Channel.SendMessageAsync("", false, embed2.Build());
                    break;
            }
        }
       /* [Command("myStats")]
        public async Task MyStats()
        {
            string target = "";
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = Context.User.Username;

            var account = UserAccounts.GetAccount(target);
            await Context.Channel.SendMessageAsync($"{target} has {account.Points} points and has won {account.GamesWon} games.");
        }*/
        [Command("getfile")]
        public async Task GetTheFiles()
            {
            string thepath = "H:\\Music";
            DirectoryInfo d = new DirectoryInfo(thepath);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.mp3", SearchOption.AllDirectories); //Getting Text files
            string str = "{";
            var dalist = new Mp3List();
            char thechr = (char)34;
            char tmpchar2 = (char)123;
            char tempchar3 = (char)125;
            int songcount = 0;
            string dasong = "Song";
            string dafile = "File Name";
            string dapath = "Path";

            foreach (FileInfo file in Files)
            {

                str = str + ", " + thechr + dasong + songcount + thechr + ":" + tmpchar2 + thechr + dafile + thechr + ":" + thechr + file.Name + thechr  + "," + thechr + dapath  + thechr + ":" + thechr + file.DirectoryName + thechr + tempchar3;
                songcount = songcount + 1;
            }
            string newstring = str + "}";
            await SendMessage("test");
            System.IO.File.WriteAllText("mp3.json", newstring);
        }
        [Command("music")]
        public async Task LoadMusic()
        {
            string dafile = System.IO.File.ReadAllText("h:\\mp3.json");
            
            
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

        [Command("rewardadd")]
        public async Task SetReward([Remainder]string arg = "")
        {
            var theAccount = UserAccounts.GetAccount(Context.Guild.Id);
            for (int i = 0; i < theAccount.WantedRewards.Count; i++)
            {
                if (!theAccount.WantedRewards[i].Contains(arg))
                {
                    var rewardCount = theAccount.WantedRewards.Count + 1;
                    theAccount.WantedRewards.Add(arg);
                    UserAccounts.SaveAccounts();

                    break;
                }
                else
                {
                    await SendMessage("Item already in list!");
                    break;
                }
            }
        }

        [Command("rewarddel")]
        public async Task DelReward([Remainder]string arg = "")
        {
            var theAccount = UserAccounts.GetAccount(Context.Guild.Id);
            for (int i = 0; i < theAccount.WantedRewards.Count; i++)
            {
                if (!theAccount.WantedRewards[i].Contains(arg))
                {
                    await SendMessage("Item not in list!");
                    break;
                }
                else
                {
                    var rewardCount = theAccount.WantedRewards.Count + 1;
                    theAccount.WantedRewards.Remove(arg);
                    UserAccounts.SaveAccounts();

                    break;
                }
            }
        }
        //move these to utilities and make it oone command for add and remove
        [Command("addfissure")]
        public async Task AddFissure([Remainder] string msg)
        {
            if (msg == "")
            {
                await SendMessage("You must enter a fissure name");
                return;
            }

            var guilddata = UserAccounts.GetAccount(Context.Guild.Id);
            var json = File.ReadAllText("SystemLang/WFdata.json");
           var thedata = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            for (int i = 0; i < guilddata.WantedFissures.Count; ++i)
            {
                if (msg == guilddata.WantedFissures[i])
                {
                    await SendMessage("That fissure has already been added, did you want to add another?");
                    break;
                }
            }
            foreach (var fissure in thedata)
            {
                if (msg.ToLower() == fissure.Value.ToLower())
                {
                    var theaccount = UserAccounts.GetAccount(Context.Guild.Id);
                    theaccount.WantedFissures.Add(msg);
                    UserAccounts.SaveAccounts();
                    break;
                }
            }

        }


        [Command("set")]
        public async Task SetCommands([Remainder]string arg = "")
        {
            if (arg == "") await SendMessage("You must enter a valid set command. Type @help set for further instructions.");

            switch(arg)
            {
                case "alert channel":
                    var killUser = Context.User.Username;
                    if (Context.User.Id == Config.bot.ownerId)
                    {
                        WFSettings.AlertsChannel = Context.Channel.Id;
                        var thealertAccount = UserAccounts.GetAccount(Context.Guild.Id);
                        thealertAccount.AlertsChannel = Context.Channel.Id;
                        UserAccounts.SaveAccounts();
                    }
                    
                    break;
                case "check alerts":
                    WFSettings.CheckAlerts = true;
                    var theAccount = UserAccounts.GetAccount(Context.Guild.Id);
                    theAccount.CheckAlerts = true;
                    UserAccounts.SaveAccounts();
                    break;
                case "check fissures":
                    WFSettings.CheckFissures = true;
                    var fissureCheck = UserAccounts.GetAccount(Context.Guild.Id);
                    fissureCheck.CheckFissures = true;
                    UserAccounts.SaveAccounts();
                    break;
                    
            }
        }

        [Command("searchsongs")]
        public async Task SearchMusic([Remainder]string arg = "")

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
               // Regex.IsMatch(searchstring, arg)
                if (songsfound == 5) { break; }
                
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
                            TagLib.File damp3 = TagLib.File.Create(mp3List[dasong].Path + "/" + mp3List[dasong].FileName);
                            string artist = damp3.Tag.FirstPerformer;

                            if (artist == "") artist = "Artist name not listed";
                            multisongs[songsfound] = "**Artist**: " + artist + " **Song Name**: " + damp3.Tag.Title;
                            songsfound = songsfound + 1;
                            if (songsfound == 5 ) goto foundpart;
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
            //string somesong = mp3List[dasong].FileName;
            //TagLib.File damp3 = TagLib.File.Create(mp3List[dasong].Path + "/" + somesong);
            //string title = damp3.Tag.Title;
            //  string artist = damp3.Tag.FirstPerformer;
            if (songsfound == 0) return;
            foundpart:
            var embed = new EmbedBuilder();
            embed.WithTitle ("**Songs found in search**");
            embed.WithDescription("**More then 5 songs found, try a different search to reduce amount**");
            embed.WithColor(new Color(188, 66, 244));
            for (int i = 0; i < songsfound; i++)
            {
                string fndsong = multisongs[i];
                embed.AddField("**Song Name**: ",fndsong,true);
            }

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("time")]
        public async Task TimeTest()
        {
            //double test = Utilities.GetTimeDiff(DateTime.Today);
           // string blah = test.ToString();
            var oldtime = DateTime.Now;
            Thread.Sleep(10000);
            var newtime = DateTime.Now;
           var blah = newtime.Subtract(oldtime);
            await SendMessage(blah.Seconds.ToString());
        }
        }
}
