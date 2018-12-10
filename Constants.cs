using System;

namespace Warframebot
{
    public static class Constants
    {
        internal static readonly string ResourceFolder = "resources";
        internal static readonly string UserAccountsFolder = "users";
        internal static readonly string ServerAccountsFolder = "servers";
        internal static readonly string LogFolder = "Logs";
        internal static readonly string InvisibleString = "\u200b";
        public const ulong DailyMuiniesGain = 250;
        public const int MessageRewardCooldown = 30;
        public const int MessageRewardMinLenght = 20;
        
        public static readonly Tuple<int, int> MessagRewardMinMax = Tuple.Create(1, 5);
        public static readonly int MinTimerIntervall = 3000;
        public const string Version = "3.1";
        internal const string Defense = "http://warframe.wikia.com/wiki/Defense";
        internal const string Spy = "http://warframe.wikia.com/wiki/Spy_2.0";
        internal const string Capture = "http://warframe.wikia.com/wiki/Capture";
        internal const string Exterminate = "http://warframe.wikia.com/wiki/Exterminate";
        internal const string Survival = "http://warframe.wikia.com/wiki/Survival";
        internal const string Excavation = "http://warframe.wikia.com/wiki/Excavation";
        internal const string Interception = "http://warframe.wikia.com/wiki/Interception";
    }
}