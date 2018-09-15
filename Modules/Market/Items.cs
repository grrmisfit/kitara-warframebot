// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Warframebot;
//
//    var items = Items.FromJson(jsonString);

namespace Warframebot.Modules.Market
{

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Items
    {
        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }

    public partial class Payload
    {
        [JsonProperty("items")]
        public ItemsClass Items { get; set; }
    }

    public partial class ItemsClass
    {
        [JsonProperty("en")]
        public En[] En { get; set; }
    }

    public partial class En
    {
        [JsonProperty("url_name")]
        public string UrlName { get; set; }

        [JsonProperty("item_name")]
        public string ItemName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Items
    {
        public static Items FromJson(string json) => JsonConvert.DeserializeObject<Items>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Items self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
}
