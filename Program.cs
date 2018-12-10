using System;
using System.IO;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Warframebot.Core;
using Microsoft.Extensions.DependencyInjection;

using Warframebot.Handler;
using Warframebot.Storage;


namespace Warframebot
{
    public class Program
    {
        
        static void Main(string[] args)

            => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            var cmdConfig = new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            };
            var services = new ServiceCollection()
                //.AddSingleton<DiscordSocketClient>()
                .AddSingleton(new CommandService(cmdConfig))
                .AddSingleton<CommandHandler>()
                .AddSingleton<InteractiveService>()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    MessageCacheSize = 20,
                    AlwaysDownloadUsers = true,
                    LogLevel = LogSeverity.Verbose

                }))
                .BuildServiceProvider();
            
            var client = services.GetRequiredService<DiscordSocketClient>();
            
            client.Log += Log;
            thestart:
            
               var  thetoken = Config.Bot.testtoken;
                var gameInfo = "Warframe Info Bot";
           
                if (string.IsNullOrEmpty(thetoken))
            {
                Console.WriteLine($"No Token found, check config. Bot will close own its own or close the window!");
              await  Task.Delay(6000000);
                return;
            }
            
            try
            {
               
                await client.LoginAsync(TokenType.Bot, thetoken);
                await client.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Connection Failed error {e}...Will retry in 5 secs.");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Retrying in {5 - i}");
                    await Task.Delay(1000);
                }

                goto thestart;
            }

            //var dataStore = Unity.Resolve<DataStore>();
            await services.GetRequiredService<CommandHandler>().InitializeAsync();
            
            client.Ready += RepeatingTimer.StartTimer;
            Global.Client = client;

            await client.SetGameAsync(gameInfo);
            
            await Task.Delay(-1);
            
        }
        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<InteractiveService>()
                
                .BuildServiceProvider();
        }

        private async Task Log(LogMessage msg)
        {
          
            Console.WriteLine(msg.Message);
            var fileName = $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}.log";
            var folder = Constants.LogFolder;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            
            StreamWriter sw = File.AppendText($"{folder}/{fileName}");
            sw.WriteLine($"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Hour}-{DateTime.Today.Minute}" + msg);
            sw.Close();
             await Task.CompletedTask;
        }
    }
}

