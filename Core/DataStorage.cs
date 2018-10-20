using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Warframebot.Core.UserAccounts;

namespace Warframebot.Core
{
    public static class DataStorage
    {


        // Save all userAccounts
        public static void SaveUserAccounts(IEnumerable<GuildAccounts> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        // Get all userAccounts
        public static IEnumerable<GuildAccounts> LoadUserAccounts(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GuildAccounts>>(json);
        }
        public static IEnumerable<UserAccount> LoadSavedAlarmSettings(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserAccount>>(json);
        }
        public static void SaveAlarmSettings(IEnumerable<UserAccount> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}