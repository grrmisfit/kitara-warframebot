using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using Warframebot.Core.UserAccounts;

namespace Warframebot.Modules.Warframe
{
    public class WarframeSettings : ModuleBase<SocketCommandContext>
    {
        [Command("add reward")]
        [Remarks("Add a reward to saved list")]
        [Summary("Adds a reward wanted by the user (this is server specific) to saved list.\nExample: !add reward endo")]
        public async Task SetReward([Remainder] string wantedreward)
        {
            if (wantedreward == "")
            {
               
                await Context.Channel.SendMessageAsync("No input detected");
                return;
            }

            var rewardCheck = Utilities.AddRewards(Context.Guild.Id, wantedreward);
            if (rewardCheck == "added")
            {
                await Context.Channel.SendMessageAsync($"{wantedreward} has been added! Use remove reward to delete.");

            }
            else
            {
                await Context.Channel.SendMessageAsync(
                    $"**{wantedreward}** is already in the list, or something went wrong! use **!list rewards** to check if its in the list");
            }

        }

        [Command("remove reward")]
        [Remarks("Remove a reward from saved list")]
        [Summary("This would remove a saved reward from saved list.\nExample: !remove reward endo")]
        public async Task DelReward([Remainder] string msg)
        {
            bool addCheck = false;
            var theAccount = UserAccounts.GetAccount(Context.Guild.Id);
            for (int i = 0; i < theAccount.WantedRewards.Count; i++)
            {
                if (theAccount.WantedRewards[i].ToLower().Contains(msg.ToLower()))
                {

                    theAccount.WantedRewards.Remove(msg);
                    UserAccounts.SaveAccounts();
                    addCheck = true;
                    break;
                }
            }

            if (addCheck == false)
            {
                await Context.Channel.SendMessageAsync($"{msg} not found in list!");
                return;
            }

            await Context.Channel.SendMessageAsync($"{msg} removed from list!");
        }

        [Command("remove fissure"), Alias("rf")]
        [Remarks("Removes fissure from list")]
        [Summary("This would remove defense from a list of wanted fissures.\nExample: !remove fissure defense. ")]
        public async Task DelFissure([Remainder] string msg)
        {
            bool addCheck = false;
            var theAccount = UserAccounts.GetAccount(Context.Guild.Id);
            for (int i = 0; i < theAccount.WantedFissures.Count; i++)
            {
                if (theAccount.WantedFissures[i].ToLower().Contains(msg.ToLower()))
                {

                    theAccount.WantedFissures.Remove(msg);
                    UserAccounts.SaveAccounts();
                    addCheck = true;
                    break;
                }
            }

            if (addCheck == false)
            {
                await Context.Channel.SendMessageAsync($"{msg} not found in list!");
                return;
            }

            await Context.Channel.SendMessageAsync($"{msg} removed from list!");
        }

        [Command("add fissure"), Alias("af")]
        [Remarks("Add a wanted fissure to saved list")]
        [Summary("Fissures added to the list that are checked and alerts the channel if found. Alerts are delayed based on the !delay setting.\n Example: !add fissure defense")]
        public async Task AddFissureAlerts([Remainder] string wantedfissure)
        {
            if (wantedfissure == "")
            {
                await Context.Channel.SendMessageAsync("You must enter a fissure name");
                return;
            }

            var addCheck = Utilities.AddFissures(Context.Guild.Id, wantedfissure);
            if (addCheck == "added")
            {
                await Context.Channel.SendMessageAsync($"{wantedfissure} has been added! Use remove fissure to delete");
            }
            else
            {
                await Context.Channel.SendMessageAsync(
                    $"**{wantedfissure}** is already in the list, or something went wrong! use **!list fissures** to check if its in the list");
            }

        }
        [Command("alarm")]
        [Remarks("Set how minutes before its dark on cetus you want to be alerted, or turn alarm on or off")]
        [Summary("Example: !alarm 10. To turn on/off !alarm off/on")]
        public async Task SetCetusAlarm([Remainder] string minutes)
        {
            var user = Context.User.Id;
            var json = string.Empty;

            json = Utilities.LoadJsonData("SystemLang/Alarm.json");
            if (string.IsNullOrEmpty(json))
            {
                await Context.Channel.SendMessageAsync("There was an error, please try again later!");
                return;
            }

            JArray a = JArray.Parse(json);
            List<UserAccount> tmpdata = new List<UserAccount>();
            var alarmUsers = a.ToObject<List<UserAccount>>();
            var alarmUser = UserAccounts.GetAlarmUser(user, 15);
            foreach (var users in alarmUsers)
            {
                if (users.DiscordId == user)
                {
                    if (minutes.ToLower() == "off")
                    {
                        alarmUser.AlarmOn = false;
                        await Context.Channel.SendMessageAsync($"{Context.User.Username}, Your Cetus alarm is now **Off**");
                        UserAccounts.SaveAlarmUser();
                        return;
                    }

                    {
                        if (minutes.ToLower() == "on")
                        {
                            alarmUser.AlarmOn = true;
                            UserAccounts.SaveAlarmUser();
                            await Context.Channel.SendMessageAsync($"{Context.User.Username}, Your Cetus alarm is now **On**");
                            return;
                        }
                    }
                }
            }

            int delaytime = 10;
            if (Int32.TryParse(minutes, out delaytime))
            {
                var wanteddelay = Int32.Parse(minutes);
                var theuser = UserAccounts.GetAlarmUser(user, wanteddelay);

                theuser.AlarmDelay = wanteddelay;
                theuser.AlarmChannel = Context.Channel.Id;
                UserAccounts.SaveAlarmUser();
                await Context.Channel.SendMessageAsync(
                    $"<@{Context.User.Id}> I have added an alarm for cetus nighttime with a delay of **{wanteddelay}** minutes!");
            }
        }
        [Command("set")]
        [Remarks("Turns a setting on or off for following alerts.")]
        [Summary("Valid options are, alert channel(sets channel command is given from, Admins only), check alerts(to get messages on saved alerts)," +
                 "check fissures(alerts on wanted fissures), alert cetus(alerts you to nighttime on cetus 15 and 5 mins before).\nExample: !set alert channel")]
        public async Task SetCommands([Remainder] string command = "")
        {
            if (command == "")
            {
                await Context.Channel.SendMessageAsync("You must enter a valid set command. Type @help set for further instructions.");
                return;
            }

            switch (command)
            {
                case "alert channel":

                    if (Context.User.Id == Config.bot.ownerId)
                    {
                        WFSettings.AlertsChannel = Context.Channel.Id;
                        var thealertAccount = UserAccounts.GetAccount(Context.Guild.Id);
                        thealertAccount.AlertsChannel = Context.Channel.Id;
                        UserAccounts.SaveAccounts();
                        await Context.Channel.SendMessageAsync(
                            $"Alert channel has been set to {Context.Channel.Name} on server {Context.Guild.Name}");
                        break;
                    }

                    await Context.Channel.SendMessageAsync($"{Context.User.Username}, you do not have proper privileges to set that!");
                    break;

                case "check alerts":

                    var theAccount = UserAccounts.GetAccount(Context.Guild.Id);
                    if (theAccount.CheckAlerts)
                    {
                        theAccount.CheckAlerts = false;
                        UserAccounts.SaveAccounts();
                        await Context.Channel.SendMessageAsync("Alert rewards messages are now turned off!");
                        break;
                    }

                    theAccount.CheckAlerts = true;
                    UserAccounts.SaveAccounts();
                    await Context.Channel.SendMessageAsync($"You will now get alerts for saved rewards every {theAccount.AlertDelay} mins");
                    break;
                case "check fissures":

                    var fissureCheck = UserAccounts.GetAccount(Context.Guild.Id);
                    if (fissureCheck.CheckFissures)
                    {
                        fissureCheck.CheckFissures = false;
                        UserAccounts.SaveAccounts();
                        await Context.Channel.SendMessageAsync("Fissures alerts are now turned off!");
                        break;
                    }

                    fissureCheck.CheckFissures = true;
                    UserAccounts.SaveAccounts();
                    await Context.Channel.SendMessageAsync(
                        $"You will now get alerts for saved fissures every {fissureCheck.AlertDelay} mins");
                    break;
                case "alert cetus":
                    var cetuscheck = UserAccounts.GetAccount(Context.Guild.Id);
                    if (cetuscheck.CetusTime == false)
                    {
                        cetuscheck.CetusTime = true;
                        UserAccounts.SaveAccounts();
                        await Context.Channel.SendMessageAsync("Cetus nightime alerts are now on!");
                        break;
                    }
                    else
                    {
                        cetuscheck.CetusTime = false;
                        UserAccounts.SaveAccounts();
                        await Context.Channel.SendMessageAsync("Cetus nightime alerts are now off!");
                        break;
                    }

                default:
                    await Context.Channel.SendMessageAsync("Command not recognized ");
                    break;
            }
        }

        [Command("delay")]
        [Remarks("Set delay for alerts.")]
        [Summary("Alerts are sent every X amount of minutes set by delay. example: !delay 15 will set all messsages to come every 15 minutes")]
        public async Task SetDelay([Remainder] string msg)
        {
            int delaytime = 5;
            if (Int32.TryParse(msg, out delaytime))
            {
                var delayaccount = UserAccounts.GetAccount(Context.Guild.Id);
                var checkedtime = Int32.Parse(msg);
                delayaccount.AlertDelay = checkedtime;
                UserAccounts.SaveAccounts();
                await Context.Channel.SendMessageAsync($"Messages will be sent every **{msg}** minutes!");
            }
        }

    }
}