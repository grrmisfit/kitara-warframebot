using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Warframebot.Modules.Warframe;
using Warframebot.Core.UserAccounts;
using System;

namespace Warframebot
{
    class Utilities
    {
        
        private static Dictionary<string, string> missions;
        private static Dictionary<string, string> sorties;
       
        private static Dictionary<string, string> voids;
    static Utilities()
        {
           
          
            string themissions = File.ReadAllText("Systemlang/WfData.json");
            string sortieinfo = File.ReadAllText("Systemlang/WfData.json");
           
           
            //JsonConvert.DeserializeObject<dynamic>(therewards);
            var data2 = JsonConvert.DeserializeObject<dynamic>(themissions);
           var sortiedata = JsonConvert.DeserializeObject<Dictionary<string, string>>(sortieinfo);
            //var sortiedata = JsonConvert.DeserializeObject<dynamic>(sortieinfo);
            
            missions = data2.ToObject<Dictionary<string, string>>();
            sorties = sortiedata;
            //sortiedata.ToObject<Dictionary<string, string>>();
            
            // for (int i = 0; i < sortiedata.Count; i++)
            // {
            //     string value_with_backslashes = sortiedata.ChildrenTokens[i].Name;
            // }
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
            string apiresponse = "";
                 using (WebClient client = new WebClient())
                 apiresponse = client.DownloadString(url);
                 return apiresponse;
        }

        public double GetTimeDiff(DateTime date)
        {
            var diff = date - DateTime.Now;
            return diff.TotalMinutes;

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
