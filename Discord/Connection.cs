using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Warframebot.Core;
using Warframebot.Discord.Entities;


namespace Warframebot.Discord
{
    public class Connection
    {
       // private IServiceCollection _services;
        private  IServiceProvider _services;
        private  DiscordSocketClient _client;
        private readonly DiscordLogger _logger;
       // CommandHandler _handler;
        
        public Connection(DiscordLogger logger, DiscordSocketClient client)
        {
            _logger = logger;
            _client = client;
            
        }

        internal async Task ConnectAsync(WarframeBotConfig config)
        {
            //_provider = MyServices();
            thestart:
            if (string.IsNullOrEmpty(config.Token))
            {
                Console.WriteLine("Bot token not found, check config files!");
                return;
            }
            
            try
            {
                _client.Log += Log;
                
                await _client.LoginAsync(TokenType.Bot, Config.Bot.token);
               // _services = new ServiceCollection()
                    //.AddSingleton(_client)
                   // .AddSingleton(new InteractiveService(_client))
                   // .BuildServiceProvider();

                await _client.StartAsync();
                await Task.Delay(1000);
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
            
           // _client.Ready += RepeatingTimer.StartTimer;
            
            Global.Client = _client;
            //_handler = new CommandHandler();
            await _client.SetGameAsync("Warframe Helper");
           // await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }
      //  private static IServiceProvider MyServices()
      //  {
       //     return new ServiceCollection()
                
       // }
        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            LogFile(msg.ToString());
           await Task.Delay(1);
        }

        private async void LogFile(string message)
        {
            var fileName = $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}.log";
            var folder = Constants.LogFolder;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            StreamWriter sw = File.AppendText($"{folder}/{fileName}");
            sw.WriteLine($"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Hour}-{DateTime.Today.Minute}" + message);
            sw.Close();
            await Task.CompletedTask;
        }
    }
}