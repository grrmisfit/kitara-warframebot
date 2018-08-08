using warframebot.Core.UserAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
namespace warframebot.Core
{
    public static class DataStorage
    {
        
      
        // Save all userAccounts
        public static void SaveUserAccounts(IEnumerable<GetPlayerOnlineResult> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        // Get all userAccounts
        public static IEnumerable<GetPlayerOnlineResult> LoadUserAccounts(string filePath)
        {
            if(!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GetPlayerOnlineResult>>(json);
        }
        public static IEnumerable<GetPlayerOnlineResult> LoadSavedAccounts(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GetPlayerOnlineResult>>(json);
        }
        public static void SaveTmpAccounts(IEnumerable<GetPlayerOnlineResult> accounts, string filePath)
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
