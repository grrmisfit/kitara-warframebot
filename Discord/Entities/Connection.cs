using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Warframebot.Core;

namespace Warframebot.Discord.Entities
{
    public class Connection
    {

        private DiscordSocketClient _client;
        private DiscordLogger _logger;
        CommandHandler _handler;
        public Connection(DiscordLogger logger, DiscordSocketClient client)
        {
            _logger = logger;
            _client = client;
            
        }

        internal async Task ConnectAsync(WarframeBotConfig config)
        {
            
            if (string.IsNullOrEmpty(config.Token))
            {
                Console.WriteLine("Bot token not found, check config files!");
                return;
            }
            _client.Log += _logger.Log;
            
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            _client.Ready += RepeatingTimer.StartTimer;
            //_client.ReactionAdded += OnReactionAdded;
            Global.Client = _client;
            _handler = new CommandHandler();
            await _client.SetGameAsync("Warframe Helper");
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }
    }
}