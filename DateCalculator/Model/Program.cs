using DateCalculator.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace DateCalculator.Model
{
    public class DateCalculatorProgram
    {
        /// <summary>
        /// Tomohiko Sakamoto Algorithm for Gregorian and Zellers Congruence for Julian.
        /// </summary>

        public DateCalculatorProgram() { }

        public string[] GetRange(int NumDays)
        {
            string[] DayArray = new string[NumDays];

            for (int i = 0; i < NumDays; i++)
            {
                DayArray[i] = (i + 1).ToString();
            }

            return DayArray;
        }

        public int GetDayOfWeekGregorian(string year, string month, string day)
        {
            int[] days = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
            
            int.TryParse(year, out int YearParsed);
            int.TryParse(month, out int MonthParsed);
            int.TryParse(day, out int DayParsed);

            // due to arrays starting at 0
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

    public class YTDLSettings
    {
        /// <summary>
        /// Settings Class for YT_DL.
        /// </summary>

        public YTDLSettings() { }

        public string app_path { get; set; }

        public string app_settings_path { get; set; }

        public string output_location { get; set; }

        public void SetDefault()
        {
            // app root directory
            this.app_path = $"{Environment.GetFolderPath(Environment.SpecialFolder.System)[..2]}/jwapp";
            this.app_settings_path = this.app_path + @"/settings.json";

            // works on ytdl logic not c#
            this.output_location = @"~/Desktop/%(title)s.%(ext)s";
        }

        public void CreateSettings()
        {
            if (!Directory.Exists(this.app_path))
            {
                Directory.CreateDirectory(this.app_path);
            }

            // File.WriteAllText handles creation if doesnt exist (which is a given in this instance)
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(app_settings_path, json);
        }

        public bool CheckSettings()
        {
            if (File.Exists(this.app_settings_path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateSettings()
        {
            var json = JsonSerializer.Serialize<YTDLSettings>(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(app_settings_path, json);
        }
    }

    public class InputSanitisationAlgorithms
    {
        public InputSanitisationAlgorithms() { }

        public bool SanitiseYear(string year)
        {
            int.TryParse(year, out int YearParsed);
            
            if (YearParsed > 0 && YearParsed <= 9999)
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
            else return false;
        }

        public bool SanitiseDay(string day)
        {
            int.TryParse(day, out int DayParsed);

            DayParsed++;

            // has to be 33 not 32 in case of 31st
            if (DayParsed > 0 && DayParsed < 32)
            {
                return true;
            }
            else return false;
        }

        public bool GetLeapYear(string year)
        {
            int.TryParse(year, out int yearConverted);

            // true if leap
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
            /// Returns true if the calendar type is Gregorian and false for Julian. Based on Calendars in the UK.
            /// </summary>

            int.TryParse(year, out int YearParsed);
            int.TryParse(month, out int MonthParsed);
            int.TryParse(day, out int DayParsed);

            // Index for dropdowns starts from zero.
            DayParsed++; MonthParsed++;

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
                    if (DayParsed <= 2)
                    {
                        return false;
                    }
                    else if (DayParsed >= 3)
                    {
                        // accounting for missing days
                        DayParsed += 11;
                        
                        if (DayParsed > 13)
                        {
                            return true;
                        }
                        else return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else return false;
        }
    }
}