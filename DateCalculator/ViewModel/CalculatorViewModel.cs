﻿using DateCalculator.Model;
using DayOfWeek.ViewModel;
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
        public CalculatorViewModel()
        {
            DayList = new string[] {"1"};
            Submit = new RelayCommand(CanSubmit, ExecuteSubmit);
        }

        private IEnumerable<string> _dayList; public IEnumerable<string> DayList
        {
            get { return _dayList; }
            set
            {
                _dayList = value;
                OnPropertyChanged(nameof(DayList));
            }
        }

        private string _dayOut, _calOut, _yearInp, _monthInp, _dayInp;
        
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

        private void OnYearChanged() { OnPropertyChanged(nameof(_yearInp)); Submit.RaiseCanExecuteChanged(); SetDayList(); }

        private void OnMonthChanged()
        {
            OnPropertyChanged(nameof(_monthInp));
            // method for updating selectable days
            Submit.RaiseCanExecuteChanged();
            SetDayList();
        }

        private void OnDayChanged() { OnPropertyChanged(nameof(_dayInp)); Submit.RaiseCanExecuteChanged(); }

        public RelayCommand Submit { get; set; }

        public void SetDayList()
        {
            var Program = new Program();
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
                            List<string> SpecialCase = new List<string> { "1", "2", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
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

        public bool CanSubmit(object obj)
        {
            var Program = new InputSanitisationAlgorithms();

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
            var Program = new Program();

            int ProgramResult = Program.GetDayOfWeekGregorian(YearInput, MonthInput, DayInput);

            switch (ProgramResult)
            {
                case 0:
                    DayOutput = "Sunday";
                    break;
                case 1:
                    DayOutput = "Monday";
                    break;
                case 2:
                    DayOutput = "Tuesday";
                    break;
                case 3:
                    DayOutput = "Wednesday";
                    break;
                case 4:
                    DayOutput = "Thursday";
                    break;
                case 5:
                    DayOutput = "Friday";
                    break;
                case 6:
                    DayOutput = "Saturday";
                    break;
            }
        }
    }

    class InputSanitisationAlgorithms
    {
        public InputSanitisationAlgorithms() { }

        public bool SanitiseYear(string year)
        {
            int.TryParse(year, out int YearParsed);
            if (YearParsed >= 1752 && YearParsed < 9999) // only gregor for now
            {
                return true;
            }
            return false;
        }

        public bool SanitiseMonth(string month)
        {
            int.TryParse(month, out int MonthParsed);

            MonthParsed++;

            if (MonthParsed > 0 && MonthParsed < 13)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SanitiseDay(string day)
        {
            int.TryParse(day, out int DayParsed);

            if (DayParsed > 0 && DayParsed < 32)
            {
                return true;
            }
            else return false;
        }

        public bool GetLeapYear(string year)
        {
            int.TryParse(year, out int yearConverted);

            if (yearConverted % 4 == 0)
            {
                return true;
            }
            else if (yearConverted % 400 == 0)
            {
                return true;
            }
            else if (yearConverted % 100 == 0)
            {
                return false;
            }
            else return false;
        }

        public bool GetCalendarType(string year, string month, string day)
        {
            /// <summary>
            /// Returns true if the calendar type is Gregorian and false for Julian.
            /// </summary>

            int.TryParse(year, out int YearParsed);
            int.TryParse(month, out int MonthParsed);
            int.TryParse(day, out int DayParsed);

            if (YearParsed > 1752)
            {
                return true;
            }
            else if (YearParsed == 1752)
            {
                if (MonthParsed < 9) // before september = julian
                {
                    return false;
                }
                else if (MonthParsed == 9)
                {
                    if (DayParsed < 2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
