
using Discord;
using Discord.WebSocket;

namespace Warframebot.Discord
{
    public static class SocketConfig
    {

        public static  DiscordSocketConfig GetDefault()
        {
            return new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Error
            };
        }

        public static DiscordSocketConfig GetNew()
        {
            return new DiscordSocketConfig();

        }
    }
}