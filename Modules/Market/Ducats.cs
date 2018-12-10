// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Warframebot;
//
//    var ducats = Ducats.FromJson(jsonString);

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Warframebot.Modules.Market
{
    public class DucatList
    {
        public string ItemName  { get; set; }
        public int DucatPrice { get; set; }

    }
    public partial class Ducats
    {
        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }

    public partial class Payload
    {
        [JsonProperty("previous_hour")]
        public List<Previous> PreviousHour { get; set; }

        [JsonProperty("previous_day")]
        public List<Previous> PreviousDay { get; set; }
    }

    public class Previous
    {
        [JsonProperty("datetime")]
        public DateTimeOffset Datetime { get; set; }

        [JsonProperty("position_change_month")]
        public long PositionChangeMonth { get; set; }

        [JsonProperty("position_change_week")]
        public long PositionChangeWeek { get; set; }

        [JsonProperty("position_change_day")]
        public long PositionChangeDay { get; set; }

        [JsonProperty("plat_worth")]
        public double PlatWorth { get; set; }

        [JsonProperty("volume")]
        public long Volume { get; set; }

        [JsonProperty("ducats_per_platinum")]
        public double DucatsPerPlatinum { get; set; }

        [JsonProperty("ducats_per_platinum_wa")]
        public double DucatsPerPlatinumWa { get; set; }

        [JsonProperty("ducats")]
        public long Ducats { get; set; }

        [JsonProperty("item")]
        public string Item { get; set; }

        [JsonProperty("median")]
        public double Median { get; set; }

        [JsonProperty("wa_price")]
        public double WaPrice { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Ducats
    {
        public static Ducats FromJson(string json) => JsonConvert.DeserializeObject<Ducats>(json, DConverter.Settings);
    }

    public static class DSerialize
    {
        public static string ToJson(this Ducats self) => JsonConvert.SerializeObject((object) self, (JsonSerializerSettings) DConverter.Settings);
    }

    internal static class DConverter
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
