
using System.Collections.Generic;
using System.Linq;


namespace Warframebot.Core.UserAccounts
{
    public static class UserAccounts
    {

        public static List<UserAccount> accounts;


        public static string accountsFile = "players.json";
            static UserAccounts()
             {
                if(DataStorage.SaveExists(accountsFile))
            {
               
                accounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
               
            }
                else
            {
                accounts = new List<UserAccount>();
                DataStorage.SaveUserAccounts(accounts, accountsFile);
            }
             }
        public static void SaveAccounts()
        {
            DataStorage.SaveUserAccounts(accounts, accountsFile);
        }
        public static bool AccountExists(string id)
        {
            var theaccount = GetOrCreateAccount(id);
            bool itsfound = false;
            if (theaccount == null)
            { itsfound = false; }
            else
            { itsfound = true; }

            return itsfound;
        }
        public static UserAccount GetAccount(string user)
        {
            return GetOrCreateAccount(user);
        }

        private static UserAccount GetOrCreateAccount(string id)
        {
            var result = from a in accounts
                         where a.Name == id
                         select a;

            var account = result.FirstOrDefault();
            if (account == null) account = CreateUserAccount(id);
            return account;
        }

        private static UserAccount CreateUserAccount(string id)
        {
            var newAccount = new UserAccount()
            {
                Name = id,
                Points = 0,
                GamesWon = 0
            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}