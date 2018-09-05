
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace Warframebot.Core.UserAccounts
{
    public static class UserAccounts
    {
        private static List<GuildAccounts> accounts;
        private static List<UserAccount> alarmAccounts;
        private static string alarmSettings = "SystemLang/Alarm.json";
        private static string accountsFile = "SystemLang/WFsettings.json";
            static UserAccounts()
             {
                if(DataStorage.SaveExists(accountsFile))
            {
               
                accounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
               
            }
                else
            {
                accounts = new List<GuildAccounts>();
                DataStorage.SaveUserAccounts(accounts, accountsFile);
            }
                 if (DataStorage.SaveExists(alarmSettings))
                 {

                     alarmAccounts = DataStorage.LoadSavedAlarmSettings(alarmSettings).ToList();

                 }
                 else
                 {
                     alarmAccounts = new List<UserAccount>();
                     DataStorage.SaveAlarmSettings(alarmAccounts, alarmSettings);
                 }

        }
        public static void SaveAccounts()
        {
            DataStorage.SaveUserAccounts(accounts, accountsFile);
        }
        public static void SaveAlarmUser()
        {
            DataStorage.SaveAlarmSettings(alarmAccounts, alarmSettings);
        }
        public static GuildAccounts GetAccount(ulong user)
        {
            return GetOrCreateAccount(user);
        }

        private static GuildAccounts GetOrCreateAccount(ulong id)
        {
            var result = from a in accounts
                         where a.Guild == id
                         select a;

            var account = result.FirstOrDefault() ?? CreateUserAccount(id);
            return account;
        }

        private static GuildAccounts CreateUserAccount(ulong id)
        {
           
            var newAccount = new GuildAccounts()
            {
                Guild = id,
                AlertsChannel = 0,
                CheckAlerts = false,
                CheckFissures = false,
                WantedRewards = new List<string> {""},
                WantedFissures = new List<string> { "" },
                TimeChecked = DateTime.Now,
                AlertDelay = 15
            };
            
            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
        public static UserAccount GetAlarmUser(ulong user, int delay)
        {
            return GetOrCreateAlarmUser(user, delay);
        }

        private static UserAccount GetOrCreateAlarmUser(ulong id, int delay)
        {
            var result = from a in alarmAccounts
                where a.DiscordId == id
                select a;

            var account = result.FirstOrDefault() ?? CreateAlarmUser(id, delay);
            return account;
        }

        private static UserAccount CreateAlarmUser(ulong id, int delay)
        {

            var newAccount = new UserAccount()
            {
                DiscordId = id,
                AlarmDelay = delay,
                AlarmOn = true,

            };

            alarmAccounts.Add(newAccount);
            SaveAlarmUser();
            return newAccount;
        }
    }
}