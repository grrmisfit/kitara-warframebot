using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Warframebot.Core;

namespace Warframebot.Discord.Entities
{
    public class Connection
    {

        private readonly DiscordSocketClient _client;
        private readonly DiscordLogger _logger;
        CommandHandler _handler;

        public Connection(DiscordLogger logger, DiscordSocketClient client)
        {
            _logger = logger;
            _client = client;
            
        }

        internal async Task ConnectAsync(WarframeBotConfig config)
        {
            thestart:
            if (string.IsNullOrEmpty(config.Token))
            {
                Console.WriteLine("Bot token not found, check config files!");
                return;
            }
           
            try
            {
                _client.Log += _logger.Log;
                await _client.LoginAsync(TokenType.Bot, Config.bot.token);
                await _client.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection Failed...Will retry in 5 secs.");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Retrying in {5 - i}");
                    await Task.Delay(1000);
                }
                goto thestart;
            }
           // await _client.LoginAsync(TokenType.Bot, Config.bot.token);
           // await _client.StartAsync();
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