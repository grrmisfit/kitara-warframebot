using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Warframebot.Core.UserAccounts
{
    public class UserAccount
    {
       
       public ulong DiscordId { get; set; }
       public string DiscordName { get; set; }
       public int AlarmDelay { get; set; }
       public bool AlarmOn { get; set; }
       public DateTime TimeAlerted { get; set; }
       public ulong AlarmChannel { get; set; }
    
    }

    public class Fissures
    {
        public IList<string> WantedFissures { get; set; }
        
    }

    public class Rewards
    {
        public IList<string> WantedRewards { get; set; }
    }
    public  class GuildAccounts
    {
        public int Id { get; set; }

        
        public ulong Guild { get; set; }

       
        public ulong AlertsChannel { get; set; }

       
        public bool CheckAlerts { get; set; }

        public Rewards WantedRewards { get; set; }

        public Fissures Fissures { get; set; }
        

        public bool CheckFissures { get; set; }

        public string WantedFissures { get; set; }

        public DateTime AlertTimeChecked { get; set; }
        
        public DateTime  TimeChecked { get; set; }
        
        public DateTime CetusTimeChecked { get; set; }
       
        public int AlertDelay { get; set; }
        
        public bool CetusTime { get; set; }
       
        public bool Cetus15TimeAlerted { get; set; }
       
        public bool Cetus5TimeAlerted { get; set; }
        
        public List<string> KnownFissures { get; set; }
        
        public List<string> KnownAlerts { get; set; }
       
        public bool NotifyAlerts { get; set; }
        
        public List<string> PmList { get; set; }
        
        public List<string> KnownNews { get; set; }
        
        public bool NotifyNews { get; set; }
    }
    /*
    public partial class GuildAccounts
    {
        public static List<GuildAccounts> FromJson(string json) => JsonConvert.DeserializeObject<List<GuildAccounts>>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<GuildAccounts> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            bool b;
            if (Boolean.TryParse(value, out b))
            {
                return b;
            }
            throw new Exception("Cannot unmarshal type bool");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
            
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }*/
}

