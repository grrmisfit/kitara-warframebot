using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Reflection;

namespace Warframebot
{


    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;

        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;


            if (context.Guild.Id is 377879473158356992)
            {
                if (msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
                {
                    var result = await _service.ExecuteAsync(context, argPos);
                    //if(!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    if (result.Error != null && result.Error != CommandError.UnknownCommand)
                    {
                        Console.WriteLine(result.ErrorReason);
                    }
                }

               
            }
                else
            {
                if (msg.HasMentionPrefix(_client.CurrentUser, ref argPos) || msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos))
                {
                    var result = await _service.ExecuteAsync(context, argPos);
                    //if(!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    if (result.Error != null && result.Error != CommandError.UnknownCommand)
                    {
                        Console.WriteLine(result.ErrorReason);
                    }
                }
            }  

        }
      
    }
    
}
