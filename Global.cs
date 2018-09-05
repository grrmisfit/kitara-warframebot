using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;


namespace Warframebot
{
    internal static class Global
    {
        internal static DiscordSocketClient Client { get; set; }
        internal static ulong MessageIdToTrack { get; set; }
    }

    public class SolNodeTmp
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class SortieData
    {
        public string Name { get; set; }
        public string Seed { get; set; }
        public string ModType { get; set; }
        public string ModDesc { get; set; }

    }
    public class ScramData
    {
        public static string ScramWord { get; set; }
        public static string ScrambledWord { get; set; }
        public static bool GameStarted { get; set; }
        public static bool Random { get; set; }
        public static bool WordGuessed { get; set; }
        public static bool GamePause { get; set; }
        public static string Category { get; set; }
        public static ulong ScramChannel { get; set; }
        public static bool GameWait { get; set; }
    }
   public partial class Mp3List
  
    {
    [JsonProperty("File Name")]
    public string FileName { get; set; }

    [JsonProperty("Path")]
    public string Path { get; set; }
}

public partial class Mp3List
{
    public static Dictionary<string, Mp3List> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, Mp3List>>(json, Mp3ListConverter.Settings);
}

public static class Serialize
{
    public static string ToJson(this Dictionary<string, Mp3List> self) => JsonConvert.SerializeObject(self, Mp3ListConverter.Settings);
}

internal static class Mp3ListConverter
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
