using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace warframebot.Modules.Warframe
{
    public partial class Warframe
    {
        [JsonProperty("WorldSeed")]
        public string WorldSeed { get; set; }

        [JsonProperty("Version")]
        public long Version { get; set; }

        [JsonProperty("MobileVersion")]
        public string MobileVersion { get; set; }

        [JsonProperty("BuildLabel")]
        public string BuildLabel { get; set; }

        [JsonProperty("Time")]
        public long Time { get; set; }

        [JsonProperty("Date")]
        public long Date { get; set; }

        [JsonProperty("Events")]
        public List<Event> Events { get; set; }

        [JsonProperty("Goals")]
        public List<object> Goals { get; set; }

        [JsonProperty("Alerts")]
        public List<Alert> Alerts { get; set; }

        [JsonProperty("Sorties")]
        public List<Sorty> Sorties { get; set; }

        [JsonProperty("SyndicateMissions")]
        public List<SyndicateMission> SyndicateMissions { get; set; }

        [JsonProperty("ActiveMissions")]
        public List<ActiveMission> ActiveMissions { get; set; }

        [JsonProperty("GlobalUpgrades")]
        public List<object> GlobalUpgrades { get; set; }

        [JsonProperty("FlashSales")]
        public List<FlashSale> FlashSales { get; set; }

        [JsonProperty("Invasions")]
        public List<Invasion> Invasions { get; set; }

        [JsonProperty("HubEvents")]
        public List<object> HubEvents { get; set; }

        [JsonProperty("NodeOverrides")]
        public List<NodeOverride> NodeOverrides { get; set; }

        [JsonProperty("BadlandNodes")]
        public List<BadlandNode> BadlandNodes { get; set; }

        [JsonProperty("VoidTraders")]
        public List<VoidTrader> VoidTraders { get; set; }

        [JsonProperty("PrimeAccessAvailability")]
        public PrimeAvailability PrimeAccessAvailability { get; set; }

        [JsonProperty("PrimeVaultAvailabilities")]
        public List<PrimeAvailability> PrimeVaultAvailabilities { get; set; }

        [JsonProperty("DailyDeals")]
        public List<DailyDeal> DailyDeals { get; set; }

        [JsonProperty("LibraryInfo")]
        public LibraryInfo LibraryInfo { get; set; }

        [JsonProperty("PVPChallengeInstances")]
        public List<PvpChallengeInstance> PvpChallengeInstances { get; set; }

        [JsonProperty("PersistentEnemies")]
        public List<object> PersistentEnemies { get; set; }

        [JsonProperty("PVPAlternativeModes")]
        public List<object> PvpAlternativeModes { get; set; }

        [JsonProperty("PVPActiveTournaments")]
        public List<object> PvpActiveTournaments { get; set; }

        [JsonProperty("ProjectPct")]
        public List<double> ProjectPct { get; set; }

        [JsonProperty("ConstructionProjects")]
        public List<object> ConstructionProjects { get; set; }

        [JsonProperty("TwitchPromos")]
        public List<object> TwitchPromos { get; set; }
    }

    public partial class ActiveMission
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("Region")]
        public long Region { get; set; }

        [JsonProperty("Seed")]
        public long Seed { get; set; }

        [JsonProperty("Activation")]
        public Activation Activation { get; set; }

        [JsonProperty("Expiry")]
        public Activation Expiry { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("MissionType")]
        public string MissionType { get; set; }

        [JsonProperty("Modifier")]
        public string Modifier { get; set; }
    }

    public partial class Activation
    {
        [JsonProperty("$date")]
        public Date Date { get; set; }
    }

    public partial class Date
    {
        [JsonProperty("$numberLong")]
        public string NumberLong { get; set; }
    }

    public partial class Id
    {
        [JsonProperty("$oid")]
        public string Oid { get; set; }
    }

    public partial class Alert
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("Activation")]
        public Activation Activation { get; set; }

        [JsonProperty("Expiry")]
        public Activation Expiry { get; set; }

        [JsonProperty("MissionInfo")]
        public MissionInfo MissionInfo { get; set; }
    }

    public partial class MissionInfo
    {
        [JsonProperty("missionType")]
        public string MissionType { get; set; }

        [JsonProperty("faction")]
        public string Faction { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("levelOverride")]
        public string LevelOverride { get; set; }

        [JsonProperty("enemySpec")]
        public string EnemySpec { get; set; }

        [JsonProperty("minEnemyLevel")]
        public long MinEnemyLevel { get; set; }

        [JsonProperty("maxEnemyLevel")]
        public long MaxEnemyLevel { get; set; }

        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("seed")]
        public long Seed { get; set; }

        [JsonProperty("maxWaveNum", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaxWaveNum { get; set; }

        [JsonProperty("missionReward")]
        public MissionReward MissionReward { get; set; }

        [JsonProperty("nightmare", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Nightmare { get; set; }

        [JsonProperty("extraEnemySpec", NullValueHandling = NullValueHandling.Ignore)]
        public string ExtraEnemySpec { get; set; }
    }

    public partial class MissionReward
    {
        [JsonProperty("credits")]
        public long Credits { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Items { get; set; }
    }

    public partial class BadlandNode
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("DefenderInfo")]
        public DefenderInfo DefenderInfo { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }
    }

    public partial class DefenderInfo
    {
        [JsonProperty("IsAlliance")]
        public bool IsAlliance { get; set; }

        [JsonProperty("Id")]
        public Id Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("MOTD")]
        public string Motd { get; set; }

        [JsonProperty("DeployerName")]
        public string DeployerName { get; set; }

        [JsonProperty("DeployerClan", NullValueHandling = NullValueHandling.Ignore)]
        public string DeployerClan { get; set; }
    }

    public partial class DailyDeal
    {
        [JsonProperty("StoreItem")]
        public string StoreItem { get; set; }

        [JsonProperty("Activation")]
        public Activation Activation { get; set; }

        [JsonProperty("Expiry")]
        public Activation Expiry { get; set; }

        [JsonProperty("Discount")]
        public long Discount { get; set; }

        [JsonProperty("OriginalPrice")]
        public long OriginalPrice { get; set; }

        [JsonProperty("SalePrice")]
        public long SalePrice { get; set; }

        [JsonProperty("AmountTotal")]
        public long AmountTotal { get; set; }

        [JsonProperty("AmountSold")]
        public long AmountSold { get; set; }
    }

    public partial class Event
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("Messages")]
        public List<Message> Messages { get; set; }

        [JsonProperty("Prop")]
        public string Prop { get; set; }

        [JsonProperty("Date")]
        public Activation Date { get; set; }

        [JsonProperty("ImageUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageUrl { get; set; }

        [JsonProperty("Priority")]
        public bool Priority { get; set; }

        [JsonProperty("MobileOnly")]
        public bool MobileOnly { get; set; }
    }

    public partial class Message
    {
        [JsonProperty("LanguageCode")]
        public string LanguageCode { get; set; }

        [JsonProperty("Message")]
        public string MessageMessage { get; set; }
    }

    public partial class FlashSale
    {
        [JsonProperty("TypeName")]
        public string TypeName { get; set; }

        [JsonProperty("StartDate")]
        public Activation StartDate { get; set; }

        [JsonProperty("EndDate")]
        public Activation EndDate { get; set; }

        [JsonProperty("Featured")]
        public bool Featured { get; set; }

        [JsonProperty("Popular")]
        public bool Popular { get; set; }

        [JsonProperty("ShowInMarket", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowInMarket { get; set; }

        [JsonProperty("BannerIndex")]
        public long BannerIndex { get; set; }

        [JsonProperty("Discount")]
        public long Discount { get; set; }

        [JsonProperty("RegularOverride")]
        public long RegularOverride { get; set; }

        [JsonProperty("PremiumOverride")]
        public long PremiumOverride { get; set; }

        [JsonProperty("BogoBuy")]
        public long BogoBuy { get; set; }

        [JsonProperty("BogoGet")]
        public long BogoGet { get; set; }
    }

    public partial class Invasion
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("Faction")]
        public FactionEnum Faction { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Count")]
        public long Count { get; set; }

        [JsonProperty("Goal")]
        public long Goal { get; set; }

        [JsonProperty("LocTag")]
        public string LocTag { get; set; }

        [JsonProperty("Completed")]
        public bool Completed { get; set; }

        [JsonProperty("AttackerReward")]
        public AttackerReward AttackerReward { get; set; }

        [JsonProperty("AttackerMissionInfo")]
        public AttackerMissionInfo AttackerMissionInfo { get; set; }

        [JsonProperty("DefenderReward")]
        public ErReward DefenderReward { get; set; }

        [JsonProperty("DefenderMissionInfo")]
        public DefenderMissionInfo DefenderMissionInfo { get; set; }

        [JsonProperty("Activation")]
        public Activation Activation { get; set; }
    }

    public partial class AttackerMissionInfo
    {
        [JsonProperty("seed")]
        public long Seed { get; set; }

        [JsonProperty("faction")]
        public AttackerMissionInfoFaction Faction { get; set; }
    }

    public partial class ErReward
    {
        [JsonProperty("countedItems")]
        public List<CountedItem> CountedItems { get; set; }
    }

    public partial class CountedItem
    {
        [JsonProperty("ItemType")]
        public string ItemType { get; set; }

        [JsonProperty("ItemCount")]
        public long ItemCount { get; set; }
    }

    public partial class DefenderMissionInfo
    {
        [JsonProperty("seed")]
        public long Seed { get; set; }

        [JsonProperty("faction")]
        public FactionEnum Faction { get; set; }

        [JsonProperty("missionReward", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> MissionReward { get; set; }
    }

    public partial class LibraryInfo
    {
        [JsonProperty("LastCompletedTargetType")]
        public string LastCompletedTargetType { get; set; }
    }

    public partial class NodeOverride
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }

        [JsonProperty("Hide", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Hide { get; set; }

        [JsonProperty("Seed", NullValueHandling = NullValueHandling.Ignore)]
        public long? Seed { get; set; }
    }

    public partial class PrimeAvailability
    {
        [JsonProperty("State")]
        public string State { get; set; }
    }

    public partial class PvpChallengeInstance
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("challengeTypeRefID")]
        public string ChallengeTypeRefId { get; set; }

        [JsonProperty("startDate")]
        public Activation StartDate { get; set; }

        [JsonProperty("endDate")]
        public Activation EndDate { get; set; }

        [JsonProperty("params")]
        public List<Param> Params { get; set; }

        [JsonProperty("isGenerated")]
        public bool IsGenerated { get; set; }

        [JsonProperty("PVPMode")]
        public string PvpMode { get; set; }

        [JsonProperty("subChallenges")]
        public List<Id> SubChallenges { get; set; }

        [JsonProperty("Category")]
        public Category Category { get; set; }
    }

    public partial class Param
    {
        [JsonProperty("n")]
        public N N { get; set; }

        [JsonProperty("v")]
        public long V { get; set; }
    }

    public partial class Sorty
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("Activation")]
        public Activation Activation { get; set; }

        [JsonProperty("Expiry")]
        public Activation Expiry { get; set; }

        [JsonProperty("Boss")]
        public string Boss { get; set; }

        [JsonProperty("Reward")]
        public string Reward { get; set; }

        [JsonProperty("ExtraDrops")]
        public List<object> ExtraDrops { get; set; }

        [JsonProperty("Seed")]
        public long Seed { get; set; }

        [JsonProperty("Variants")]
        public List<Variant> Variants { get; set; }

        [JsonProperty("Twitter")]
        public bool Twitter { get; set; }
    }

    public partial class Variant
    {
        [JsonProperty("missionType")]
        public string MissionType { get; set; }

        [JsonProperty("modifierType")]
        public string ModifierType { get; set; }

        [JsonProperty("node")]
        public string Node { get; set; }

        [JsonProperty("tileset")]
        public string Tileset { get; set; }
    }

    public partial class SyndicateMission
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("Activation")]
        public Activation Activation { get; set; }

        [JsonProperty("Expiry")]
        public Activation Expiry { get; set; }

        [JsonProperty("Tag")]
        public string Tag { get; set; }

        [JsonProperty("Seed")]
        public long Seed { get; set; }

        [JsonProperty("Nodes")]
        public List<string> Nodes { get; set; }

        [JsonProperty("Jobs", NullValueHandling = NullValueHandling.Ignore)]
        public List<Job> Jobs { get; set; }
    }

    public partial class Job
    {
        [JsonProperty("jobType")]
        public string JobType { get; set; }

        [JsonProperty("rewards")]
        public string Rewards { get; set; }

        [JsonProperty("masteryReq")]
        public long MasteryReq { get; set; }

        [JsonProperty("minEnemyLevel")]
        public long MinEnemyLevel { get; set; }

        [JsonProperty("maxEnemyLevel")]
        public long MaxEnemyLevel { get; set; }

        [JsonProperty("xpAmounts")]
        public List<long> XpAmounts { get; set; }
    }

    public partial class VoidTrader
    {
        [JsonProperty("_id")]
        public Id Id { get; set; }

        [JsonProperty("Activation")]
        public Activation Activation { get; set; }

        [JsonProperty("Expiry")]
        public Activation Expiry { get; set; }

        [JsonProperty("Character")]
        public string Character { get; set; }

        [JsonProperty("Node")]
        public string Node { get; set; }
    }

    public enum AttackerMissionInfoFaction { FcCorpus, FcGrineer };

    public enum FactionEnum { FcCorpus, FcGrineer, FcInfestation };

    public enum Category { PvpChallengeTypeCategoryDaily, PvpChallengeTypeCategoryWeekly, PvpChallengeTypeCategoryWeeklyRoot, PVPChallengeTypeCategory_MODEAFFECTOR};

    public enum N { ScriptParamValue };

    public partial struct AttackerReward
    {
        public List<object> AnythingArray;
        public ErReward ErReward;

        public static implicit operator AttackerReward(List<object> AnythingArray) => new AttackerReward { AnythingArray = AnythingArray };
        public static implicit operator AttackerReward(ErReward ErReward) => new AttackerReward { ErReward = ErReward };
        public bool IsNull => AnythingArray == null && ErReward == null;
    }

    public partial class Warframe
    {
        public static Warframe FromJson(string json) => JsonConvert.DeserializeObject<Warframe>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Warframe self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                AttackerMissionInfoFactionConverter.Singleton,
                AttackerRewardConverter.Singleton,
                FactionEnumConverter.Singleton,
                CategoryConverter.Singleton,
                NConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class AttackerMissionInfoFactionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AttackerMissionInfoFaction) || t == typeof(AttackerMissionInfoFaction?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "FC_CORPUS":
                    return AttackerMissionInfoFaction.FcCorpus;
                case "FC_GRINEER":
                    return AttackerMissionInfoFaction.FcGrineer;
            }
            throw new Exception("Cannot unmarshal type AttackerMissionInfoFaction");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AttackerMissionInfoFaction)untypedValue;
            switch (value)
            {
                case AttackerMissionInfoFaction.FcCorpus:
                    serializer.Serialize(writer, "FC_CORPUS");
                    return;
                case AttackerMissionInfoFaction.FcGrineer:
                    serializer.Serialize(writer, "FC_GRINEER");
                    return;
            }
            throw new Exception("Cannot marshal type AttackerMissionInfoFaction");
        }

        public static readonly AttackerMissionInfoFactionConverter Singleton = new AttackerMissionInfoFactionConverter();
    }

    internal class AttackerRewardConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AttackerReward) || t == typeof(AttackerReward?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<ErReward>(reader);
                    return new AttackerReward { ErReward = objectValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<List<object>>(reader);
                    return new AttackerReward { AnythingArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type AttackerReward");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (AttackerReward)untypedValue;
            if (value.AnythingArray != null)
            {
                serializer.Serialize(writer, value.AnythingArray);
                return;
            }
            if (value.ErReward != null)
            {
                serializer.Serialize(writer, value.ErReward);
                return;
            }
            throw new Exception("Cannot marshal type AttackerReward");
        }

        public static readonly AttackerRewardConverter Singleton = new AttackerRewardConverter();
    }

    internal class FactionEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(FactionEnum) || t == typeof(FactionEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "FC_CORPUS":
                    return FactionEnum.FcCorpus;
                case "FC_GRINEER":
                    return FactionEnum.FcGrineer;
                case "FC_INFESTATION":
                    return FactionEnum.FcInfestation;
            }
            throw new Exception("Cannot unmarshal type FactionEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (FactionEnum)untypedValue;
            switch (value)
            {
                case FactionEnum.FcCorpus:
                    serializer.Serialize(writer, "FC_CORPUS");
                    return;
                case FactionEnum.FcGrineer:
                    serializer.Serialize(writer, "FC_GRINEER");
                    return;
                case FactionEnum.FcInfestation:
                    serializer.Serialize(writer, "FC_INFESTATION");
                    return;
            }
            throw new Exception("Cannot marshal type FactionEnum");
        }

        public static readonly FactionEnumConverter Singleton = new FactionEnumConverter();
    }

    internal class CategoryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Category) || t == typeof(Category?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "PVPChallengeTypeCategory_DAILY":
                    return Category.PvpChallengeTypeCategoryDaily;
                case "PVPChallengeTypeCategory_WEEKLY":
                    return Category.PvpChallengeTypeCategoryWeekly;
                case "PVPChallengeTypeCategory_WEEKLY_ROOT":
                    return Category.PvpChallengeTypeCategoryWeeklyRoot;
                case "PVPChallengeTypeCategory_MODEAFFECTOR":
                    return Category.PVPChallengeTypeCategory_MODEAFFECTOR;

            }
            throw new Exception("Cannot marshal type Category");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Category)untypedValue;
            switch (value)
            {
                case Category.PvpChallengeTypeCategoryDaily:
                    serializer.Serialize(writer, "PVPChallengeTypeCategory_DAILY");
                    return;
                case Category.PvpChallengeTypeCategoryWeekly:
                    serializer.Serialize(writer, "PVPChallengeTypeCategory_WEEKLY");
                    return;
                case Category.PvpChallengeTypeCategoryWeeklyRoot:
                    serializer.Serialize(writer, "PVPChallengeTypeCategory_WEEKLY_ROOT");
                    return;
                case Category.PVPChallengeTypeCategory_MODEAFFECTOR:
                    serializer.Serialize(writer, "PVPChallengeTypeCategory_MODEAFFECTOR");
                    return;
            }
            throw new Exception("Cannot marshal type Category");
        }

        public static readonly CategoryConverter Singleton = new CategoryConverter();
    }

    internal class NConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(N) || t == typeof(N?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "ScriptParamValue")
            {
                return N.ScriptParamValue;
            }
            throw new Exception("Cannot unmarshal type N");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (N)untypedValue;
            if (value == N.ScriptParamValue)
            {
                serializer.Serialize(writer, "ScriptParamValue");
                return;
            }
            throw new Exception("Cannot marshal type N");
        }

        public static readonly NConverter Singleton = new NConverter();
    }
}


