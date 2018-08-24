using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Warframebot.Modules.Warframe
{
    public partial class Sorties
    {
        [JsonProperty("modifierTypes")]
        public Dictionary<string, string> ModifierTypes { get; set; }

        [JsonProperty("modifierDescriptions")]
        public Dictionary<string, string> ModifierDescriptions { get; set; }

        [JsonProperty("bosses")]
        public Dictionary<string, Boss> Bosses { get; set; }
    }

    public partial class Boss
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("faction")]
        public string Faction { get; set; }
    }
    public partial class Sorties
    {
        public static Sorties FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Sorties>(json, Convertered.Settings);
        }
    }

    public static class Serialized
    {
        public static string ToJson(this Sorties self) => JsonConvert.SerializeObject(self, Convertered.Settings);
    }

    internal static class Convertered
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
}


