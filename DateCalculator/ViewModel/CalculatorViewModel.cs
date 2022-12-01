using DateCalculator.Model;
using DayOfWeek.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace DateCalculator.ViewModel
{
    class CalculatorViewModel : BaseViewModel
    {
        public CalculatorViewModel()
        {
            DayList = new string[] {"1"};
            ButtonState = false;
            Submit = new RelayCommand(o => { DayOutput = YearInput + " " + (int.Parse(MonthInput) + 1).ToString() + " " + (int.Parse(DayInput) + 1).ToString(); });
        }

        private string[] _dayList; public string[] DayList
        {
            get { return _dayList; }
            set
            {
                _dayList = value;
                OnPropertyChanged(nameof(DayList));
            }
        }

        private string _dayOut, _calOut, _yearInp, _monthInp, _dayInp;

        private bool _buttonState;
        
        public string DayOutput
        {
            get { return _dayOut; }
            set
            {
                _dayOut = value;
                OnPropertyChanged(nameof(DayOutput));
            }
        }

        public string CalOutput
        {
            get { return _calOut; }
            set
            {
                _calOut = value;
                OnPropertyChanged(nameof(CalOutput));
            }
        }
    
        public string YearInput
        {
            get { return _yearInp; }
            set
            {
                _yearInp = value;
                OnPropertyChanged(nameof(YearInput));
                if (SanitiseYear())
                {
                    ButtonState = true;
                }
                else
                {
                    ButtonState = false;
                }
            }
        }

        public string MonthInput
        {
            get { return _monthInp; }
            set
            {
                _monthInp = value;
                OnPropertyChanged(nameof(MonthInput));
                // method for updating selectable days
                var Program = new Program();
                switch (MonthInput)
                {
                    case "0": // jan
                        DayList = Program.GetRange(31);
                        break;
                    case "1": // feb
                        DayList = Program.GetRange(28);
                        break;
                    case "2": // mar
                        DayList = Program.GetRange(31);
                        break;
                    case "3": // apr
                        DayList = Program.GetRange(30);
                        break;
                    case "4": // may
                        DayList = Program.GetRange(31);
                        break;
                    case "5": // jun
                        DayList = Program.GetRange(30);
                        break;
                    case "6": // jul
                        DayList = Program.GetRange(31);
                        break;
                    case "7": // aug
                        DayList = Program.GetRange(31);
                        break;
                    case "8": // sep
                        DayList = Program.GetRange(30);
                        break;
                    case "9": // oct
                        DayList = Program.GetRange(31);
                        break;
                    case "10": // nov
                        DayList = Program.GetRange(30);
                        break;
                    case "11": // dec
                        DayList = Program.GetRange(31);
                        break;
                }
            }
        }

        public string DayInput
        {
            get { return _dayInp; }
            set
            {
                _dayInp = value;
                OnPropertyChanged(nameof(DayInput));
            }
        }

        public bool ButtonState
        {
            get { return _buttonState; }
            set
            {
                _buttonState = value;
                OnPropertyChanged(nameof(ButtonState));
                OnPropertyChanged(nameof(YearInput));
            }

        }

        public RelayCommand Submit { get; set; }

        private bool SanitiseYear()
        {
            var Program = new InputSanitisationAlgorithms();

            if (!string.IsNullOrEmpty(YearInput))
            {
                if (Program.SanitiseYear(YearInput))
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
    }

    class InputSanitisationAlgorithms
    {
        public InputSanitisationAlgorithms() { }
        public bool SanitiseYear(string year)
        {
            int v = int.Parse(year);
            if (v > 9999 && v < 9999)
            {
                return true;
            }
            return false;
        }
    }
}
