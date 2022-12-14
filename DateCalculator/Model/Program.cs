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
        /// Tomohiko Sakamoto Algorithm for Gregorian and Zellers Congruence for Julian.
        /// </summary>

        public int GetDayOfWeekGregorian(string year, string month, string day)
        {
            int[] days = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
            
            int.TryParse(year, out int YearParsed);
            int.TryParse(month, out int MonthParsed);
            int.TryParse(day, out int DayParsed);

            MonthParsed++; DayParsed++;
            if (MonthParsed < 3) YearParsed--;
            if (YearParsed == 1752 && MonthParsed == 9 && DayParsed >= 3) DayParsed += 11; // fix for sept 1752

            int result = (YearParsed + YearParsed / 4 - YearParsed / 100 + YearParsed / 400 + days[MonthParsed - 1] + DayParsed) % 7;

            return result;
        }

        public int GetDayofWeekJulian(string year, string month, string day)
        {
            // Method for calculating Julian.
            // https://en.wikipedia.org/wiki/Zeller%27s_congruence

            int.TryParse(year, out int YearParsed);
            int.TryParse(month, out int MonthParsed);
            int.TryParse(day, out int DayParsed);

            MonthParsed++; DayParsed++;

            if (MonthParsed < 3) MonthParsed += 12; // for jan/feb it is month 13/14
            int Y = YearParsed - 1;

            int result = (DayParsed + (13 * (MonthParsed - 2) / 5) + 2 + Y + Y / 4 + 5) % 7; // sunday is 0 version

            return result;
        }
    }
}