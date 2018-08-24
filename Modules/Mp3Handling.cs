using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
namespace Warframebot.Modules
{
    class Mp3Handling
    {
        
        /*public static Mp3List CheckForNewSong(string songname)
        {
            string dafile = System.IO.File.ReadAllText("h:\\mp3.json");


            var songnames = Mp3List.FromJson(dafile);
            var result = from a in songnames
                         where a.Value.FileName == songname
                         select a;

            var account = result.FirstOrDefault();
            if (account == null) account = AddMp3toFile(songname);
            return account;
        }

        private static object AddMp3toFile(string songname)
        {
            throw new NotImplementedException();
        }*/
    }
}
