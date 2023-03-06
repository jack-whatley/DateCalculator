using DateCalculator.Model;
using DateCalculator.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace DateCalculator.ViewModel
{
    class CalculatorViewModel : BaseViewModel
    {
        // TODO: Fix Submit/Clear Button

        public CalculatorViewModel()
        {
            // default button text
            ButtonText = "Submit";

            // setting up relay command
            Submit = new RelayCommand(CanSubmit, ExecuteSubmit);
        }

        // props

        private string[] _dayList;

        private string _dayOut, _calOut, _yearInp, _monthInp, _dayInp, _inpOut, _butText;

        public string[] DayList
        {
            get { return _dayList; }
            set
            {
                _dayList = value;
                OnPropertyChanged(nameof(DayList));
            }
        }

        public string ButtonText
        {
            get { return _butText; }
            set
            {
                _butText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public string InputOutput
        {
            get { return _inpOut; }
            set
            {
                _inpOut = value;
                OnPropertyChanged(nameof(InputOutput));
            }
        }

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
                OnYearChanged();
            }
        }

        public string MonthInput
        {
            get { return _monthInp; }
            set
            {
                _monthInp = value;
                OnMonthChanged();
            }
        }

        public string DayInput
        {
            get { return _dayInp; }
            set
            {
                _dayInp = value;
                OnDayChanged();
            }
        }

        private string _yearSaved, _monthSaved, _daySaved;

        public string YearSaved
        {
            get { return _yearSaved; }
            set 
            { 
                _yearSaved = value;
                OnPropertyChanged(nameof(YearSaved));
            }
        }

        public string MonthSaved
        {
            get { return _monthSaved; }
            set
            {
                _monthSaved = value;
                OnPropertyChanged(nameof(MonthSaved));
            }
        }

        public string DaySaved
        {
            get { return _daySaved; }
            set
            {
                _daySaved = value;
                OnPropertyChanged(nameof(DaySaved));
            }
        }

        // update functions

        private void OnYearChanged()
        {
            OnPropertyChanged(nameof(YearInput));
            Submit.RaiseCanExecuteChanged();
            SetDayList();
        }

        private void OnMonthChanged()
        {
            OnPropertyChanged(nameof(MonthInput));
            Submit.RaiseCanExecuteChanged();
            // method for updating selectable days
            SetDayList();
        }

        private void OnDayChanged()
        {
            OnPropertyChanged(nameof(DayInput));
            Submit.RaiseCanExecuteChanged();
        }

        // relay command

        public RelayCommand Submit { get; set; }

        // relay command functions

        public void SetDayList()
        {
            var Program = new DateCalculatorProgram();
            var LeapCalc = new InputSanitisationAlgorithms();

            if (!string.IsNullOrEmpty(YearInput))
            {
                switch (MonthInput)
                {
                    case "0": // jan
                        DayList = Program.GetRange(31);
                        break;
                    case "1": // feb
                        if (LeapCalc.GetLeapYear(YearInput))
                        {
                            DayList = Program.GetRange(29);
                        }
                        else
                        {
                            DayList = Program.GetRange(28);
                        }
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
                        if (YearInput == "1752")
                        {
                            string[] SpecialCase = new string[] { "1", "2", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
                            DayList = SpecialCase;
                        }
                        else
                        {
                            DayList = Program.GetRange(30);
                        }
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

        public void SubmitCalendar()
        {
            var Program = new DateCalculatorProgram();
            var InputAlgo = new InputSanitisationAlgorithms();

            int ProgramResult;

            if (InputAlgo.GetCalendarType(YearInput, MonthInput, DayInput))
            {
                ProgramResult = Program.GetDayOfWeekGregorian(YearInput, MonthInput, DayInput);
                CalOutput = "Gregorian";
            }
            else
            {
                ProgramResult = Program.GetDayofWeekJulian(YearInput, MonthInput, DayInput);
                CalOutput = "Julian";
            }

            DayOutput = ProgramResult switch
            {
                0 => "Sunday",
                1 => "Monday",
                2 => "Tuesday",
                3 => "Wednesday",
                4 => "Thursday",
                5 => "Friday",
                6 => "Saturday",
                _ => "Fail",
            };

            // 1752 is special case due to index method for month and day
            // month and day are -1 due to array starting at 0
            // +12 for jump to 14th sept from 2nd
            if (YearInput == "1752" && MonthInput == "8" && int.Parse(DayInput) >= 2)
            {
                InputOutput = $"{int.Parse(DayInput) + 12} / {int.Parse(MonthInput) + 1} / {YearInput}";
            }
            else InputOutput = $"{int.Parse(DayInput) + 1} / {int.Parse(MonthInput) + 1} / {YearInput}";
        }

        public void SubmitClear()
        {
            YearInput = "";
            MonthInput = "-1";
            DayList = new string[] { };

            DayOutput = "";
            InputOutput = "";
            CalOutput = "";
        }

        public void SaveInput() 
        {
            YearSaved = YearInput;
            MonthSaved = MonthInput;
            DaySaved = DayInput;
        }

        public bool CheckDifference()
        {
            if (YearSaved == YearInput)
            {
                if (MonthSaved == MonthInput)
                {
                    if (DaySaved == DayInput)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CanSubmit(object obj)
        {
            // if its the same it clears, else stops clear
            if (ButtonText == "Clear")
            {
                if (CheckDifference()) return true;
                else
                {
                    ButtonText = "Submit";
                }
            }

            var Program = new InputSanitisationAlgorithms();

            // if string is not empty && year is within range (same for month, day)
            if (!string.IsNullOrEmpty(YearInput) && Program.SanitiseYear(YearInput))
            {
                if (!string.IsNullOrEmpty(MonthInput) && Program.SanitiseMonth(MonthInput))
                {
                    if (!string.IsNullOrEmpty(DayInput) && Program.SanitiseDay(DayInput))
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public void ExecuteSubmit(object obj)
        {
            if (ButtonText == "Submit")
            {
                SubmitCalendar();
                SaveInput();
                ButtonText = "Clear";
            }
            else
            {
                SubmitClear();
                ButtonText = "Submit";
            }
        }
    }
}