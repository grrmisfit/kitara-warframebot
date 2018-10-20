using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Api.V5.Models.Subscriptions;


namespace Warframebot.Modules
{

    public class Twitch : ModuleBase<SocketCommandContext>

    {
        internal static TwitchAPI api;

        [Command("check")]
        public async Task CheckTwitch()
        {
            api = new TwitchAPI();
            api.Settings.ClientId = "jhyfc3wnfdn5geh1qv7xqdy4e9n13n";
            api.Settings.AccessToken = "cnxc9jps0w281sn1hnvg18ltdu8g5t";
           
            bool isStreaming = await api.V5.Streams.BroadcasterOnlineAsync("accessiblegamer");
            if (isStreaming)
            {
                await Context.Channel.SendMessageAsync("yes");
            }
            else
            {
                await Context.Channel.SendMessageAsync("no");
            }
        }
    }
}