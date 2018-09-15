using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
namespace discordbot.Modules
{
   public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Echoed message");
            embed.WithDescription(message);
            embed.WithColor(new Color(0, 255, 0));
                

            await Context.Channel.SendMessageAsync("", false,  embed.Build());
        }
    }
}
