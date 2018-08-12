using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using warframebot.Core;

namespace warframebot
{
    public class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;

        static void Main(string[] args)
        
           =>new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            
            if (Config.bot.token == "" || Config.bot.token == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose

            });
            _client.Log += Log;
            _client.Ready += RepeatingTimer.StartTimer;
            _client.ReactionAdded += OnReactionAdded;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            Global.Client = _client;
            _handler = new CommandHandler();
            await _client.SetGameAsync("Warframe Helper");
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
            
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if(reaction.MessageId == Global.MessageIdToTrack)
            {
                if(reaction.Emote.Name == "👌")
                {
                    await channel.SendMessageAsync(reaction.User.Value.Username + " says ok");  
                }
            }
            
        }

       
        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            
        }
       
    }
    }

