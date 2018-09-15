using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using Warframebot.Core.UserAccounts;
using Warframebot.Modules.Warframe;

namespace Warframebot.Modules
{


    public class Misc : ModuleBase<SocketCommandContext>
    {
        private CommandService _service;

        public Misc(CommandService service)
        {
            _service = service;
        }
        public  async Task SendMessage(string msg)
        {
            await Context.Channel.SendMessageAsync(msg);
        }

        public static async Task SendMessageChannel(ulong chanId, string msg)
        {
            if (Global.Client.GetChannel(chanId) is IMessageChannel chnl) await chnl.SendMessageAsync(msg);
        }

        [Command("test")]
        public async Task TestModule()
        {
            var themods = _service.Modules;
            await SendMessage(themods.ToString());
        }
        [Command("help"), Alias("h"),
         Remarks(
             "DMs you a message if you dont specify a command -")]
        public async Task Help()
        {
            await Context.Channel.SendMessageAsync("Check your DMs.");

            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();

            var contextString = Context.Guild?.Name ?? "DMs with me";
            var builder = new EmbedBuilder()
            {
                Title = "Help",
                Description = $"These are the commands you can use in {contextString}",
                Color = new Color(114, 137, 218)
            };
            var themods = _service.Modules;
            foreach (var module in themods)
            {
                
                await AddModuleEmbedField(module, builder);
            }

            await dmChannel.SendMessageAsync("", false, builder.Build());

            // Embed are limited to 24 Fields at max. So lets clear some stuff
            // out and then send it in multiple embeds if it is too big.
            builder.WithTitle("")
                .WithDescription("")
                .WithAuthor("");
            while (builder.Fields.Count > 24)
            {
                builder.Fields.RemoveRange(0, 25);
                await dmChannel.SendMessageAsync("", false, builder.Build());

            }
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

       /* [Command("delete last")]
        [Alias("dl")]
        public async Task DeleteMessage()
        {
            var amount = 100;
            var userId = Global.Client.CurrentUser.Id;
            IEnumerable<IMessage> messages =  await Context.Message.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            //var result = messages.Where(x => x.Author.Id == user.Id && x.CreatedAt >= DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(14)));
            List<string> msglist = new List<string>();
            foreach (var themsg in messages)
            {
                if (themsg.Author.Id == userId)
                {
                   // msglist.Add(themsg);
                }
            }
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
*/

        [RequireOwner]
        [Command("die")]
        [Remarks("Tells the bot to close and log off!")]
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


       
        /* 
        [Command("react")]
        public async Task HandleReactionMessage()
        {
            RestUserMessage msg = await Context.Channel.SendMessageAsync("React to me!");
            Global.MessageIdToTrack = msg.Id;
        }

       [Command("helpold")]
        public async Task HelpList([Remainder] string arg = "")
        {
            var embed = new EmbedBuilder();
            embed.WithColor(new Color(0, 255, 0));
            string value = arg;
            switch (value)
            {
                case "":
                    var helpembed = new EmbedBuilder();
                    helpembed.WithTitle("**Help topics**");
                    helpembed.WithDescription("**List of current help topics.**");
                    //embed2.WithDescription("use @Kitara or ! (depending on settings) with following commands.");
                    helpembed.AddField("Help for the following commands, type !help and one of the commands to get help on the command", "acolytes \n sortie \n fissures \n alerts \n cetustime \n list rewards" +
                                                   " \n list fissures \n set", true);
                    await Context.Channel.SendMessageAsync("", false, helpembed.Build());
                    break;

                case "acolytes":
                    embed.WithTitle("Acolytes command information");
                    embed.AddField("acolytes",
                        "Calls up information on current Acolytes and if found their location and health.", true);
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "sortie":
                    embed.AddField("sortie", "provides current known sortie and related information.", true);
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "fissures":
                    embed.AddField("fissures", "Tells you the current fissures and relic type.", true);
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "alerts":
                    embed.AddField("alerts", "Tells you the current alerts plus rewards.", true);
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "cetustime":
                    embed.AddField("cetustime",
                        "tells you the current time on cetus and how long before day/night starts/ends.");
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "list rewards":
                    embed.AddField("list rewards",
                        "list all current wanted rewards that are searched for in alerts and invasions.");
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "list fissures":
                    embed.AddField("list fissures", "Lists all the saved wanted fissures", true);
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "market sell":
                    embed.AddField("Market search", "Search Warframe.market for top 5 cheapest orders. " +
                                                    "This requires an exact name, " +
                                                    "example !market sell nova prime set will return the top 5 cheapest " +
                                                    "sellers of nova prime sets with username and plat amount",true);
                    embed.AddField("Tip",
                        "if you dont know exact name use !find with a partial name like !find nova and then pick the option you want.");
                    
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                case "set":
                    embed.AddField("set",
                        "Use this command to set various options in the bot. Use !set help for help with specific help commands.");
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                    break;
                default:
                    await SendMessage("Help topic not found, try !help for list of help topics.");
                    break;
            }
        }
        
        [RequireOwner]
        [Command("getfile")]
        [Remarks("builds a list of known mp3s in a set directory")]
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
        [Remarks("Picks a random song and sends it known information to channel")]
        [Summary("Very much a work in progress")]
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
        [Remarks("Lists known songs")]
        [Summary("This feature is very much work in progress and not really used atm!")]
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

       */

        
        [RequireOwner]
        [Command("update")]
        [Remarks("Update command, only useable by Server owner!")]
        public async Task UpdateCommand()
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

        [Command("ping")]
        [Remarks("Tells you the delay or \"ping\" (in ms)between you and the bot ")]
        public async Task ping()
        {
            await Context.Channel.SendMessageAsync("Pong! (" + Global.Client.Latency + "ms)");
        }
        private async Task AddModuleEmbedField(ModuleInfo module, EmbedBuilder builder)
        {
            if (module == null) return;
            var descriptionBuilder = new List<string>();
            var duplicateChecker = new List<string>();
            foreach (var cmd in module.Commands)
            {
                var result = await cmd.CheckPreconditionsAsync(Context);
                if (!result.IsSuccess || duplicateChecker.Contains(cmd.Aliases.First())) continue;
                duplicateChecker.Add(cmd.Aliases.First());
                var cmdDescription = $"`{cmd.Aliases.First()}`";
                if (!string.IsNullOrEmpty(cmd.Summary))
                //    cmdDescription += $" | {cmd.Summary}";
                if (!string.IsNullOrEmpty(cmd.Remarks))
                    cmdDescription += $" | {cmd.Remarks}";
                if (cmdDescription != "``")
                    descriptionBuilder.Add(cmdDescription);
            }

            if (descriptionBuilder.Count <= 0) return;

            var moduleNotes = "";
            if (!string.IsNullOrEmpty(module.Summary))
                moduleNotes += $" {module.Summary}";
            if (!string.IsNullOrEmpty(module.Remarks))
                moduleNotes += $" {module.Remarks}";
            if (!string.IsNullOrEmpty(moduleNotes))
                moduleNotes += "\n";
            if (!string.IsNullOrEmpty(module.Name))
            {
                builder.AddField($"__**{module.Name}:**__",
                    $"{moduleNotes}" + string.Join("\n", descriptionBuilder) + $"\n{Constants.InvisibleString}");
            }
        }
        //borrowed from community bot "https://github.com/petrspelos/Community-Discord-BOT"
        
        [Command("help"), Alias("h")]
        [Remarks("Shows what a specific command does and what you need to put after the command.")]
        
        public async Task HelpQuery([Remainder] string query)
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Title = $"Help for '{query}'"
            };

            var result = _service.Search(Context, query);
            if (query.StartsWith("module "))
                query = query.Remove(0, "module ".Length);
            var emb = result.IsSuccess ? HelpCommand(result, builder) : await HelpModule(query, builder);

            if (emb.Fields.Length == 0)
            {
                await ReplyAsync($"Sorry, I couldn't find anything for \"{query}\".");
                return;
            }

            await Context.Channel.SendMessageAsync("", false, emb);
        }

        private static Embed HelpCommand(SearchResult search, EmbedBuilder builder)
        {
            foreach (var match in search.Commands)
            {
                var cmd = match.Command;
                var parameters = cmd.Parameters.Select(p => string.IsNullOrEmpty(p.Summary) ? p.Name : p.Summary);
                var paramsString = $"**Needed input**: {string.Join(", ", parameters)}" +
                                   (string.IsNullOrEmpty(cmd.Remarks) ? "" : $"\n**Remarks**: {cmd.Remarks}") +
                                   (string.IsNullOrEmpty(cmd.Summary) ? "" : $"\n**Summary**: {cmd.Summary}");

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = paramsString;
                    x.IsInline = false;
                });
            }

            return builder.Build();
        }

        private async Task<Embed> HelpModule(string moduleName, EmbedBuilder builder)
        {
            var module = _service.Modules.ToList().Find(mod =>
                string.Equals(mod.Name, moduleName, StringComparison.CurrentCultureIgnoreCase));
            await AddModuleEmbedField(module, builder);
            return builder.Build();
        }

    }
}