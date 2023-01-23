using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DateCalculator.ViewModel
{
    class DownloadViewModel : BaseViewModel
    {
        public DownloadViewModel() 
        {
            ButtonPress = new RelayCommand(DownloadVideo);
        }

        public RelayCommand ButtonPress { get; set; }

        public void DownloadVideo(object obj)
        {
            string YTDL = "/C C:/Users/the-c/Desktop/youtube-downloader/youtube-dl.exe";
            string args = "https://youtu.be/REU8pMbh23I";
            string command = $"{YTDL} -o ~/Desktop/%(title)s.%(ext)s {args}"; // {args} or --help

            ProcessStartInfo psi = new ProcessStartInfo("cmd", "/c" + command);

            try
            {
                System.Diagnostics.Process.Start("cmd.exe", command);
                /*using(Process process = new Process())
                {
                    process.StartInfo = psi;
                    process.Start();

                    process.WaitForExit();
                }*/
            }
            catch
            {
                Debug.WriteLine("ytdl failed");
            }
        }
    }
}
