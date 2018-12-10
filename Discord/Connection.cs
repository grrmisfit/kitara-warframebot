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
       
        private  IServiceProvider _services;
        private  DiscordSocketClient _client;
        private readonly DiscordLogger _logger;
       
        
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
                _client.Log += Log;
                
                await _client.LoginAsync(TokenType.Bot, Config.Bot.token);
               

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
            
           
            
            Global.Client = _client;
            
            await _client.SetGameAsync("Warframe Helper");
           
            await Task.Delay(-1);
        }
      
        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            LogFile(msg.ToString());
           await Task.Delay(1);
        }

        public static async void LogFile(string message)
        {
            var fileName = $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}.log";
            var folder = Constants.LogFolder;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            StreamWriter sw = File.AppendText($"{folder}/{fileName}");
            sw.WriteLine($"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Hour}-{DateTime.Today.Minute}" + message);
            sw.Close();
            await Task.CompletedTask;
        }
    }
}