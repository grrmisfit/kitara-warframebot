using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Warframebot.Modules
{
    class Scramble
    {
        public class Categories
        {
            public List<string> Movies { get; set; }
            public List<string> Cars { get; set; }
            public List<string> Music { get; set; }
            public List<string> States { get; set; }
            public List<string> Countries { get; set; }
            public List<string> Electronics { get; set; }
            public List<string> Presidents { get; set; }
            public List<string> Planets { get; set; }
            public List<string> Colors { get; set; }
            public List<string> Holidays { get; set; }
            public List<string> Cities { get; set; }
            public List<string> Actors { get; set; }
            public List<string> Animals { get; set; }
        }

        public static string ScrambleWord(string word)
        {
            ScramData.ScramWord = word;
            string input = word;

            char[] chars = input.ToArray();
            Random r = new Random();
            for (int i = 0; i < chars.Length; i++)
            {
                int randomIndex = r.Next(0, chars.Length);
                char temp = chars[randomIndex];
                chars[randomIndex] = chars[i];
                chars[i] = temp;
            }
            ScramData.ScrambledWord = new string(chars);
            return ScramData.ScrambledWord;
        }
        public static string ScrambleWordMulti(string word)
        {
            
            string input = word;

            char[] chars = input.ToArray();
            Random r = new Random();
            for (int i = 0; i < chars.Length; i++)
            {
                int randomIndex = r.Next(0, chars.Length);
                char temp = chars[randomIndex];
                chars[randomIndex] = chars[i];
                chars[i] = temp;
            }
             
            return new string(chars);
        }
        public static string ScrambleMutiple(string text)
        {
            ScramData.ScramWord = text;
            string whatsaid = text;
            string[] words = whatsaid.Split(' ');
            string tmpword = "";
            foreach (var word in words)
            {
                tmpword = tmpword + " " + ScrambleWordMulti(word);
            }
            ScramData.ScrambledWord = tmpword;
            return tmpword;
        }

        public static string GetScramWord()
        {
            string scraminfo = File.ReadAllText("SystemLang/scramwords.json");
            string daword = "";

            var scramwords = JsonConvert.DeserializeObject<Scramble.Categories>(scraminfo);
            if (ScramData.Random == true)
            {
                Random rancat = new Random();
                int catnum = rancat.Next(12);

                switch (catnum)
                {
                    case 0:
                        int moviecount = rancat.Next(scramwords.Movies.Count);
                        daword = scramwords.Movies[moviecount];
                        ScramData.Category = "Movies";
                        break;

                    case 1:
                        int carcount = rancat.Next(scramwords.Cars.Count);
                        daword = scramwords.Cars[carcount];
                        ScramData.Category = "Cars"; ;
                        break;

                    case 2:
                        int musiccount = rancat.Next(scramwords.Music.Count);
                        daword = scramwords.Music[musiccount];
                        ScramData.Category = "Music";
                        break;

                    case 3:
                        int statescount = rancat.Next(scramwords.States.Count);
                        daword = scramwords.States[statescount];
                        ScramData.Category = "States";
                        break;
                    case 4:
                        int countrycount = rancat.Next(scramwords.Countries.Count);
                        daword = scramwords.Countries[countrycount];
                        ScramData.Category = "Countries";
                        break;
                    case 5:
                        int eleccount = rancat.Next(scramwords.Electronics.Count);
                        daword = scramwords.Electronics[eleccount];
                        ScramData.Category = "Electronics";
                        break;
                    case 6:
                        int presicount = rancat.Next(scramwords.Presidents.Count);
                        daword = scramwords.Presidents[presicount];
                        ScramData.Category = "Presidents";
                        break;
                    case 7:
                        int planetcount = rancat.Next(scramwords.Planets.Count);
                        daword = scramwords.Planets[planetcount];
                        ScramData.Category = "Planets";
                        break;
                    case 8:
                        int colorcount = rancat.Next(scramwords.Colors.Count);
                        daword = scramwords.Colors[colorcount];
                        ScramData.Category = "Colors";
                        break;
                    case 9:
                        int holicount = rancat.Next(scramwords.Holidays.Count);
                        daword = scramwords.Holidays[holicount];
                        ScramData.Category = "Holidays";
                        break;
                    case 10:
                        int citycount = rancat.Next(scramwords.Cities.Count);
                        daword = scramwords.Cities[citycount];
                        ScramData.Category = "Cities";
                        break;
                    case 11:
                        int actorcount = rancat.Next(scramwords.Actors.Count);
                        daword = scramwords.Actors[actorcount];
                        ScramData.Category = "Actors";
                        break;
                    case 12:
                        int animalcount = rancat.Next(scramwords.Animals.Count);
                        daword = scramwords.Animals[animalcount];
                        ScramData.Category = "Animals";
                        break;
                }

                if (daword.Contains(" ") == true)
                {

                    ScramData.ScrambledWord = ScrambleMutiple(daword);
                    
                }
                else
                {
                    ScramData.ScrambledWord = ScrambleWord(daword);
                    
                }

            }
            return ScramData.ScrambledWord;


        }
    }
}
