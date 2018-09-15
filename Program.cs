using System;

using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Warframebot.Core;
using Microsoft.Extensions.DependencyInjection;

using Warframebot.Handler;


namespace Warframebot
{
    public class Program
    {
        
        static void Main(string[] args)

            => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            var services = ConfigureServices();

            var client = services.GetRequiredService<DiscordSocketClient>();
          
            client.Log += Log;
            thestart:
            //await Task.Delay(3000);
           // var cmdConfig = new CommandServiceConfig
           // {
          //      DefaultRunMode = RunMode.Async

           // };
            if (string.IsNullOrEmpty(Config.bot.token)) return;
            //client = new DiscordSocketClient(new DiscordSocketConfig
            //{
            //    LogLevel = LogSeverity.Verbose

           // });

            try
            {
                await client.LoginAsync(TokenType.Bot, Config.bot.token);
                await client.StartAsync();
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

           // services = new ServiceCollection()
             //   .AddSingleton(client)
              //  .AddSingleton<InteractiveService>()
               // .AddSingleton<CommandService>()
               // //.BuildServiceProvider();
//
          //  cmdcommands = new CommandService(cmdConfig);
           // await cmdcommands.AddModulesAsync(Assembly.GetEntryAssembly());

            await services.GetRequiredService<CommandHandler>().InitializeAsync();



            //client.Log += Log;
            client.Ready += RepeatingTimer.StartTimer;
            Global.Client = client;

            await client.SetGameAsync("Warframe Info Bot");



            await Task.Delay(-1);
            Console.Read();
        }
        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<InteractiveService>()
                
                .BuildServiceProvider();
        }

        /*
        public async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(client, msg);
            int argPos = 0;



            if (context.Guild.Id is 377879473158356992)
            {
                if (msg.HasMentionPrefix(client.CurrentUser, ref argPos))
                {
                    var result = await cmdcommands.ExecuteAsync(context, argPos, services);
                    //if(!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    if (result.Error != null && result.Error != CommandError.UnknownCommand)
                    {
                        Console.WriteLine(result.ErrorReason);
                    }
                }


            }
            else
            {
                if (msg.HasMentionPrefix(client.CurrentUser, ref argPos) || msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos))
                {
                    var result = await cmdcommands.ExecuteAsync(context, argPos, services);
                    //if(!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    if (result.Error != null && result.Error != CommandError.UnknownCommand)
                    {
                        Console.WriteLine(result.ErrorReason);
                    }
                }
            }

            //var context = new SocketCommandContext(client, msg);
            //await commands.ExecuteAsync(context, argPos, services);
        }


                private static async Task Main()
               {

                   Unity.RegisterTypes();
                  // var storage = Unity.Resolve<IDataStorage>();

                   var connection = Unity.Resolve<Connection>();


                   await connection.ConnectAsync(new WarframeBotConfig
                   {
                       Token = Config.bot.token //storage.RestoreObject<string>("BotToken")

                   });
                   await Task.Delay(1000);

               }

               public async Task StartAsync()
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
    */
        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);

        }
    }
}

