using DateCalculator.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace DateCalculator.ViewModel
{
    class DownloadViewModel : BaseViewModel
    {
        public DownloadViewModel() 
        {
            ButtonPress = new RelayCommand(CreateSettings);
            CheckYTDL = new RelayCommand(o => StatusText = settings.CheckYTDL());
            settings.SetDefault();
            settings.CreateSettings();
        }

        public Data settings = new Data() { };

        private string _logTxt, _linkTxt, _statTxt;

        public string LogText
        {
            get { return _logTxt; }
            set
            {
                _logTxt = value;
                OnPropertyChanged(nameof(LogText));
            }
        }

        public string LinkText
        {
            get { return _linkTxt; }
            set
            {
                _linkTxt = value;
                OnPropertyChanged(nameof(LinkText));
            }
        }

        public string StatusText
        {
            get { return _statTxt; }
            set
            {
                _statTxt = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }

        readonly Regex regex = new Regex(@"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube(-nocookie)?\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$");

        public RelayCommand ButtonPress { get; set; }

        public RelayCommand CheckYTDL { get; set; }

        public void CreateSettings(object obj)
        {
            try
            {
                // creating settings txt
                string FileName = @"C:\jwapp\settings.json";
                
                // creating directory for app
                /*if (Directory.Exists("C:\\jwapp"))
                {
                    return;
                }
                else
                {
                    Directory.CreateDirectory("C:\\jwapp");
                }

                if (File.Exists(FileName))
                {
                    return;
                }
                else
                {
                    var file = File.Create(FileName);
                }

                File.Create(FileName);*/

                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FileName, json);
            }
            catch
            {
                LogText += "\n[console] directory already exists or error";
            }
            
        }

        // download video function
        public void DownloadVideo(object obj)
        {
            string YTDL = "/C C:/Users/the-c/Desktop/youtube-downloader/youtube-dl.exe";
            string link = "https://youtu.be/REU8pMbh23I";
            string path = "~/Desktop";
            string save = $"-o {path}/%(title)s.%(ext)s";
            string command = $"{YTDL} {save} {link}"; // {args} or --help

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
                    string Line = CMD.StandardOutput.ReadLine();
                    LogText += $"\n{Line}";
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