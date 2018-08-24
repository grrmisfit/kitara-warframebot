
using System.Collections.Generic;
using System.Linq;


namespace Warframebot.Core.UserAccounts
{
    public static class UserAccounts
    {

        public static List<GuildAccounts> accounts;


        public static string accountsFile = "SystemLang/WFsetings.json";
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
             }
        public static void SaveAccounts()
        {
            DataStorage.SaveUserAccounts(accounts, accountsFile);
        }
        public static bool AccountExists(ulong id)
        {
            var theaccount = GetOrCreateAccount(id);
            bool itsfound = false;
            if (theaccount == null)
            { itsfound = false; }
            else
            { itsfound = true; }

            return itsfound;
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

            var account = result.FirstOrDefault();
            if (account == null) account = CreateUserAccount(id);
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
                WantedFissures = null,
                WantedRewards = null,

            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}