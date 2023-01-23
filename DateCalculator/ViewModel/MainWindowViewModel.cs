using DateCalculator.ViewModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DateCalculator.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            HomeVM = new HomeViewModel();
            CalcVM = new CalculatorViewModel();
            DownVM = new DownloadViewModel();

            CurrentViewModel = HomeVM;
            WindowPathState = "M 18.5,10.5 H 27.5 V 19.5 H 18.5 Z";

            HomeViewCommand = new RelayCommand(o => CurrentViewModel = HomeVM);
            CalculatorViewCommand = new RelayCommand(o => CurrentViewModel = CalcVM);
            DownloadViewCommand = new RelayCommand(o => CurrentViewModel = DownVM);
            CloseApp = new RelayCommand(o => App.Current.Shutdown());
            MinimiseApp = new RelayCommand(o => App.Current.MainWindow.WindowState = WindowState.Minimized);
            MaximiseApp = new RelayCommand(SwitchMaxMin);
        }

        private string _currentPath;  
        
        public string WindowPathState 
        {
            get { return _currentPath; }
            set 
            { 
                _currentPath = value;
                OnPropertyChanged(nameof(WindowPathState));
            }
        }

        private HomeViewModel HomeVM { get; set; } 
        private CalculatorViewModel CalcVM { get; set; }
        private DownloadViewModel DownVM { get; set; }

        private BaseViewModel _currentViewModel; 
        
        public BaseViewModel CurrentViewModel 
        {
            get 
            { 
                return _currentViewModel;
            }
            set 
            { 
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            } 
        }

        public RelayCommand HomeViewCommand { get; set; } 
        public RelayCommand CalculatorViewCommand { get; set; } 
        public RelayCommand DownloadViewCommand { get; set; }
        public RelayCommand CloseApp { get; set; } 
        public RelayCommand MinimiseApp { get; set; } 
        public RelayCommand MaximiseApp { get; set; }

        private void SwitchMaxMin(object obj)
        {
            if (App.Current.MainWindow.WindowState == WindowState.Normal)
            {
                App.Current.MainWindow.WindowState = WindowState.Maximized;
                WindowPathState = "M 18.5,12.5 H 25.5 V 19.5 H 18.5 Z M 20.5,12.5 V 10.5 H 27.5 V 17.5 H 25.5";
            } 
            else
            {
                App.Current.MainWindow.WindowState = WindowState.Normal;
                WindowPathState = "M 18.5,10.5 H 27.5 V 19.5 H 18.5 Z";
            }
        }
    }
}