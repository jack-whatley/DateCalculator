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
                settings = JsonSerializer.Deserialize<Data>(JsonString);
            }
            else
            {
                settings.CreateSettings();
            }

            OpenSettings = new RelayCommand(OpenSettingsFolder);
            DownloadVideo = new RelayCommand(CheckLink, StartDownload);
            DownYTDL = new RelayCommand(DownloadYTDL);
            //CheckYTDL = new RelayCommand(o => DownYTDL.RaiseCanExecuteChanged());
        }

        public Data settings = new Data() { };

        private string _logTxt, _linkTxt, _statText;

        public string StatusText
        {
            get { return _statText; }
            set
            {
                _statText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }

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
                DownloadVideo.RaiseCanExecuteChanged();
            }
        }

        private bool CheckLink(object obj)
        {
            // youtube link regex
            Regex regex = new Regex(@"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube(-nocookie)?\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$");

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

        private bool CheckYTDLInstallation(object obj)
        {
            string PIPList = "pip list";

            Process CMD = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "CMD.exe",
                    Arguments = PIPList,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            };

            try
            {
                CMD.Start();
                CMD.WaitForExit();

                while (!CMD.StandardOutput.EndOfStream)
                {
                    string Line = CMD.StandardOutput.ReadLine();
                    PipOutput += $"{Line} ";
                }
            }
            catch 
            {
                LogText += "[console] pip / python error, check if its installed";
            }

            // should appear in pip list
            if (PipOutput.Contains("youtube-dl"))
            {
                // if it appears there is no need to reinstall
                return false;
            }
            else
            {
                return true;
            }
        }

        public RelayCommand OpenSettings { get; set; }

        public RelayCommand DownYTDL { get; set; }

        public RelayCommand DownloadVideo { get; set; }

        public RelayCommand CheckYTDL { get; set; }

        private void OpenSettingsFolder(object obj)
        {
            var SettingFolder = new System.Diagnostics.ProcessStartInfo() { FileName = settings.app_path, UseShellExecute = true };
            Process.Start(SettingFolder);
        }

        private void StartDownload(object obj)
        {
            // usage example: python -m youtube_dl -o "~/Desktop/%(title)s.%(ext)s" https://www.youtube.com/watch?v=TGqWphOB9io
            string YTDL = "/C python -m youtube_dl";
            string path = @"-o ~/Desktop/%(title)s.%(ext)s";
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

            /* out of date method
            string YTDL = $"/C {settings.ytdl_path}";
            string link = LinkText;
            string path = "~/Desktop"; // folder its saved in
            string save = $"-o {path}/%(title)s.%(ext)s"; // the way its saved
            string command = $"{YTDL} {save} {link}";
            -o ~/Desktop/%(title)s.%(ext)s
             */
        }

        public string PipOutput = "";

        private void DownloadYTDL(object obj)
        {
            // downloads latest version of master branch as youtube_dl
            string PIP = "/C pip install git+https://github.com/ytdl-org/youtube-dl.git@master#egg=youtube_dl";

            Process CMD = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "CMD.exe",
                    Arguments = PIP,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            };

            try
            {
                CMD.Start();
                CMD.WaitForExit();

                LogText += "[console] pip activated";

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
                LogText += "[console] pip / python error, check if its installed";
            }
        }
    }
}