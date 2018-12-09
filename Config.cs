
using System.IO;
using Newtonsoft.Json;

namespace Warframebot
{
   public class Config
    {
        private const string configFolder = "Resources";
        private const string configFile = "config.json";

      public static BotConfig Bot;
        static Config()
            {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            if (!File.Exists(configFolder + "/" + configFile))
            
            {

                Bot = new BotConfig();
                string json = JsonConvert.SerializeObject(Bot, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + configFile);
                Bot = JsonConvert.DeserializeObject<BotConfig>(json);
               
            }
            }
    }

    public struct BotConfig
    {

        public string token;
        public string testtoken;
        public string cmdPrefix;
        public ulong ownerId;
        public string DropBoxToken;
    }
   
}
