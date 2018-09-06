﻿using System;
using System.Threading;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;
using Warframebot.Core;
using Dropbox.Api;
using Warframebot.Discord.Entities;

namespace Warframebot
{
    public class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;
        /*
        static void Main(string[] args)
           
            => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            thestart:
            await Task.Delay(3000);
            if (string.IsNullOrEmpty(Config.bot.token)) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose

            });
            
            
            try
            {
                await _client.LoginAsync(TokenType.Bot, Config.bot.token);
                await _client.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection Failed...Will retry in 5 secs.");
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Retrying in {5 - i}");
                  await  Task.Delay(1000);
                }
                goto thestart;
            }
            _client.Log += Log;
            _client.Ready += RepeatingTimer.StartTimer;
            Global.Client = _client;
            _handler = new CommandHandler();
            await _client.SetGameAsync("Warframe Info Bot");
            await _handler.InitializeAsync(_client);


            await Task.Delay(-1);
            Console.Read();
        }
        */

        private static async Task Main()
       {

           Unity.RegisterTypes();
          // var storage = Unity.Resolve<IDataStorage>();

           var connection = Unity.Resolve<Connection>();
          

           await connection.ConnectAsync(new WarframeBotConfig
           {
               Token = Config.bot.token //storage.RestoreObject<string>("BotToken")

           });
           await Task.Delay(-1);

       }
        /*
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
        
        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);

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
    }
}

