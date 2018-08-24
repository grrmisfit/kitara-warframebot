using System;

namespace Warframebot
{
    public static class Constants
    {
            internal static readonly string ResourceFolder = "resources";
            internal static readonly string UserAccountsFolder = "users";
            internal static readonly string ServerAccountsFolder = "servers";
            internal static readonly string LogFolder = "logs";
            internal static readonly string InvisibleString = "\u200b";
            public const ulong DailyMuiniesGain = 250;
            public const int MessageRewardCooldown = 30;
            public const int MessageRewardMinLenght = 20;
            public const int MaxMessageLength = 2000;
            public static readonly Tuple<int, int> MessagRewardMinMax = Tuple.Create(1, 5);
            public static readonly int MinTimerIntervall = 3000;
        }
}