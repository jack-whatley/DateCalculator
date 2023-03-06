using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace DateCalculator.ViewModel
{
	class HomeViewModel : BaseViewModel
	{	
		public HomeViewModel() 
		{
			// setting up command
            YTDLLink = new RelayCommand(OpenLinkFunction);
		}

        // relay command

        public RelayCommand YTDLLink { get; set; }
        
        // relay command function

        private void OpenLinkFunction(object obj)
        {
            string url = "https://github.com/ytdl-org/youtube-dl";

            // using cmd to open default browser
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}
