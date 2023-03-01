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
            // need to set default every time to find paths, can then overwrite if settings are loaded
            settings.SetDefault();

            // checks if it should load or remake settings
            if (settings.CheckSettings())
            {
                string JsonString = File.ReadAllText(settings.app_settings_path);

                // TODO: better error handling
                try
                {
                    settings = JsonSerializer.Deserialize<YTDLSettings>(JsonString);
                }
                catch
                {
                    LogText += "[console] settings error, recreating";
                    settings.CreateSettings();
                }
            }
            else
            {
                settings.CreateSettings();
            }

            // setting value of output location for user
            OutLoc = settings.output_location;

            // setting up relay commands
            OpenSettings = new RelayCommand(OpenSettingsFolder);
            DownVideo = new RelayCommand(CheckLink, DownloadVideo);
            DownYTDL = new RelayCommand(DownloadYTDL);
            OpenLink = new RelayCommand(OpenLinkFunction);
        }

        // creating data class
        
        public YTDLSettings settings = new YTDLSettings() { };

        // propfulls

        private string _logTxt, _linkTxt, _outTxt;

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
                // update command ability to execute
                DownVideo.RaiseCanExecuteChanged();
            }
        }

        public string OutLoc
        {
            get { return _outTxt; }
            set 
            { 
                _outTxt = value; 
                OnPropertyChanged(nameof(OutLoc));
                
                // updating output location
                settings.output_location = OutLoc;
                
                // saving new settings
                settings.UpdateSettings();
            }
        }

        // relay commands

        public RelayCommand OpenSettings { get; set; }

        public RelayCommand DownYTDL { get; set; }

        public RelayCommand DownVideo { get; set; }

        public RelayCommand OpenLink { get; set; }

        // relay command functions

        private bool CheckLink(object obj)
        {
            // youtube link regex
            Regex regex = new Regex(@"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube(-nocookie)?\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$");

            // null into regex causes error
            if (!string.IsNullOrEmpty(LinkText))
            {
                if (regex.IsMatch(LinkText))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void OpenSettingsFolder(object obj)
        {
            var SettingFolder = new System.Diagnostics.ProcessStartInfo() { FileName = settings.app_path, UseShellExecute = true };
            Process.Start(SettingFolder);
        }

        private void DownloadVideo(object obj)
        {
            // usage example: python -m youtube_dl -o "~/Desktop/%(title)s.%(ext)s" https://www.youtube.com/watch?v=TGqWphOB9io
            string YTDL = "/C python -m youtube_dl";
            string path = $"-o {settings.output_location}";
            string URL = LinkText;
            string command = $"{YTDL} {path} {URL}";

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
                // running process
                CMD.Start();

                // ending process
                CMD.WaitForExit();

                LogText += "[console] ytdl activated";

                // outputting results
                while (CMD.StandardOutput.ReadLine() != null)
                {
                    LogText += $"\n{CMD.StandardOutput.ReadLine()}";
                }

                while (CMD.StandardError.ReadLine() != null)
                {
                    LogText += $"\n{CMD.StandardError.ReadLine()}";
                }
            }
            catch
            {
                LogText += "[console] ytdl error";
            }
        }

        private void DownloadYTDL(object obj)
        {
            // downloads latest version of master branch as youtube_dl
            string PIP = "/K pip install git+https://github.com/ytdl-org/youtube-dl.git@master#egg=youtube_dl";

            Process CMD = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "CMD.exe",
                    Arguments = PIP,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = false,
                    UseShellExecute = true,
                }
            };

            try
            {
                CMD.Start();
                //CMD.WaitForExit();

                LogText += "[console] pip activated, check cmd window for installation status";
            }
            catch
            {
                LogText += "[console] pip / python error, check if its installed";
            }
        }

        private void OpenLinkFunction(object obj) 
        {
            string url = "https://github.com/ytdl-org/youtube-dl#output-template";
            
            // using cmd to open default browser
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}