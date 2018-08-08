using Discord.WebSocket;
using warframebot.Modules;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace warframebot.Core.UserAccounts
{
  public static  class UserAccounts
    {
        

        public static List<GetPlayerOnlineResult> accounts;
        public static List<GetPlayerOnlineResult> curonline;

        public static string accountsFile = "players.json";
            static UserAccounts()
             {
                if(DataStorage.SaveExists(accountsFile))
            {
               
                accounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
               
            }
                else
            {
                accounts = new List<GetPlayerOnlineResult>();
                DataStorage.SaveUserAccounts(accounts, accountsFile);
            }
             }
        
        
            
          }
        
        }
