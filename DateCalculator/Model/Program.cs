using DateCalculator.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DateCalculator.Model
{
    public class Program
    {
        public Program() { }

        public string[] GetRange(int NumDays)
        {
            string[] AllDays = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
            string[] ArrayFinal = new string[NumDays];
            for (int i = 0; i < NumDays; i++)
            {
                ArrayFinal[i] = AllDays[i];
            }
            return ArrayFinal;
        }

        /// <summary>
        /// Calculation for Gregorian calendar
        /// k = day of month, m = month number (march = 1, feb = 12)
        /// D = last two digits of year, C = first two digits of year
        /// sunday = 0
        /// f = k + [(13*m-1)/5] + D + [D/4] + [C/4] - 2*C
        /// </summary>

        public decimal GetYearDigits(string year, bool FirstLast)
        {
            // FirstLast = true => first two digits 
            string FirstTwo = year.Substring(0, 2);
            string LastTwo = year.Substring(2, 2);

            if (FirstLast)
            {
                int.TryParse(FirstTwo, out int FirstTwoDigits);
                return FirstTwoDigits;
            }
            else
            {
                int.TryParse(LastTwo, out int LastTwoDigits);
                return LastTwoDigits;
            }
        }

        public decimal GetMonthCode(string month)
        {
            int.TryParse(month, out int MonthParsed);
            MonthParsed++;
            switch (MonthParsed)
            {
                case 1: // jan
                    return 11;
                case 2: // feb
                    return 12;
                case 3: // mar
                    return 1;
                case 4:
                    return 2;
                case 5:
                    return 3;
                case 6:
                    return 4;
                case 7:
                    return 5;
                case 8:
                    return 6;
                case 9:
                    return 7;
                case 10:
                    return 8;
                case 11:
                    return 9;
                case 12:
                    return 10;
                default:
                    return 0;
            }
        }

        public int GetDayOfWeek(string year, string month, string day)
        {
            decimal FirstDigits = GetYearDigits(year, true);
            decimal LastDigits = GetYearDigits(year, false);
            decimal MonthCode = GetMonthCode(month);
            int DayParsed = int.Parse(day);
            DayParsed++;

            decimal result = DayParsed + Math.Truncate((13 * (MonthCode - 1)) / 5) + LastDigits + 
                Math.Truncate(LastDigits / 4) + Math.Truncate(FirstDigits / 4) - (2 * FirstDigits);

            int FinalResult = Convert.ToInt32(Math.Truncate(result));

            FinalResult = (int)(7 * Math.Truncate(result / 7));

            return FinalResult;
        }
    }
}