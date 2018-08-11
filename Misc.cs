using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using warframebot.Core.UserAccounts;
using System.Net;
using Discord.Rest;
using Newtonsoft.Json;
using warframebot.Modules.Warframe;
using Newtonsoft.Json.Linq;
using warframebot.Core;
using System.IO;
using discordbot.Modules;

namespace warframebot.Modules
{
    
    public class Misc : ModuleBase<SocketCommandContext>
    {
        
        public async Task SendMessage(string msg)
            {
           await Context.Channel.SendMessageAsync( msg);
            }
        public async static Task SendMessageChannel(ulong chanId, string msg)

        {

            var chnl = Global.Client.GetChannel(chanId) as IMessageChannel; // 4
            await chnl.SendMessageAsync(msg); // 5 
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
           // await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

       

       /* [Command("secret")]

        public async Task RevealSecret([Remainder]string arg = "")
        {
            if (!IsUserOwner((SocketGuildUser)Context.User)) return;
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("SECRET"));

        }
        */
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
        
      // will look back into this at a later date
        [Command("acolytes")]
        public async Task GetAcolytes()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse = "";
            //apiClient.Encoding = Encoding.UTF8;

            using (WebClient client = new WebClient())

                apiresponse = client.DownloadString(url);


            var warframe = Warframe.Warframe.FromJson(apiresponse); //Warframe.Warframe.FromJson(apiresponse);
           
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
                

               
               // await SendMessage(Utilities.ReplaceInfo(tmpaco));
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
                await SendMessage(tmpalert1 + " | " + tmpalert2 + " | Credits: " + tmpalert3 + " | Items: " + tmpalert4);
                
            }
        }
        [Command("fissures")]
        public async Task GetMissions()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse = "";
            //apiClient.Encoding = Encoding.UTF8;

            using (WebClient client = new WebClient())
               
                apiresponse = client.DownloadString(url);

            // Warframe warframe = JsonConvert.DeserializeObject<Warframe>(apiresponse);
           // using Warframe;
            var warframe = Warframe.Warframe.FromJson(apiresponse);
            var seed = warframe.WorldSeed;
            var activeMissions = warframe.ActiveMissions; //this is a List<ActiveMission> 
            
            int dacount = 0;
            foreach (ActiveMission am in activeMissions)
               
            {
                
                string type = activeMissions[dacount].MissionType;
               Date activationDate = am.Activation.Date;

                 themsg = themsg + Utilities.GetMissions(type) + " " + Utilities.ReplaceInfo(activeMissions[dacount].Node) + " " + Utilities.ReplaceInfo(activeMissions[dacount].Modifier) + "\n";
             dacount = dacount + 1;
               
            }
                await SendMessage(themsg);
        }
            [Command("sortie")]
            public async Task CurrentSortie()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse = "";
            //api

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
            embed.WithDescription("Start Time: " + curtime );
            embed.AddField("First Mission", firstmis, true);
            embed.AddField("Modifier", firstmistype, true);
            embed.AddField("Planet", firstmisnode, true);
            embed.AddField("Second Mission", secmis, true);
            embed.AddField("Modifier",secmistype, true);
            embed.AddField("Planet", secmisnode, true);
            embed.AddField("Third Mission", thirdmis, true);
            embed.AddField("Modifier", thirdmistype, true);
            embed.AddField("Planet", thirdmisnode, true);
            embed.AddField("Final Boss", bossname, true);
            embed.WithColor(new Color(0, 255, 0));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
 }

     

        [Command("scram")]
        public async Task MyStats([Remainder]string theword)
        {
            string scramword = Scramble.ScrambleWord(theword);
           await Context.Channel.SendMessageAsync(scramword);
        }

     
       
       [Command("react")]
       public async Task HandleReactionMessage()
        {
           RestUserMessage msg = await Context.Channel.SendMessageAsync("React to me!");
            Global.MessageIdToTrack = msg.Id;
        }
      
        [Command("help")]
        public async Task HelpList()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Current commands");
            embed.WithDescription("use @botname or ! (depending on settings) with following commands");
           
            embed.AddField("acolytes", "Calls up information on current Acolytes and if found their location and health", true);
            embed.AddField("sortie", "provides current known sortie and related information", true);
            embed.AddField("fissures", "current fissures and type", true);
            embed.AddField("alerts", "current alerts plus rewards", true);
            embed.WithColor(new Color(0, 255, 0));

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

    }
}
