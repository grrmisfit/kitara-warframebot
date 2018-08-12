using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace warframebot.Modules
{
    class Scramble
    {
       

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

            /* char[] chars = new char[word.Length];
             Random rand = new Random(10000);
             int index = 0;
             while (word.Length > 0)
             { // Get a random number between 0 and the length of the word. 
                 int next = rand.Next(0, word.Length - 1); // Take the character from the random position 
                                                           //and add to our char array. 
                 chars[index] = word[next];                // Remove the character from the word. 
                 word = word.Substring(0, next) + word.Substring(next + 1);
                 ++index;
             }

             return new String(chars);
         }*/
        }
    }
}
