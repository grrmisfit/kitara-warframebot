using System.Collections.Generic;
using LiteDB;
using Warframebot.Core.UserAccounts;

namespace Warframebot.Data
{
    public class DbStorage
    {
        public static UserAccount.GuildAccounts GetGuildInfo(ulong guildId)
        {
            using (var db = new LiteDatabase(@"Data\guilds.db"))
            {

                var servers = db.GetCollection<UserAccount.GuildAccounts>("guilds");

                var result = servers.FindOne(x => x.Guild == guildId);
                if (result == null)
                {
                    var newGuild = new UserAccount.GuildAccounts
                    {
                        
                        Guild = guildId,
                        AlertDelay = 15,

                        Rewards = new UserAccount.Rewards
                        {
                            WantedRewards = new List<string>
                            {
                                "none"
                            }
                        },
                        Fissures = new UserAccount.Fissures
                        {
                            WantedFissures = new List<string>
                            {
                                "none"
                            }
                           
                        },
                        News = new UserAccount.News
                        {
                            KnownNews = new List<string>
                            {
                                "none"
                            }
                        },

                        Alerts = new UserAccount.Alerts
                        {
                            KnownAlerts = new List<string>
                            {
                                "none"
                            }
                        },

                        PmLists = new UserAccount.PmLists
                        {
                            PmList = new List<string>
                            {
                                "none"
                            }
                         }
                        
                    };
                    servers.Insert(newGuild);
                    servers = db.GetCollection<UserAccount.GuildAccounts>("guilds");
                    result = servers.FindOne(x => x.Guild == guildId);
                }

                return result;
            }
        }

        public static void UpdateDb(ulong guildid, UserAccount.GuildAccounts guild)
        {
            using (var db = new LiteDatabase(@"Data\guilds.db"))
            {

                var guilds = db.GetCollection<UserAccount.GuildAccounts>("guilds");
                var results = guilds.FindOne(x => x.Guild == guildid);
                if (results == null)
                {
                    guilds.Insert(guild);
                    return;
                }

                guilds.Update(guild);
            }
        }

        public static UserAccount.AlarmUsers GetAlarmUser(ulong playerId)
        {
            using (var db = new LiteDatabase(@"Data\players.db"))
            {

                var user = db.GetCollection<UserAccount.AlarmUsers>("players");

                var result = user.FindOne(x => x.DiscordId == playerId) ?? CreateUser(playerId);
                return result;
            }
        }

        public static void UpdateAlarmuser(ulong playerId, UserAccount.AlarmUsers data)
        {
            using (var db = new LiteDatabase(@"Data\players.db"))
            {

                var user = db.GetCollection<UserAccount.AlarmUsers>("players");

                var result = user.FindOne(x => x.DiscordId == playerId);
                if (result == null)
                {
                    user.Insert(data);
                    return;
                }
                user.Update(data);
            }
        }

        public static UserAccount.AlarmUsers CreateUser(ulong id)
        {
            using (var db = new LiteDatabase(@"Data\players.db"))
            {

                var servers = db.GetCollection<UserAccount.AlarmUsers>("players");

                var create = new UserAccount.AlarmUsers();
                create.DiscordId = id;
                create.AlarmDelay = 15;
                create.AlarmOn = false;
                servers.Insert(create);
                var user = servers.FindOne(x => x.DiscordId == id);
                return user;
            }
        }

        public static IEnumerable<UserAccount.AlarmUsers> GetAlarmDb()
        {
            using (var db = new LiteDatabase(@"Data\players.db"))
            {

                var results = db.GetCollection<UserAccount.AlarmUsers>("players");
                
                var servers = results.FindAll();

                return servers;
            }
        }
        
        public static IEnumerable<UserAccount.GuildAccounts> GetDb()
        {
            using (var db = new LiteDatabase(@"Data\guilds.db"))
            {

                var results = db.GetCollection<UserAccount.GuildAccounts>("guilds");

                var servers = results.FindAll();

                return servers;
            }
        }
    }
}