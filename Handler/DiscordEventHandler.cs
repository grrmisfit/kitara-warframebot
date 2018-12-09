using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Warframebot.Handler
{
    public class DiscordEventHandler
    {
        /// <summary>
        /// Put your subscriptions to events here!
        /// Just one non awaited async Method per functionality you want to provide 
        /// </summary>
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        
            private readonly DiscordSocketClient _client;
            private readonly CommandHandler _commandHandler;
            
           
           // private readonly Logger _logger;
           

            public DiscordEventHandler( DiscordSocketClient client, CommandHandler commandHandler)
            {
               // _logger = logger;
                _client = client;
                _commandHandler = commandHandler;
                
            }

            public void InitDiscordEvents()
            {
                _client.ChannelCreated += ChannelCreated;
                _client.ChannelDestroyed += ChannelDestroyed;
                _client.ChannelUpdated += ChannelUpdated;
                _client.Connected += Connected;
                _client.CurrentUserUpdated += CurrentUserUpdated;
                _client.Disconnected += Disconnected;
                _client.GuildAvailable += GuildAvailable;
                _client.GuildMembersDownloaded += GuildMembersDownloaded;
                _client.GuildMemberUpdated += GuildMemberUpdated;
                _client.GuildUnavailable += GuildUnavailable;
                _client.GuildUpdated += GuildUpdated;
                _client.JoinedGuild += JoinedGuild;
                _client.LatencyUpdated += LatencyUpdated;
                _client.LeftGuild += LeftGuild;
                _client.Log += Log;
                _client.LoggedIn += LoggedIn;
                _client.LoggedOut += LoggedOut;
                _client.MessageDeleted += MessageDeleted;
                _client.MessageReceived += MessageReceived;
                _client.MessageUpdated += MessageUpdated;
                _client.ReactionAdded += ReactionAdded;
                _client.ReactionRemoved += ReactionRemoved;
                _client.ReactionsCleared += ReactionsCleared;
                _client.Ready += Ready;
                _client.RecipientAdded += RecipientAdded;
                _client.RecipientRemoved += RecipientRemoved;
                _client.RoleCreated += RoleCreated;
                _client.RoleDeleted += RoleDeleted;
                _client.RoleUpdated += RoleUpdated;
                _client.UserBanned += UserBanned;
                _client.UserIsTyping += UserIsTyping;
                _client.UserJoined += UserJoined;
                _client.UserLeft += UserLeft;
                _client.UserUnbanned += UserUnbanned;
                _client.UserUpdated += UserUpdated;
                _client.UserVoiceStateUpdated += UserVoiceStateUpdated;

                // THIS ONE IS AN EXCEPTION!
                // I don't know how we should handle contidional 
                // subscription to an event otherwise...
                
            }

            private async Task ChannelCreated(SocketChannel channel)
            {
            // not used yet
        }

        private async Task ChannelDestroyed(SocketChannel channel)
            {
            // not used yet
        }

        private async Task ChannelUpdated(SocketChannel channelBefore, SocketChannel channelAfter)
            {
                // not used yet
        }

        private async Task Connected()
            {
                // not used yet
        }

        private async Task CurrentUserUpdated(SocketSelfUser userBefore, SocketSelfUser userAfter)
            {
                // not used yet
        }

        private async Task Disconnected(Exception exception)
            {
                // not used yet
        }

        private async Task GuildAvailable(SocketGuild guild)
            {

                // not used yet
        }

        private async Task GuildMembersDownloaded(SocketGuild guild)
            {
            //not uset yet
            }

            private async Task GuildMemberUpdated(SocketGuildUser userBefore, SocketGuildUser userAfter)
            {
            //not uset yet
        }

        private async Task GuildUnavailable(SocketGuild guild)
            {
                //not uset yet
        }

        private async Task GuildUpdated(SocketGuild guildBefore, SocketGuild guildAfter)
            {
                //not uset yet
        }

        private async Task JoinedGuild(SocketGuild guild)
            {
              //  ServerBots.JoinedGuild(guild);
            }

            private async Task LatencyUpdated(int latencyBefore, int latencyAfter)
            {
                //not uset yet
        }

        private async Task LeftGuild(SocketGuild guild)
            {
                //not uset yet
        }

        private async Task Log(LogMessage logMessage)
            {
                //not uset yet
        }

        private async Task LoggedIn()
            {
                //not uset yet
        }

        private async Task LoggedOut()
            {
                //not uset yet
        }

        private async Task MessageDeleted(Cacheable<IMessage, ulong> cacheMessage, ISocketMessageChannel channel)
            {
            //not uset yet
        }

        private async Task MessageReceived(SocketMessage message)
            {
                _commandHandler.HandleCommandAsync(message);
             
            }

            private async Task MessageUpdated(Cacheable<IMessage, ulong> cacheMessageBefore, SocketMessage messageAfter, ISocketMessageChannel channel)
            {
            //not uset yet  
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> cacheMessage, ISocketMessageChannel channel, SocketReaction reaction)
            {

            //not uset yet
        }

        private async Task ReactionRemoved(Cacheable<IUserMessage, ulong> cacheMessage, ISocketMessageChannel channel, SocketReaction reaction)
            {
                //not uset yet
        }

        private async Task ReactionsCleared(Cacheable<IUserMessage, ulong> cacheMessage, ISocketMessageChannel channel)
            {
                //not uset yet
        }

        private async Task Ready()
            {

            //not uset yet
        }

        private async Task RecipientAdded(SocketGroupUser user)
            {
                //not uset yet
        }

        private async Task RecipientRemoved(SocketGroupUser user)
            {
                //not uset yet
        }

        private async Task RoleCreated(SocketRole role)
            {
                //not uset yet
        }

        private async Task RoleDeleted(SocketRole role)
            {
            //not uset yet
        }

        private async Task RoleUpdated(SocketRole roleBefore, SocketRole roleAfter)
            {
            //not uset yet
        }

        private async Task UserBanned(SocketUser user, SocketGuild guild)
            {
                //not uset yet
        }

        private async Task UserIsTyping(SocketUser user, ISocketMessageChannel channel)
            {
                //not uset yet
        }

        private async Task UserJoined(SocketGuildUser user)
            {
            //not uset yet

        }

        private async Task UserLeft(SocketGuildUser user)
            {
            //not uset yet
        }

        private async Task UserUnbanned(SocketUser user, SocketGuild guild)
            {
                //not uset yet
        }

        private async Task UserUpdated(SocketUser user, SocketUser guild)
            {
                //not uset yet
        }

        private async Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState voiceStateBefore, SocketVoiceState voiceStateAfter)
            {
                //not uset yet
        }
    }
    }
