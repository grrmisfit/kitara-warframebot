using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Warframebot.Handler
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _command;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services, CommandService command, DiscordSocketClient client)
        {

            _command = command;
            _services = services;
            _client = client;
        }

        public async Task InitializeAsync()
        {
            await _command.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _services);
            Global.Client = _client;
            _client.MessageReceived += HandleCommandAsync;
        }


        public async Task HandleCommandAsync(SocketMessage rawMessage)
        {
            var msg = rawMessage as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;



            /* if (context.Guild.Id is 377879473158356992)
             {
                 if (msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
                 {
                     var result = await _command.ExecuteAsync(context, argPos, _services);
                     //if(!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                     if (result.Error != null && result.Error != CommandError.UnknownCommand)
                     {
                         Console.WriteLine(result.ErrorReason);
                     }
                 }
 
 
             }
             else
             {*/
            if (msg.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
                msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos))
            {
                var result = await _command.ExecuteAsync(context, argPos, _services);
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
}
