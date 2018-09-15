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
        public const int MaxMessageLength = 2000;
        public static readonly Tuple<int, int> MessagRewardMinMax = Tuple.Create(1, 5);
        public static readonly int MinTimerIntervall = 3000;
        public const string Version = "3.1";
        public const string Defense = "http://warframe.wikia.com/wiki/Defense";
        public const string Spy = "http://warframe.wikia.com/wiki/Spy_2.0";
        public const string Capture = "http://warframe.wikia.com/wiki/Capture";
        public const string Exterminate = "http://warframe.wikia.com/wiki/Exterminate";
        public const string Survival = "http://warframe.wikia.com/wiki/Survival";
        public const string Excavation = "http://warframe.wikia.com/wiki/Excavation";
        public const string Interception = "http://warframe.wikia.com/wiki/Interception";
    }
}