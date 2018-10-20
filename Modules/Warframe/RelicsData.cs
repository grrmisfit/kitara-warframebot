namespace Warframebot.Modules.Warframe
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class RelicsData
    {
        [JsonProperty("relics")]
        public List<Relic> Relics { get; set; }
    }

    public partial class Relic
    {
        [JsonProperty("tier")]
        public Tier Tier { get; set; }

        [JsonProperty("relicName")]
        public string RelicName { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("rewards")]
        public List<Reward> Rewards { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    public partial class Reward
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("itemName")]
        public string ItemName { get; set; }

        [JsonProperty("rarity")]
        public Rarity Rarity { get; set; }

        [JsonProperty("chance")]
        public double Chance { get; set; }
    }

    public enum Rarity { Rare, Uncommon };

    public enum State { Exceptional, Flawless, Intact, Radiant };

    public enum Tier { Axi, Lith, Meso, Neo };

    public partial class RelicsData
    {
        public static RelicsData FromJson(string json) => JsonConvert.DeserializeObject<RelicsData>(json, RConverter.Settings);
    }

    public static class RSerialize
    {
        public static string ToJson(this RelicsData self) => JsonConvert.SerializeObject(self, RConverter.Settings);
    }

    internal static class RConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                RarityConverter.Singleton,
                StateConverter.Singleton,
                TierConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class RarityConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Rarity) || t == typeof(Rarity?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Rare":
                    return Rarity.Rare;
                case "Uncommon":
                    return Rarity.Uncommon;
            }
            throw new Exception("Cannot unmarshal type Rarity");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Rarity)untypedValue;
            switch (value)
            {
                case Rarity.Rare:
                    serializer.Serialize(writer, "Rare");
                    return;
                case Rarity.Uncommon:
                    serializer.Serialize(writer, "Uncommon");
                    return;
            }
            throw new Exception("Cannot marshal type Rarity");
        }

        public static readonly RarityConverter Singleton = new RarityConverter();
    }

    internal class StateConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(State) || t == typeof(State?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Exceptional":
                    return State.Exceptional;
                case "Flawless":
                    return State.Flawless;
                case "Intact":
                    return State.Intact;
                case "Radiant":
                    return State.Radiant;
            }
            throw new Exception("Cannot unmarshal type State");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (State)untypedValue;
            switch (value)
            {
                case State.Exceptional:
                    serializer.Serialize(writer, "Exceptional");
                    return;
                case State.Flawless:
                    serializer.Serialize(writer, "Flawless");
                    return;
                case State.Intact:
                    serializer.Serialize(writer, "Intact");
                    return;
                case State.Radiant:
                    serializer.Serialize(writer, "Radiant");
                    return;
            }
            throw new Exception("Cannot marshal type State");
        }

        public static readonly StateConverter Singleton = new StateConverter();
    }

    internal class TierConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Tier) || t == typeof(Tier?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Axi":
                    return Tier.Axi;
                case "Lith":
                    return Tier.Lith;
                case "Meso":
                    return Tier.Meso;
                case "Neo":
                    return Tier.Neo;
            }
            throw new Exception("Cannot unmarshal type Tier");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Tier)untypedValue;
            switch (value)
            {
                case Tier.Axi:
                    serializer.Serialize(writer, "Axi");
                    return;
                case Tier.Lith:
                    serializer.Serialize(writer, "Lith");
                    return;
                case Tier.Meso:
                    serializer.Serialize(writer, "Meso");
                    return;
                case Tier.Neo:
                    serializer.Serialize(writer, "Neo");
                    return;
            }
            throw new Exception("Cannot marshal type Tier");
        }

        public static readonly TierConverter Singleton = new TierConverter();
    }
}
