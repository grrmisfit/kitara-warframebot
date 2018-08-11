using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using Discord;
namespace warframebot
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
            //string tmpreward;
            //  if (curreward.TryGetValue(key, out tmpreward))
            //  { return tmpreward; }
            //  return "Data not found!";
        }
        private Task SendMessage(string v)
        {
            throw new NotImplementedException();
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
