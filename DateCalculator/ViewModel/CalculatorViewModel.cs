﻿using DateCalculator.Model;
using DayOfWeek.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            Submit = new RelayCommand(CanSubmit, o => { DayOutput = YearInput + " " + (int.Parse(MonthInput) + 1).ToString() + " " + (int.Parse(DayInput) + 1).ToString(); });
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

        private void OnYearChanged() { OnPropertyChanged(nameof(_yearInp)); Submit.RaiseCanExecuteChanged(); }

        private void OnMonthChanged()
        {
            OnPropertyChanged(nameof(_monthInp));
            // method for updating selectable days
            Submit.RaiseCanExecuteChanged();
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

        private void OnDayChanged() { OnPropertyChanged(nameof(_dayInp)); Submit.RaiseCanExecuteChanged(); }

        public RelayCommand Submit { get; set; }

        public bool CanSubmit(object obj)
        {
            var Program = new InputSanitisationAlgorithms();

            if (!string.IsNullOrEmpty(YearInput) && Program.SanitiseYear(YearInput))
            {
                if (!string.IsNullOrEmpty(MonthInput) && Program.SanitiseMonth(MonthInput))
                {
                    if (!string.IsNullOrEmpty(DayInput))
                    {
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
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
            int.TryParse(year, out int v);
            if (v > 1752 && v < 9999) // only gregor for now
            {
                return true;
            }
            return false;
        }

        public bool SanitiseMonth(string month)
        { 
            int.TryParse(month, out int monthConverted);

            monthConverted++;

            if (monthConverted > 0 && monthConverted < 13)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            else { return false; }
        }
    }
}