using System;
using Warframebot.Storage;

namespace Warframebot
{
   public class Logger : ILogger
    {
      public void Log(string message)
        {
          Console.WriteLine(message);
            //LogFile(message);
        }

       /* private void LogFile(string message)
        {
            var fileName = $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}.log";
            var folder = Constants.LogFolder;

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            StreamWriter sw = File.AppendText($"{folder}/{fileName}");
            sw.WriteLine($"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Hour}-{DateTime.Today.Minute}" + message );
            sw.Close();
        }
*/
    }

}
