using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Warframebot.Core.UserAccounts
{
    public class UserAccount
    {
       public string Name { get; set; }
       public int Points { get; set; }
       public int GamesWon { get; set; }
    }

    public partial class GuildAccounts
    {
        [JsonProperty("Guild")]
        public ulong Guild { get; set; }

        [JsonProperty("AlertsChannel")]
        public ulong AlertsChannel { get; set; }

        [JsonProperty("CheckAlerts")]
        [JsonConverter(typeof(ParseStringConverter))]
        public bool CheckAlerts { get; set; }

        [JsonProperty("WantedRewards")]
        public List<string> WantedRewards { get; set; }

        [JsonProperty("CheckFissures")]
        [JsonConverter(typeof(ParseStringConverter))]
        public bool CheckFissures { get; set; }

        [JsonProperty("WantedFissures")]
        public List<string> WantedFissures { get; set; }

        [JsonProperty("TimeChecked")]
        public DateTime  TimeChecked { get; set; }
    }

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
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}

