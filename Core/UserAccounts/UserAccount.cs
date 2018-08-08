using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warframebot.Core.UserAccounts
{
    public class UserAccount
    {
        public string Steamid { get; set; }


        public int Entityid { get; set; }


        public string Ip { get; set; }

        public string Name { get; set; }

        public bool Online { get; set; }

        public PositionOnline Position { get; set; }

        public int Experience { get; set; }

        public double Level { get; set; }

        public int Health { get; set; }

        public int Stamina { get; set; }


        public int Zombiekills { get; set; }


        public int Playerkills { get; set; }


        public int Playerdeaths { get; set; }


        public int Score { get; set; }


        public int Totalplaytime { get; set; }


        public string Lastonline { get; set; }


        public int Ping { get; set; }
    }


}
