using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Addons.Interactive.InteractiveBuilder;
using Discord.Commands;
using Newtonsoft.Json;
using Warframebot.Modules.Market;

namespace Warframebot.Modules.Warframe
{
    public class MarketCommands : InteractiveBase
    {
        public static string  GetMarketItems()
        {
            var json = "SystemLang/Items.json";
            using (StreamReader stream = new StreamReader("SystemLang/Items.json"))
            {
                return stream.ReadToEnd();

            }
        
        }
        
        [Command("ducat list")]
        public async Task SaveDucat()
        {
            Dictionary<string, string> itemList = new Dictionary<string, string>();
            string theurl = "";
            string tmpitem = "";
            var ducatsJson = File.ReadAllText("SystemLang/Ducats.json");
            var json = GetMarketItems();
            if (string.IsNullOrEmpty(json))
            {
                return;
            }
            
            var ducats = Ducats.FromJson(ducatsJson);
            var itemDucats = ducats.Payload.PreviousHour;
            var market = Items.FromJson(json);
            var items = market.Payload.Items.En;
            foreach (var item in itemDucats)
            {
                var curitem = item.Item;
                foreach (var theitem in items)
                {
                    if (curitem == theitem.Id)
                    {
                      itemList.Add(theitem.ItemName,item.Ducats.ToString());  
                    }
                }
            }
            string json2 = JsonConvert.SerializeObject(itemList, Formatting.Indented);
            File.WriteAllText("ducatlist.json", json2);
        }
        [Command("ducat")]
        public async Task DucatSearch([Remainder] string msg)
        {
            string theurl = "";
            string tmpitem = "";
            var ducatsJson = File.ReadAllText("SystemLang/Ducats.json");
            var json = GetMarketItems();
            if (string.IsNullOrEmpty(json))
            {
                return;
            }
            var ducats = Ducats.FromJson(ducatsJson);
            var itemDucats = ducats.Payload.PreviousHour;
            var market = Items.FromJson(json);
            var items = market.Payload.Items.En;
            foreach (var item in items)
            {
                if (item.ItemName.ToLower().Contains(msg.ToLower()))
                {
                    tmpitem = item.Id;
                    break;
                }
            }

            foreach (var ducat in itemDucats)
            {
                if (ducat.Item == tmpitem)
                {
                    await Context.Channel.SendMessageAsync(
                        // $"Ducat price is {ducat.Ducats} and ducats per plat is {ducat.DucatsPerPlatinum}");
                        $"You would get {ducat.Ducats} ducats for {msg}");
                    break;
                }
            }
        }

        [Command("market search", RunMode = RunMode.Async),Alias("ms")]
        [Remarks("Search Warframe.Market for sell orders.")]
        [Summary(
            "Example: !market search nova prime \nWill search all buy orders from sellers who are listed as \"Online\". "+
            "Will return all results containing the requested item, then pick the number you want by typing it in channel\nResults will be the top 5 cheapest.")]
        public async Task FindInMarket([Remainder] string msg)
        {
            string theresp = "";
            var json = File.ReadAllText("SystemLang/Items.json");
            var market = Items.FromJson(json);
            var items = market.Payload.Items.En;
            var embed = new EmbedBuilder();
            embed.WithTitle("Search results");
            embed.WithDescription("Please pick one");
            embed.WithColor(new Color(188, 66, 244));
            int thecount = 1;
            int dacount = 0;
           List<string> theitem = new List<string>();
            List<string> pickeditem = new List<string>();
            for (long i = 0; i < items.Count(); i++)
            {
                var theitems = items[i].ItemName.ToLower();
                if (theitems.Contains(msg.ToLower()))
                {
                    if (dacount == 5)
                    {
                        break;
                    }
                    embed.AddField($"Item {dacount + 1}", theitems, true);
                    int listcount = dacount + 1;
                    theitem.Add(listcount.ToString());
                    pickeditem.Add(theitems);
                    
                    dacount = dacount + 1;
                   
                }
            }

            if (!theitem.Any())
            { await Context.Channel.SendMessageAsync(
                    "No orders were found or you mispelled the item you were looking for. Check spelling or try a different word");
                return;
            }


            await Context.Channel.SendMessageAsync("", false, embed.Build());
            var timeout = 30; //seconds the bot will wait
            var interactiveMessage = new InteractiveMessageBuilder("Pick the one you want and reply with item number!") //initial message the bot will send
                .ListenChannels(Context.Channel) //The channels the bot will listen
                .WithTimeSpan(TimeSpan.FromSeconds(timeout)) //Timeout
                .ListenUsers(Context.User) //The users the bot will listen
                .SetWrongResponseMessage("NO! pick one please") // Message sent when a response that isnt in the options is sent
                .SetTimeoutMessage("Nevermind, use the command again") //Message sent on timeout
                .SetOptions(theitem.ToArray()) //Options
                .EnableLoop() //Won't stop asking unless timeout
                .WithCancellationWord("cancel") //if c is sent, will stop
                .SetCancelationMessage("Alright, nvm then") //Message sent if the user cancels
                .Build();
            
            var response = await StartInteractiveMessage(interactiveMessage);
            if (response == null)
            {
                return;
            }
            for (int i = 0; i < theitem.Count; i++)
            {
                if (theitem[i].Contains(response.ToString()))
                {
                    theresp = theitem[i];
                }
            }
            switch (theresp)
            {
                case "1":
                    theresp =pickeditem[0];
                    break;
                case "2":
                    theresp = pickeditem[1];
                    break;
                case "3":
                    theresp = pickeditem[2];
                    break;
                case "4":
                    theresp = pickeditem[3];
                    break;
                case "5":
                    theresp = pickeditem[4];
                    break;
                default:
                    theresp = "Wrong Choice";
                   await Context.Channel.SendMessageAsync("Wrong option picked");
                    break;
            }
            foreach (var res in items)
            {
                if(res.ItemName.ToLower().Contains(theresp))
                {
                    string apiresponse = "";
                    var theurl = "https://api.warframe.market/v1/items/" + res.UrlName + "/orders";
                    try
                    {

                        using (WebClient client = new WebClient())
                            apiresponse = client.DownloadString(theurl);

                    }
                    catch (WebException e)
                    {
                       await Context.Channel.SendMessageAsync(
                            "There was an error getting the required data from the website, please try again later");
                        return;
                    }

                    if (apiresponse.Contains("Something bad happened"))
                    {
                        await Context.Channel.SendMessageAsync("Error, please try again in a couple minutes!");
                        return;
                    }
                        var orders = WFOrders.FromJson(apiresponse);
                    
                    var blah = orders.Payload.Orders;
                    List<KeyValuePair<string, long>> theOrders = new List<KeyValuePair<string, long>>();
                    foreach (var order in blah)
                    {
                        if (order.OrderType == OrderType.Sell)
                        {
                            theOrders.Add(new KeyValuePair<string, long>(order.User.IngameName,
                                order.Platinum));
                        }
                    }
                    var mylist =theOrders.OrderBy(x => x.Value).ToList();
                    var embed2 = new EmbedBuilder();
                    embed2.WithTitle("Top 5 Cheapest orders");
                    embed2.WithThumbnailUrl(
                        "http://3rdshifters.org/market.png");
                    
                    embed2.WithColor(new Color(188, 66, 244));
                    for (int i = 0; i < mylist.Count; i++)
                    {
                        string mystring = mylist[i].ToString();
                        var replstr = mystring.Replace("[", "Game Name: **");
                        replstr = replstr.Replace(",", "**\nCost: **");
                        replstr = replstr.Replace("]", "** Platinum");
                        if (i == 5) break;
                        embed2.AddField($"Order {i + 1}", $"{replstr} ");
                    }

                    await Context.Channel.SendMessageAsync("**Here are results of your order search**", false, embed2.Build());
                   
                break;

                }
            }

        }

        [Command("buy orders"),Alias("bo")]
        [Remarks("search buy orders for an item from players listed online")]
        [Summary("Example: !buy orders nova prime set\nWill return all orders for nova prime set. Search must be exact part name. If unsure use !buy search for partial matches\nResults will be top 5 highest priced.")]
        public async Task BuyOrders([Remainder] string msg)
        {
            var json = File.ReadAllText("SystemLang/Items.json");
            var market = Items.FromJson(json);
            var items = market.Payload.Items.En;
            string theurl = "";
            string apiresponse = "";
            foreach (var item in items)
            {
                var theitem = item.ItemName.ToLower();
                if (theitem == msg)
                {
                     theurl = "https://api.warframe.market/v1/items/" + item.UrlName+"/orders";
                    break;
                }
            }
            try
            {
                
                using (WebClient client = new WebClient())
                    apiresponse = client.DownloadString(theurl);

            }
            catch (WebException e)
            {
                Console.WriteLine(e);
                
            }

           var orders = WFOrders.FromJson(apiresponse);
            var blah = orders.Payload.Orders;
            List<KeyValuePair<string,long>> theOrders = new List<KeyValuePair<string,long>>();
            foreach (var order in blah)
            {
                if (order.OrderType == OrderType.Buy && order.User.Status == Status.Ingame)
                {
                    theOrders.Add(new KeyValuePair<string,long> ($"{order.User.IngameName}",order.Platinum));
                }
            }
            if (!theOrders.Any())
            {
                await Context.Channel.SendMessageAsync(
                    "No orders were found or you mispelled the item you were looking for. Check spelling or try a different search. You can also do !buy search/!bs for partial searches.");
                return;
            }
            //theOrders.ToArray();
            var mylist = theOrders.OrderByDescending(x => x.Value).ToList();
            var embed = new EmbedBuilder();
            embed.WithTitle("Top 5 Highest priced orders");
            embed.WithThumbnailUrl(
                "http://3rdshifters.org/market.png");
            for (int i = 0; i < mylist.Count; i++)
            {
                string mystring = mylist[i].ToString();
                var replstr = mystring.Replace("[", "Username: **");
                replstr = replstr.Replace(",", "**\nCost: **");
                replstr = replstr.Replace("]", "** Platinum");
                if (i == 5) break;
                embed.AddField($"Order {i + 1}",$"{replstr} ");
            }
            embed.WithColor(new Color(188, 66, 244));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
          
        }
        [Command("buy search", RunMode = RunMode.Async), Alias("bs")]
        [Remarks("Search Warframe.Market for sell orders.")]
        [Summary(
           "Example: !market search nova prime \nWill search all buy orders from sellers who are listed as \"Online\". " +
           "Will return all results containing the requested item, then pick the number you want by typing it in channel\nResults will be the top 5 cheapest.")]
        public async Task BuyOrdersSearch([Remainder] string msg)
        {
            string theresp = "";
            var json = File.ReadAllText("SystemLang/Items.json");
            var market = Items.FromJson(json);
            var items = market.Payload.Items.En;
            var embed = new EmbedBuilder();
            embed.WithTitle("Search results");
            embed.WithDescription("Please pick one");
            embed.WithColor(new Color(188, 66, 244));
            int thecount = 1;
            int dacount = 0;
            List<string> theitem = new List<string>();
            List<string> pickeditem = new List<string>();
            for (long i = 0; i < items.Count(); i++)
            {
                var theitems = items[i].ItemName.ToLower();
                if (theitems.Contains(msg.ToLower()))
                {
                    if (dacount == 5)
                    {
                        break;
                    }
                    embed.AddField($"Item {dacount + 1}", theitems, true);
                    int listcount = dacount + 1;
                    theitem.Add(listcount.ToString());
                    pickeditem.Add(theitems);

                    dacount = dacount + 1;
                }
            }

            if (!theitem.Any())
            {
                await Context.Channel.SendMessageAsync(
                      "No orders were found or you mispelled the item you were looking for. Check spelling or try a different word");
                return;
            }


            await Context.Channel.SendMessageAsync("", false, embed.Build());
            var timeout = 30; //seconds the bot will wait
            var interactiveMessage = new InteractiveMessageBuilder("Pick the one you want and reply with item number! You have 30s!") //initial message the bot will send
                .ListenChannels(Context.Channel) //The channels the bot will listen
                .WithTimeSpan(TimeSpan.FromSeconds(timeout)) //Timeout
                .ListenUsers(Context.User) //The users the bot will listen
                .SetWrongResponseMessage("NO! pick one please") // Message sent when a response that isnt in the options is sent
                .SetTimeoutMessage("Command timed out, use the command again") //Message sent on timeout
                .SetOptions(theitem.ToArray()) //Options
                .EnableLoop() //Won't stop asking unless timeout
                .WithCancellationWord("cancel") //if c is sent, will stop
                .SetCancelationMessage("Alright, nvm then") //Message sent if the user cancels
                .Build();

            var response = await StartInteractiveMessage(interactiveMessage);
            for (int i = 0; i < theitem.Count; i++)
            {
                if (theitem[i].Contains(response.ToString()))
                {
                    theresp = theitem[i];

                }
            }
            switch (theresp)
            {
                case "1":
                    theresp = pickeditem[0];
                    break;
                case "2":
                    theresp = pickeditem[1];
                    break;
                case "3":
                    theresp = pickeditem[2];
                    break;
                case "4":
                    theresp = pickeditem[3];
                    break;
                case "5":
                    theresp = pickeditem[4];
                    break;
                default:
                    theresp = "Wrong Choice";
                    await Context.Channel.SendMessageAsync("Wrong option picked");
                    break;
            }
            foreach (var res in items)
            {
                if (res.ItemName.ToLower().Contains(theresp))
                {
                    string apiresponse = "";
                    var theurl = "https://api.warframe.market/v1/items/" + res.UrlName + "/orders";
                    try
                    {

                        using (WebClient client = new WebClient())
                            apiresponse = client.DownloadString(theurl);

                    }
                    catch (WebException e)
                    {
                        await Context.Channel.SendMessageAsync(
                             "There was an error getting the required data from the website, please try again later");
                        return;
                    }

                    if (apiresponse.Contains("Something bad happened"))
                    {
                        await Context.Channel.SendMessageAsync("Error, please try again in a couple minutes!");
                        return;
                    }
                    var orders = WFOrders.FromJson(apiresponse);

                    var blah = orders.Payload.Orders;
                    List<KeyValuePair<string, long>> theOrders = new List<KeyValuePair<string, long>>();
                    foreach (var order in blah)
                    {
                        if (order.OrderType == OrderType.Buy)
                        {
                            theOrders.Add(new KeyValuePair<string, long>(order.User.IngameName,
                                order.Platinum));
                        }
                    }

                    //theOrders.ToArray();
                    var mylist = theOrders.OrderByDescending(x => x.Value).ToList();
                    var embed2 = new EmbedBuilder();
                    embed2.WithTitle("Top 5 Cheapest orders");
                    embed2.WithThumbnailUrl(
                        "http://3rdshifters.org/market.png");

                    embed2.WithColor(new Color(188, 66, 244));
                    for (int i = 0; i < mylist.Count; i++)
                    {
                        string mystring = mylist[i].ToString();
                        var replstr = mystring.Replace("[", "Game Name: **");
                        replstr = replstr.Replace(",", "**\nCost: **");
                        replstr = replstr.Replace("]", "** Platinum");
                        if (i == 5)
                        {
                            break;
                        }
                        embed2.AddField($"Order {i + 1}", $"{replstr} ");
                    }

                    await Context.Channel.SendMessageAsync("**Here are results of your order search**", false, embed2.Build());

                    break;

                }
            }

        }

        [Command("sell orders"), Alias("so")]
        [Remarks("search buy orders for an item from players listed online")]
        [Summary("Example: !sell orders nova prime set\nWill return all orders for nova prime set. Search must be exact part name. If unsure use !market search for partial matches\nResults will be top 5 cheapest.")]
        public async Task MarketCommand([Remainder] string msg)
        {
            var json = File.ReadAllText("SystemLang/Items.json");
            var market = Items.FromJson(json);
            var items = market.Payload.Items.En;
            string theurl = "";
            string apiresponse = "";
            foreach (var item in items)
            {
                var theitem = item.ItemName.ToLower();
                if (theitem == msg.ToLower())
                {
                    theurl = "https://api.warframe.market/v1/items/" + item.UrlName + "/orders";
                    try
                    {
                        if (string.IsNullOrEmpty(theurl))
                        {
                            return;
                        }
                        using (WebClient client = new WebClient())
                            apiresponse = client.DownloadString(theurl);

                    }
                    catch (WebException e)
                    {
                        Console.WriteLine(e);

                    }
                    break;
                }
            }

            if (string.IsNullOrEmpty(apiresponse))
            {
                await Context.Channel.SendMessageAsync("There was an error, or no matches found, please try again! You can also try a partial search using !market search or !ms. !so is for exact matches only.");
                return;
            }
            var orders = WFOrders.FromJson(apiresponse);
            var blah = orders.Payload.Orders;
            List<KeyValuePair<string, long>> theOrders = new List<KeyValuePair<string, long>>();
            foreach (var order in blah)
            {
                if (order.OrderType == OrderType.Sell && order.User.Status == Status.Ingame)
                {
                    theOrders.Add(new KeyValuePair<string, long>($"{order.User.IngameName}", order.Platinum));
                }
            }
            if (!theOrders.Any())
            {
                await Context.Channel.SendMessageAsync(
                    "No orders were found or you mispelled the item you were looking for. Check spelling or try a different search");
                return;
            }
            //theOrders.ToArray();
            var mylist = theOrders.OrderBy(x => x.Value).ToList();
            var embed = new EmbedBuilder();
            embed.WithTitle("Top 5 Cheapest orders");
            embed.WithThumbnailUrl(
                "http://3rdshifters.org/market.png");
            for (int i = 0; i < mylist.Count; i++)
            {
                string mystring = mylist[i].ToString();
                var replstr = mystring.Replace("[", "Username: **");
                replstr = replstr.Replace(",", "**\nCost: **");
                replstr = replstr.Replace("]", "** Platinum");
                if (i == 5)
                {
                    break;
                }
                embed.AddField($"Order {i + 1}", $"{replstr} ");
            }
            embed.WithColor(new Color(188, 66, 244));
            await Context.Channel.SendMessageAsync("", false, embed.Build());
            var top5 = orders.Payload.Orders

               .OrderBy(ord => ord.Platinum)
                .Take(5)
                .Select(acc => acc.Platinum)
                .ToArray();
            var result = new StringBuilder();
            for (int i = 0; i < top5.Length; i++)
            {
                result.Append($"#{i + 1} - {top5[i]}\n");
            }



        }
    }
}