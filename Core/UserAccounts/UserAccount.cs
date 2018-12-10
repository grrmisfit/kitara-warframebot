using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Warframebot.Core.UserAccounts
{
    public class UserAccount
    {
        public class AlarmUsers
        {
            public int Id { get; set; }
            public ulong DiscordId { get; set; }
            public string DiscordName { get; set; }
            public int AlarmDelay { get; set; }
            public bool AlarmOn { get; set; }
            public DateTime TimeAlerted { get; set; }
            public ulong AlarmChannel { get; set; }
        }


        public class Fissures
        {
            public bool CheckFissures { get; set; }
            public IList<string> WantedFissures { get; set; }
            public IList<string> KnownFissures { get; set; }

        }


        public class Rewards
        {
            public IList<string> WantedRewards { get; set; }
        }

        public class News
        {
            public IList<string> KnownNews { get; set; }
        }

        public class Alerts
        {
            public IList<string> KnownAlerts { get; set; }
        }

        public class PmLists
        {
            public IList<string> PmList { get; set; }
        }
        public class GuildAccounts
        {
            public int Id { get; set; }


            public ulong Guild { get; set; }


            public ulong AlertsChannel { get; set; }
            
            public bool CheckAlerts { get; set; }

            public Rewards Rewards { get; set; }

            public Fissures Fissures { get; set; }
            
            public DateTime AlertTimeChecked { get; set; }

            public DateTime TimeChecked { get; set; }

            public DateTime CetusTimeChecked { get; set; }

            public int AlertDelay { get; set; }

            public bool CetusTime { get; set; }

            public bool Cetus15TimeAlerted { get; set; }

            public bool Cetus5TimeAlerted { get; set; }

            public News News { get; set; }

            public Alerts Alerts { get; set; }

            public bool NotifyAlerts { get; set; }

            public PmLists PmLists { get; set; }

            

            public bool NotifyNews { get; set; }
        }
    }

   
}

