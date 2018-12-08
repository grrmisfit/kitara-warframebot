using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Warframebot.Core.UserAccounts;
using System;
using System.Threading.Tasks;
using LiteDB;
using Warframebot.Data;
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

       // private static Dictionary<string, string> missions;
        private static Dictionary<string, string> wfData;


        static Utilities()
        {

            if (!File.Exists("Systemlang/WfData.json"))
            {
                Console.WriteLine("WfData.json could not be found! This is a vital file and program will now end!");
                Task.Delay(5000);
                Environment.Exit(0);
              
            }
            string wfDbInfo = File.ReadAllText("Systemlang/WfData.json");
            //string sortieinfo = File.ReadAllText("Systemlang/WfData.json");



            //var data2 = JsonConvert.DeserializeObject<dynamic>(wfDbInfo);
            var sortiedata = JsonConvert.DeserializeObject<Dictionary<string, string>>(wfDbInfo);

           // missions = data2.ToObject<Dictionary<string, string>>();
            wfData = sortiedata;

        }


        public static string GetMissions(string key)
        {
            if (wfData.ContainsKey(key)) return wfData[key];
            return "";
        }

        public static string GetSortieBoss(string key)
        {
            if (wfData.ContainsKey(key)) return wfData[key];

            return "";
        }
        public static string GetSortieType(string key)
        {
            if (wfData.ContainsKey(key)) return wfData[key];

            return "";
        }
        public static string GetSortieSeed(string key)
        {
            if (wfData.ContainsKey(key)) return wfData[key];

            return "";
        }
        public static string ReplaceInfo(string key)
        {

            if (wfData.ContainsKey(key)) return wfData[key];
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

        public static string AddFissures(ulong guildId, string msg)
        {
            var guilddata = DbStorage.GetGuildInfo(guildId);//UserAccounts.GetAccount(guildId);

            var json = File.ReadAllText("SystemLang/WFdata.json");
            var thedata = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if(guilddata.Fissures == null) goto next;
            
           
            for (int i = 0; i < guilddata.Fissures.WantedFissures.Count; ++i)
            {
                if (guilddata.Fissures.WantedFissures[i] == msg)
                {
                    return "not added";
                }
            }
           next:
            foreach (var fissure in thedata)
            {
                if (msg.ToLower() == fissure.Value.ToLower())
                {
                   // var theaccount = DbStorage.GetGuildInfo(guildId);//UserAccounts.GetAccount(guildId);
                                                                     //theaccount.WantedFissures.Add(msg);

                    //var update = new UserAccount.GuildAccounts
                    
                        guilddata.Fissures.WantedFissures.Add(msg); 
                         
                   
                    


                    using (var db = new LiteDatabase(@"Data\guilds.db"))
                    {

                        var guilds = db.GetCollection<UserAccount.GuildAccounts>("guilds");
                        var results = guilds.FindOne(x => x.Guild == guildId);
                        if (results == null)
                        {
                            guilds.Insert(guilddata);

                        }

                        guilds.Update(guilddata);
                        //theaccount.WantedFissures.Add(msg);
                        // UserAccounts.SaveAccounts();
                       // DbStorage.UpdateDb(guildId, guilddata);
                    }

                    break;
                }
            }

            return "added";
        }

        public static string AddRewards(ulong guildid, string msg)
        {

            var theAccount = DbStorage.GetGuildInfo(guildid);
            if (theAccount.Rewards == null) goto next;
            
            for (int i = 0; i < theAccount.Rewards.WantedRewards.Count; i++)
            {
                if (theAccount.Rewards.WantedRewards[i].ToLower().Contains(msg.ToLower()))
                {


                    return "not added";
                }
            }
            next:
            if (theAccount.Rewards != null)
            {
                theAccount.Rewards.WantedRewards.Add(msg);
                var update = new UserAccount.GuildAccounts
                {
                    Rewards = new UserAccount.Rewards
                    {
                        WantedRewards = new List<string>
                        {
                            msg
                        }
                    }
                };
            }

            DbStorage.UpdateDb(guildid, theAccount);
            return "added";
            
        }

        public static string GetCetusTime()
        {
            var json = GetWarframeInfo();
            if (String.IsNullOrEmpty(json)) return "error";
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
#pragma warning disable IDE0018 // Inline variable declaration
            long ignoreMe;
#pragma warning restore IDE0018 // Inline variable declaration
            if (!Int64.TryParse(expiretime, out ignoreMe))
            {
                return "125";
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

            if (!String.IsNullOrEmpty(mystr) && Int64.Parse(mystr) > 50)
            {
                long timeremain = Int64.Parse(mystr) - 50;
                return "daytime";
            }

            if (!String.IsNullOrEmpty(mystr))
            { return "nighttime"; }

            return "error";
        }

        public static string LoadJsonData(string file)
        {
            var json = String.Empty;
            json = File.ReadAllText(file);
            if (String.IsNullOrEmpty(json))
            {
                return null;
            }


            return json;
        }
        public static double TimeCheck(DateTime announcedtime, DateTime currenttime)
        {


            var checktime = currenttime.Subtract(announcedtime).TotalHours;
            return checktime;
        }

        public static string TimeSince(string thetime)
        {
            if (string.IsNullOrEmpty(thetime)) return "error";
            DateTimeOffset oldtime = DateTimeOffset.FromUnixTimeMilliseconds(Int64.Parse(thetime));
            DateTimeOffset nowtime = DateTimeOffset.UtcNow;
            var exptime = $"{Math.Floor(nowtime.Subtract(oldtime).TotalHours)}";
            
            return exptime;
        }

        public static string ExpireFisTime(string thetime)
        {

            DateTimeOffset oldtime = DateTimeOffset.FromUnixTimeMilliseconds(Int64.Parse(thetime));
            DateTimeOffset nowtime = DateTimeOffset.UtcNow;
            var exptime = $"{oldtime.Subtract(nowtime).Hours}h {oldtime.Subtract(nowtime).Minutes}m {oldtime.Subtract(nowtime).Seconds}s";
            // DateTime exptime = new DateTime(oldtime.Subtract(nowtime.to));

            return exptime;
        }
        public static async Task UpdateBot()
        {
            try
            {
                WebClient client = new WebClient();
                Uri url = new Uri("http://3rdshifters.org/Warframebot.exe");
                client.DownloadFileAsync(url, "Update/Warframebot.exe");
                client.DownloadProgressChanged += WebClientDownloadProgressChanged;
                client.DownloadDataCompleted += WebClientDownloadCompleted;
                await Task.Delay(1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Global.DownloadDone = false;
            }

        }

        private static void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            if (e.ProgressPercentage == 100) Global.DownloadDone = true;
            Console.WriteLine("Download status: {0}%.", e.ProgressPercentage);
        }

        private static void WebClientDownloadCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            Console.WriteLine("Download finished!");
            Global.DownloadDone = true;
        }

        public static string FissureLink(string wantedFissure)
        {
            string theFissure = "";
            switch (wantedFissure.ToLower())
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
                case "extermination":
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

        public static async Task CleanUpFissures()
        {
           /* var json = GetWfSettings();
            var warjson = GetWarframeInfo();
            var wardata = Warframe.FromJson(warjson);
            var thefissures = wardata.ActiveMissions;
           
            List<string> knownFis = new List<string>();
            if (string.IsNullOrEmpty(json) || json == "error")
            {
                return;
            }
            List<string> fisList = new List<string>();
            foreach (var t in thefissures)
            {
                knownFis.Add(t.Id.Oid);
            }
            var accounts = GuildAccounts.FromJson(json);

            foreach (var guild in accounts)
            {

                foreach (var t in guild.KnownFissures)
                {
                    if (!knownFis.Contains(t))
                    {
                        fisList.Add(t);
                    }
                }
                var account = UserAccounts.GetAccount(guild.Guild);

                foreach (var t in fisList)
                {
                    account.KnownFissures.Remove(t);

                }
                UserAccounts.SaveAccounts();
            }
            await Task.Delay(1000);*/
        }

        public async Task UpdateDucats()
        {
              var url = "https://api.warframe.market/v1/tools/ducats";

              WebClient client = new WebClient();
              var json =  client.DownloadString(url);
              File.WriteAllText("SystemLang/Ducats.json",json);
              await Task.Delay(1);
        }
        public static async Task CleanUpAlerts()
        {
           
            var warjson = GetWarframeInfo();
            var wardata = Warframe.FromJson(warjson);
            var theAlerts = wardata.Alerts;
           
            List<string> knownAlerts = new List<string>();
            
            List<string> alertList = new List<string>();
            foreach (var t in theAlerts)
            {
                knownAlerts.Add(t.Id.Oid);
            }
            var accounts = DbStorage.GetDb();

            foreach (var guild in accounts)
            {

                foreach (var t in guild.Alerts.KnownAlerts)
                {
                    if (!knownAlerts.Contains(t))
                    {
                        alertList.Add(t);
                    }
                }
                var account = DbStorage.GetGuildInfo(guild.Guild);

                foreach (var t in alertList)
                {
                    account.Alerts.KnownAlerts.Remove(t);

                }
                DbStorage.UpdateDb(account.Guild,account);
            }

            await Task.Delay(1000);
        }
       

        public static string CheckBans()
        {
            return "";
        }
    }



}
    

    

