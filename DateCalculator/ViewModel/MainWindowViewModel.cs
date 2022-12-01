using DayOfWeek.ViewModel;
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

            CurrentViewModel = HomeVM;
            WindowPathState = MaxPathData;

            HomeViewCommand = new RelayCommand(o => CurrentViewModel = HomeVM);
            CalculatorViewCommand = new RelayCommand(o => CurrentViewModel = CalcVM);
            CloseApp = new RelayCommand(o => App.Current.Shutdown());
            MinimiseApp = new RelayCommand(o => App.Current.MainWindow.WindowState = WindowState.Minimized);
            MaximiseApp = new RelayCommand(SwitchMaxMin);
        }

        private string MaxPathData = "M 18.5,10.5 H 27.5 V 19.5 H 18.5 Z";
        private string MinPathData = "M 18.5,12.5 H 25.5 V 19.5 H 18.5 Z M 20.5,12.5 V 10.5 H 27.5 V 17.5 H 25.5";

        private string _currentPath;  public string WindowPathState 
        {
            get { return _currentPath; }
            set 
            { 
                _currentPath = value;
                OnCurrentPathChanged();
            }
        }

        private HomeViewModel HomeVM { get; set; } private CalculatorViewModel CalcVM { get; set; }

        private BaseViewModel _currentViewModel; public BaseViewModel CurrentViewModel 
        {
            get 
            { 
                return _currentViewModel;
            }
            set 
            { 
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            } 
        }
        
        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnCurrentPathChanged()
        {
            OnPropertyChanged(nameof(WindowPathState));
        }

        public RelayCommand HomeViewCommand { get; set; } public RelayCommand CalculatorViewCommand { get; set; } public RelayCommand CloseApp { get; set; } public RelayCommand MinimiseApp { get; set; } public RelayCommand MaximiseApp { get; set; }

        private void SwitchMaxMin(object obj)
        {
            if (App.Current.MainWindow.WindowState == WindowState.Normal)
            {
                App.Current.MainWindow.WindowState = WindowState.Maximized;
                WindowPathState = MinPathData;
            } 
            else
            {
                App.Current.MainWindow.WindowState = WindowState.Normal;
                WindowPathState = MaxPathData;
            }
        }
    }
}