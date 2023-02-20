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
            YTDLPath = settings.ytdl_path;

            CheckYTDL = new RelayCommand(o => StatusText = settings.CheckYTDL());
            OpenSettings = new RelayCommand(OpenSettingsFolder);
        }

        // @"C:/jwapp", @"C:/jwapp/settings.json", false, @"C:/jwapp/ytdl/youtube-dl.exe"

        public Data settings = new Data() { };

        private string _logTxt, _linkTxt, _statTxt, _ytdlPath, _pathStat;

        public string PathStatus
        {
            get { return _pathStat; }
            set
            {
                _pathStat = value;
                OnPropertyChanged(nameof(PathStatus));
            }
        }

        public string YTDLPath
        {
            get { return _ytdlPath; }
            set
            {
                _ytdlPath = value;
                OnPathChanged();
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

        public RelayCommand CheckYTDL { get; set; }

        public RelayCommand OpenSettings { get; set; }

        public void OpenSettingsFolder(object obj)
        {
            var psi = new System.Diagnostics.ProcessStartInfo() { FileName = settings.app_path, UseShellExecute = true };
            Process.Start(psi);
        }

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