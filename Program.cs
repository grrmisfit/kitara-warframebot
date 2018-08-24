using Discord.WebSocket;

using System.Threading.Tasks;
using Discord;

using Warframebot.Discord.Entities;
using Warframebot.Storage;


namespace Warframebot
{
    public class Program
    {
       
        

        private static async Task Main()
        {

            Unity.RegisterTypes();
            var storage = Unity.Resolve<IDataStorage>();
          
            var connection = Unity.Resolve<Connection>();


            await connection.ConnectAsync(new WarframeBotConfig
            {
                Token = Config.bot.token //storage.RestoreObject<string>("BotToken")
                
            });
            await Task.Delay(-1);
            // =>new Program().StartAsync().GetAwaiter().GetResult();
        }

       /* public async Task StartAsync()
        {
          
           
            
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
        */
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
 
    }
    }

