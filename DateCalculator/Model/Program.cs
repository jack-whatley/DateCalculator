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
        /// Tomohiko Sakamoto Algorithm for Gregorian
        /// </summary>

        public int GetDayOfWeekGregorian(string year, string month, string day)
        {
            int[] days = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
            
            int.TryParse(year, out int YearParsed);
            int.TryParse(month, out int MonthParsed);
            int.TryParse(day, out int DayParsed);

            MonthParsed++; DayParsed++;
            if (MonthParsed < 3) YearParsed--;

            int result = (YearParsed + YearParsed / 4 - YearParsed / 100 + YearParsed / 400 + days[MonthParsed - 1] + DayParsed) % 7;

            return result;
        }

        public int GetDayofWeekJulian(string year, string month, string day)
        {

            return 1;
        }
    }
}