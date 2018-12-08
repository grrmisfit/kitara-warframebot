using System.Collections.Generic;
using LiteDB;
using Warframebot.Core.UserAccounts;

namespace Warframebot.Data
{
    public class DbStorage
    {
        public static GuildAccounts GetGuildInfo(ulong guildId)
        {
            using (var db = new LiteDatabase(@"Data\guilds.db"))
            {

                var servers = db.GetCollection<GuildAccounts>("guilds");

                var result = servers.FindOne(x => x.Guild == guildId);
                if (result == null)
                {
                    var newGuild = new GuildAccounts();
                    newGuild.Guild = guildId;
                    servers.Insert(newGuild);
                    
                    result = servers.FindOne(x => x.Guild == guildId);
                }

                return result;
            }
        }

        public static void UpdateDb(ulong guildid, GuildAccounts guild)
        {
            using (var db = new LiteDatabase(@"Data\guilds.db"))
            {

                var guilds = db.GetCollection<GuildAccounts>("guilds");
                var results = guilds.FindOne(x => x.Guild == guildid);
                if (results == null)
                {
                    guilds.Insert(guild);
                    return;
                }

                guilds.Update(guild);
            }
        }

        public static UserAccount GetPlayerInfo(ulong playerId)
        {
            using (var db = new LiteDatabase(@"Data\players.db"))
            {

                var servers = db.GetCollection<UserAccount>("players");

                var result = servers.FindById(playerId);

                return result;
            }
        }

        
        public static IEnumerable<GuildAccounts> GetDb()
        {
            using (var db = new LiteDatabase(@"Data\guilds.db"))
            {

                var results = db.GetCollection<GuildAccounts>("guilds");

                var servers = results.FindAll();

                return servers;
            }
        }
    }
}