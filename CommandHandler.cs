using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Reflection;
using Warframebot.Core;
using Warframebot.Core.UserAccounts;
using Warframebot.Modules;

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

               // return;
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

            string currentWord = ScramData.ScramWord;
           
            currentWord = currentWord.ToLower();
           if (msg.Content == currentWord)
            {
                await Misc.SendMessageChannel(ScramData.ScramChannel, context.User.Username + " got the correct answer!");
                
                var account = UserAccounts.GetAccount(context.User.Username);
                account.Points += currentWord.Length;
                
                UserAccounts.SaveAccounts();
                ScramData.WordGuessed = true;
                ScramData.GamePause = true;
                ScramData.GameWait = true;
                await Misc.SendMessageChannel(ScramData.ScramChannel, "**" + "New word coming in 10 secs!" + "**");
                RepeatingTimer.DelayScramTimer();
                
            }
           
            

        }
       /* public async Task CatchScramWord(SocketMessage msgs)
        {
            var msg = msgs as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            int argPos = 0;
            if (msgs.Content == null) return;
           
            
            string currentWord = ScramData.ScramWord;
            currentWord = currentWord.ToLower();
            if ( msgs.Content == currentWord )
            {
                await Misc.SendMessageChannel(471312780079923210, context.User.Username);
            }
              
        }*/
    }
    
}
