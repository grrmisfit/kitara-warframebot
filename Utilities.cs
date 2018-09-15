using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Warframebot.Core.UserAccounts;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Warframebot.Modules;
using Warframebot.Modules.Warframe;

namespace Warframebot
{
    public static class Extensions
    {
        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                    ? value
                    : value.Substring(0, maxLength)
                );
        }
    }
    class Utilities
    {
       
        private static Dictionary<string, string> missions;
        private static Dictionary<string, string> sorties;
       

    static Utilities()
        {

            if (!File.Exists("Systemlang/WfData.json"))
            {
                Console.WriteLine("WfData.json could not be found! This is a vital file and program will now end!");
                Task.Delay(5000);
                Environment.Exit(0);
                return;
            }
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
        public static string ReplaceRewardInfo(string key)
        {
            string rewards = "";
            string therewards = File.ReadAllText("Systemlang/rewards.json");
            
            var reward = JsonConvert.DeserializeObject<Dictionary<string, string>>(therewards);
            
            
            if (reward.ContainsKey(key)) rewards = reward[key];
            return rewards;
           
        }
        
      

        public static string GetWarframeInfo()
        {
            string url = "http://content.warframe.com/dynamic/worldState.php";
           
            try
            {
                string apiresponse;
                using (WebClient client = new WebClient())
                apiresponse = client.DownloadString(url);
                return apiresponse;
            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                return "";
            }
                 
                 
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
            
            
            theAccount.WantedRewards.Add(msg);
            for (int a = 0; a < theAccount.WantedRewards.Count; a++)
            {
                if(theAccount.WantedRewards[a].ToLower().Contains("nothing"))// we add nothing on account creation and now we remove it once they add something
                {
                    theAccount.WantedRewards.Remove("nothing");
                }
            }
            UserAccounts.SaveAccounts();
            return "added";
        }

        public static string GetCetusTime()
        {
            var json = GetWarframeInfo();
            if (string.IsNullOrEmpty(json)) return "error";
            var warframe = Warframe.FromJson(json);
            var checkFissAlerts = warframe.SyndicateMissions;
            var expiretime = "";
            foreach (var syn in checkFissAlerts)

            {
                if (syn.Tag == "CetusSyndicate")
                {
                    expiretime = syn.Expiry.Date.NumberLong;
                }
            }
            long ignoreMe;
            if (!Int64.TryParse(expiretime, out ignoreMe))
            {
                return  "125";
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

        public static string ExpireFisTime(string thetime)
        {
            
            DateTimeOffset oldtime = DateTimeOffset.FromUnixTimeMilliseconds(Int64.Parse(thetime));
            DateTimeOffset nowtime = DateTimeOffset.UtcNow;
            var exptime = $"{oldtime.Subtract(nowtime).Hours}h {oldtime.Subtract(nowtime).Minutes}m {oldtime.Subtract(nowtime).Seconds}s";
           // DateTime exptime = new DateTime(oldtime.Subtract(nowtime.to));

            return exptime;
        }
        public static void UpdateBot()
        {

        }

        public static string FissureLink(string wantedFissure)
        {
            string theFissure = "";
            switch (wantedFissure)
            {
                case "defense":
                    theFissure = $"[Defense has been found! List is below.]({Constants.Defense}) ";
                    break;
                case "capture":
                    theFissure = $"[Capture has been found! List is below.]({Constants.Capture})";
                    break;
                case "interception":
                    theFissure = $"[Interception has been found! List is below.]({Constants.Interception}) ";
                    break;
                case "spy":
                    theFissure = $"[Spy has been found! List is below.]({Constants.Spy}) ";
                    break;
                case "excavation":
                    theFissure = $"[Excavation has been found! List is below.]({Constants.Excavation}) ";
                    break;
                case "exterminate":
                    theFissure = $"[Exterminate has been found! List is below.]({Constants.Exterminate}) ";
                    break;
                case "survival":
                    theFissure = $"[Survival has been found! List is below.]({Constants.Survival}) ";
                    break;
                default:
                    theFissure = "Unkown";
                    break;
            }

            return theFissure;
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
