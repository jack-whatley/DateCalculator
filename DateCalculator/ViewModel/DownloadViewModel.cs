using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

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
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            };

            try
            {
                // running and closing process
                CMD.Start();
                CMD.WaitForExit();

                LogText += "[console] ytdl activated";

                // outputting results
                while (!CMD.StandardOutput.EndOfStream)
                {
                    string line = CMD.StandardOutput.ReadLine();
                    LogText += $"\n{line}";
                }

                while (!CMD.StandardError.EndOfStream)
                {
                    string ErrorLine = CMD.StandardError.ReadLine();
                    LogText += $"\n{ErrorLine}";
                }
            }
            catch
            {
                LogText += "[console] ytdl error";
            }
        }
    }
}
