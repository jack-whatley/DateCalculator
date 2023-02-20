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
        }

        // latest ytdl: pip install git+https://github.com/ytdl-org/youtube-dl.git@master#egg=youtube_dl

        public Data settings = new Data() { };

        private string _logTxt, _linkTxt;

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

        private void OnPathChanged()
        {
            OnPropertyChanged(nameof(YTDLPath));

            // checking ytdl file
            if (File.Exists(YTDLPath))
            {
                // file exists
                PathStatus = "YTDL Found";
                settings.ytdl_status = true;
                settings.ytdl_path = YTDLPath;
                settings.UpdateSettings();
            }
            else
            {
                // file doesnt exist
                PathStatus = "YTDL Not Found";
                settings.ytdl_status = false;
            }
        }

        public RelayCommand OpenSettings { get; set; }

        public RelayCommand DownYTDL { get; set; }

        public RelayCommand DownloadVideo { get; set; }

        public void OpenSettingsFolder(object obj)
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
    }
}