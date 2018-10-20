using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Warframebot.Modules.Warframe
{
    public class MostlyHarmless : ModuleBase<SocketCommandContext>
    {
        private CommandService _service;

        public MostlyHarmless(CommandService service)
        {
            _service = service;
        }

        [Command("builds")]
        [Remarks("only used in a specific (Mostly Harmless) discord")]
        [Summary("Gives role Warframe Builds")]
        public async Task AddBuildsRole()
        {
            var user = Context.User as SocketGuildUser;
            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Warframe Builds");
            if (user != null && user.Roles.Contains(role))
            {
                await Context.Channel.SendMessageAsync("You have already completed this task Tenno!");
                return;
            }

            if (user != null)
            {
                await user.AddRoleAsync(role);
                await Context.Channel.SendMessageAsync($"You have been given role of {role}. Use !build warframe name to be pinged in proper channel");
            }
        }

        [Command("build")]
        [Remarks("Use to be pinged in proper channel for selected warframe.")]
        public  async Task PingBuildChannel([Remainder] string msg)
        {
            var guild = Context.Guild.Channels;
            foreach (var chan in guild)
            {
                if (chan.Name.ToLower() == msg.ToLower())
                {
                    IUserMessage m = await ReplyAsync("I will ping you in the proper channel Tenno.");
                    await Task.Delay(1000);
                    //if (Global.Client.GetChannel(chan.Id) is IMessageChannel chnl) await chnl.SendMessageAsync($"<@{Context.User.Id}>");
                    var channel = Global.Client.GetChannel(chan.Id) as SocketTextChannel;
                    var d = await channel.SendMessageAsync($"<@{Context.User.Id}>");
                    await Task.Delay(10000);
                    await m.DeleteAsync();
                    await d.DeleteAsync();
                    return;
                }
               
                    
                
            }
            await Context.Channel.SendMessageAsync(
                "Build channel not found! Check spelling and try again or contact an Officer.");
        }
        [Command("freddo")]
        [Remarks("only used in a specific discord")]
        [Summary("Gives you ability to see bot spam channel. Use !freddo off to disable.")]
        public async Task AddFreddoRole([Remainder] string msg = "")
        {
            var user = Context.User as SocketGuildUser;
            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Alliance Friends");
            if (user != null && user.Roles.Contains(role) && msg == "")
            {
                await Context.Channel.SendMessageAsync("You have already completed this task Tenno!");
                return;
            }
            if (user != null && !user.Roles.Contains(role) && msg == "off")
            {
                await Context.Channel.SendMessageAsync("You have already completed this task Tenno!");
                return;
            }

            if (msg == "off")
            {
                if (user != null && user.Roles.Contains(role)) await user.RemoveRoleAsync(role);
                await Context.Channel.SendMessageAsync($"Your role of {role} has been removed");
            }
            else if (msg == "")
            {
                if (user != null) await user.AddRoleAsync(role);
                await Context.Channel.SendMessageAsync($"You have been given role of {role}");
            }
        }

        [Command("stream")]
        [Remarks("used to give Stream Notifications role")]
        [Summary("Gives you ability to see stream channel. Use !stream off to disable.")]
        public async Task AddStreamRole([Remainder] string msg = "")
        {
            var user = Context.User as SocketGuildUser;
            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Stream Notifications");
            if (user != null && user.Roles.Contains(role) && msg == "")
            {
                await Context.Channel.SendMessageAsync("You have already completed this task Tenno!");
                return;
            }
            if (user != null && !user.Roles.Contains(role) && msg == "off")
            {
                await Context.Channel.SendMessageAsync("You have already completed this task Tenno!");
                return;
            }

            if (msg == "off")
            {
                if (user != null && user.Roles.Contains(role)) await user.RemoveRoleAsync(role);
                await Context.Channel.SendMessageAsync($"Your role of {role} has been removed");
            }
            else if (msg == "")
            {
                if (user != null) await user.AddRoleAsync(role);
                await Context.Channel.SendMessageAsync($"You have been given role of {role}");
            }
        }
        [Command("symbol")]
        [Summary("The symbol of our great alliance, sent in all its glory to the channel for all to see!")]
        public async Task TestDb()
        {
            var embed = new EmbedBuilder();
            embed.WithImageUrl("http://3rdshifters.org/dur.png");
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }


    }
}