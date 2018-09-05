using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Warframebot.Core.UserAccounts;
using System;
using Warframebot.Modules.Warframe;

namespace Warframebot
{
    
    class Utilities
    {
       
        private static Dictionary<string, string> missions;
        private static Dictionary<string, string> sorties;
       
/*
        private static Dictionary<string, string> voids;
*/
    static Utilities()
        {
           
          
            string themissions = File.ReadAllText("Systemlang/WfData.json");
            string sortieinfo = File.ReadAllText("Systemlang/WfData.json");
           
           
           
            var data2 = JsonConvert.DeserializeObject<dynamic>(themissions);
           var sortiedata = JsonConvert.DeserializeObject<Dictionary<string, string>>(sortieinfo);
            
            missions = data2.ToObject<Dictionary<string, string>>();
            sorties = sortiedata;
           
        }

      
        public static string GetMissions(string key)
        {
            if (missions.ContainsKey(key)) return missions[key];
            return "";
        }
     
        public static string GetSortieBoss(string key)
        {
             if (sorties.ContainsKey(key)) return sorties[key];
           
            return "";
        }
        public static string GetSortieType(string key)
        {
            if (sorties.ContainsKey(key)) return sorties[key];
            
            return "";
        }
        public static string GetSortieSeed(string key)
        {
            if (sorties.ContainsKey(key)) return sorties[key];
            
            return "";
        }   
        public static string ReplaceInfo(string key)
        {
            
            if (sorties.ContainsKey(key)) return sorties[key];
            return "Data not found!";
        }
        public static string ReplaceInfo2(string key)
        {
            string therewards = File.ReadAllText("Systemlang/rewards.json");



            var reward = JsonConvert.DeserializeObject<Dictionary<string, string>>(therewards);
            
            string rewards = "";
            if (reward.ContainsKey(key)) rewards = reward[key];
            return rewards;
           
        }
        
      

        public static string GetWarframeInfo()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
            string apiresponse;
                 using (WebClient client = new WebClient())
                 apiresponse = client.DownloadString(url);
                 return apiresponse;
        }

        public static double GetTimeDiff(DateTime date)
        {
            var diff = date - DateTime.Now;
            return diff.TotalSeconds;

        }

        public static string AddFissures(ulong guildId , string msg)
        {
            var guilddata = UserAccounts.GetAccount(guildId);
            var json = File.ReadAllText("SystemLang/WFdata.json");
            var thedata = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            for (int i = 0; i < guilddata.WantedFissures.Count; ++i)
            {
                if (msg == guilddata.WantedFissures[i])
                {
                    return "not added";
                }
            }
            foreach (var fissure in thedata)
            {
                if (msg.ToLower() == fissure.Value.ToLower())
                {
                    var theaccount = UserAccounts.GetAccount(guildId);
                    theaccount.WantedFissures.Add(msg);
                    UserAccounts.SaveAccounts();
                    break;
                }
            }

            return "added";
        }

        public static string AddRewards(ulong guildid, string msg)
        {

            var theAccount = UserAccounts.GetAccount(guildid);
            for (int i = 0; i < theAccount.WantedRewards.Count; i++)
            {
                if (theAccount.WantedRewards[i].ToLower().Contains(msg.ToLower()))
                {
                    

                    return "not added";
                 }
            }
           // var rewardCount = theAccount.WantedRewards.Count + 1;
            theAccount.WantedRewards.Add(msg);
            UserAccounts.SaveAccounts();
            return "added";
        }

        public static string GetCetusTime()
        {
            var warframe = Warframe.FromJson(Utilities.GetWarframeInfo());
            var checkFissAlerts = warframe.SyndicateMissions;
            var expiretime = "";
            foreach (var syn in checkFissAlerts)

            {
                if (syn.Tag == "CetusSyndicate")
                {
                    expiretime = syn.Expiry.Date.NumberLong;
                }
            }
            DateTimeOffset oldtime = DateTimeOffset.FromUnixTimeMilliseconds(Int64.Parse(expiretime));

            var newtime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            DateTimeOffset newexptime = DateTimeOffset.FromUnixTimeMilliseconds(newtime);

            var blah = newexptime.Subtract(oldtime);
            // blah.ToString().Length;

            var stringtime = blah.TotalMinutes.ToString();
            var strlen = stringtime.Length;
            var mystr = stringtime.Right(strlen - 1);
            mystr = mystr.Split('.')[0];
            
                return mystr;
        }

        public static string CetusTimeCheck()
        {
            var warframe = Warframe.FromJson(Utilities.GetWarframeInfo());
            var checkFissAlerts = warframe.SyndicateMissions;
            var expiretime = "";
            foreach (var syn in checkFissAlerts)

            {
                if (syn.Tag == "CetusSyndicate")
                {
                    expiretime = syn.Expiry.Date.NumberLong;
                }
            }

            DateTimeOffset oldtime = DateTimeOffset.FromUnixTimeMilliseconds(Int64.Parse(expiretime));

            var newtime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            DateTimeOffset newexptime = DateTimeOffset.FromUnixTimeMilliseconds(newtime);

            var blah = newexptime.Subtract(oldtime);
            

            var stringtime = blah.TotalMinutes.ToString();
            var strlen = stringtime.Length;
            var mystr = stringtime.Right(strlen - 1);
            mystr = mystr.Split('.')[0];

            if (!string.IsNullOrEmpty(mystr) && Int64.Parse(mystr) > 50)
            {
                long timeremain = Int64.Parse(mystr) - 50;
                return "daytime";
            }

            if (!string.IsNullOrEmpty(mystr))
            { return "nighttime";}

            return "error";
        }

        public static string LoadJsonData(string file)
        {
            var json = string.Empty;
            json = File.ReadAllText(file);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            
            return json;
        }
        public static double TimeCheck(DateTime announcedtime, DateTime currenttime)
        {
            
            
            var checktime = currenttime.Subtract(announcedtime).TotalMinutes;
            return checktime;
        }
        /* public static string GetFormattedAlert(string key, params object[] parameter)
     {

         if (alerts.ContainsKey(key))
           {  return string.Format(alerts[key], parameter);


         }
         return "";
     }
     public static string GetFormattedAlert(string key, object parameter)
     {

         return GetFormattedAlert(key, new object[] { parameter });
     } */
    }
    

    
}
