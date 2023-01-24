using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DateCalculator.ViewModel
{
    class DownloadViewModel : BaseViewModel
    {
        public DownloadViewModel() 
        {
            ButtonPress = new RelayCommand(DownloadVideo);
        }

        private string _logTxt;

        public string LogText
        {
            get { return _logTxt; }
            set
            {
                _logTxt = value;
                OnPropertyChanged(nameof(LogText));
            }
        }

        public RelayCommand ButtonPress { get; set; }

        // download video function
        public void DownloadVideo(object obj)
        {
            string YTDL = "/C C:/Users/the-c/Desktop/youtube-downloader/youtube-dl.exe";
            string args = "https://youtu.be/REU8pMbh23I";
            string path = "~/Desktop";
            string save = $"-o {path}/%(title)s.%(ext)s";
            string command = $"{YTDL} {save} {args}"; // {args} or --help

            // laying out cmd process
            Process CMD = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "CMD.exe",
                    Arguments = command,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            };

            // running and closing process
            CMD.Start();
            CMD.WaitForExit();

            LogText += "\n[console] ytdl activated";

            // outputting results
            while (!CMD.StandardOutput.EndOfStream)
            {
                string line = CMD.StandardOutput.ReadLine();
                Debug.WriteLine(line);
                LogText += $"\n{line}";
            }
        }
    }
}
