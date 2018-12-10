using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

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
            if (!(rawMessage is SocketUserMessage msg)) return;
            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;



            
            if ((msg.HasStringPrefix(Config.Bot.cmdPrefix, ref argPos) &&
                 (context.Guild == null || context.Guild.Id != 377879473158356992)) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _command.ExecuteAsync(context, argPos, _services);
                
                if (result.Error != null && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }

        }
    }
}
