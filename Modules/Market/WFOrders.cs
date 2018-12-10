// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Warframebot;
//
//    var wfOrders = WfOrders.FromJson(jsonString);

using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Warframebot.Modules.Market
{
    public partial class WFOrders
    {
        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }

    public partial class Payload
    {
        [JsonProperty("orders")]
        public List<Order> Orders { get; set; }
    }

    public partial class Order
    {
        [JsonProperty("visible")]
        public bool Visible { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("creation_date")]
        public DateTimeOffset CreationDate { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("last_update")]
        public DateTimeOffset LastUpdate { get; set; }

        [JsonProperty("platinum")]
        public long Platinum { get; set; }

        [JsonProperty("order_type")]
        public OrderType OrderType { get; set; }

        [JsonProperty("region")]
        public Region Region { get; set; }

        [JsonProperty("platform")]
        public Platform Platform { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class User
    {
        [JsonProperty("ingame_name")]
        public string IngameName { get; set; }

        [JsonProperty("last_seen")]
        public DateTimeOffset? LastSeen { get; set; }

        [JsonProperty("reputation_bonus")]
        public long ReputationBonus { get; set; }

        [JsonProperty("reputation")]
        public long Reputation { get; set; }

        [JsonProperty("region")]
        public Region Region { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }
    }

    public enum OrderType { Buy, Sell };

    public enum Platform { Pc };

    public enum Region { En, Fr, Ko, Ru, Sv };

    public enum Status { Ingame, Offline, Online };

    public partial class WFOrders
    {
        public static WFOrders FromJson(string json) => JsonConvert.DeserializeObject<WFOrders>(json, WFConverter.Settings);
    }

    public static class WfSerialize
    {
        public static string ToJson(this WFOrders self) => JsonConvert.SerializeObject((object) self, (JsonSerializerSettings) WFConverter.Settings);
    }

    internal static class WFConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                OrderTypeConverter.Singleton,
                PlatformConverter.Singleton,
                RegionConverter.Singleton,
                StatusConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class OrderTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(OrderType) || t == typeof(OrderType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "buy":
                    return OrderType.Buy;
                case "sell":
                    return OrderType.Sell;
            }
            throw new Exception("Cannot unmarshal type OrderType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (OrderType)untypedValue;
            switch (value)
            {
                case OrderType.Buy:
                    serializer.Serialize(writer, "buy");
                    return;
                case OrderType.Sell:
                    serializer.Serialize(writer, "sell");
                    return;
                default:
                    return;
            }
            
        }

        public static readonly OrderTypeConverter Singleton = new OrderTypeConverter();
    }

    internal class PlatformConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Platform) || t == typeof(Platform?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "pc")
            {
                return Platform.Pc;
            }
            throw new Exception("Cannot unmarshal type Platform");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Platform)untypedValue;
            if (value == Platform.Pc)
            {
                serializer.Serialize(writer, "pc");
                return;
            }
            throw new Exception("Cannot marshal type Platform");
        }

        public static readonly PlatformConverter Singleton = new PlatformConverter();
    }

    internal class RegionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Region) || t == typeof(Region?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "en":
                    return Region.En;
                case "fr":
                    return Region.Fr;
                case "ko":
                    return Region.Ko;
                case "ru":
                    return Region.Ru;
                case "sv":
                    return Region.Sv;
                    default:
                        return Region.En;
            }
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Region)untypedValue;
            switch (value)
            {
                case Region.En:
                    serializer.Serialize(writer, "en");
                    return;
                case Region.Fr:
                    serializer.Serialize(writer, "fr");
                    return;
                case Region.Ko:
                    serializer.Serialize(writer, "ko");
                    return;
                case Region.Ru:
                    serializer.Serialize(writer, "ru");
                    return;
                case Region.Sv:
                    serializer.Serialize(writer, "sv");
                    return;
                default:
                    serializer.Serialize(writer, "en");
                    return;
            }
        }

        public static readonly RegionConverter Singleton = new RegionConverter();
    }

    internal class StatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Status) || t == typeof(Status?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "ingame":
                    return Status.Ingame;
                case "offline":
                    return Status.Offline;
                case "online":
                    return Status.Online;
                    default:
                        return Status.Offline;
            }
           
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Status)untypedValue;
            switch (value)
            {
                case Status.Ingame:
                    serializer.Serialize(writer, "ingame");
                    return;
                case Status.Offline:
                    serializer.Serialize(writer, "offline");
                    return;
                case Status.Online:
                    serializer.Serialize(writer, "online");
                    return;
                default:
                        serializer.Serialize(writer, "offline");
                    return;
            }
           
        }

        public static readonly StatusConverter Singleton = new StatusConverter();
    }
}
