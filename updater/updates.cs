using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace updater
{
    public class updates
    {
       public static void RunUpdate(object sender, EventArgs e)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri("https://www.dropbox.com/s/4weg7k4fp41t4u5/Warframebot.exe?dl=0"),
                @"Update/Warframebot.exe");
        }

       public static void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var progressBar = e.ProgressPercentage;
            Console.WriteLine(progressBar.ToString());
        }

      public static  void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (!File.Exists("Update/Warframebot.exe"))
            {
                Console.WriteLine("update failed, restarting current version of bot");
                Process.Start("Warframebot.exe");
                Environment.Exit(0);
            }

           /* File.Move("warframebot.exe", "warframebot.bak");
            File.Move("Update/warframebot.exe", "warframebot.exe");
            File.Delete("warframebot.bak");
            Process.Start("warframebot.exe"); */
            Console.WriteLine("sucess");
        }
    }
}