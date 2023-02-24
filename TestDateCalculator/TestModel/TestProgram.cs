using DateCalculator.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TestDateCalculator.TestModel
{
    [TestClass]
    public class TestYTDLSettings
    {
        /// <summary>
        /// Test class for YTDL settings class.
        /// </summary>

        // creating a new YTDLSettings class for testing purposes

        public TestYTDLSettings() 
        { 
            // setting default values
            TestSettings.SetDefault();
        }

        public YTDLSettings TestSettings = new YTDLSettings() { };

        private readonly string[] CorrectSettings = new string[]
        { 
            // hard coding for testing purposes
            @"C:/jwapp",
            @"C:/jwapp/settings.json",
        };

        [TestMethod]
        public void TestSetDefaultDirectory()
        {
            Assert.AreEqual(CorrectSettings[0], TestSettings.app_path);
        }

        [TestMethod]
        public void TestSetDefaultSettings()
        {
            Assert.AreEqual(CorrectSettings[1], TestSettings.app_settings_path);
        }
    }

    [TestClass]
    public class TestInputSanitisation
    {
        /// <summary>
        /// Test class for Input Sanitisation class.
        /// </summary>

        public TestInputSanitisation() { }

        public InputSanitisationAlgorithms TestSanitisation = new InputSanitisationAlgorithms();

        private readonly string[] TestDates = new string[]
        {
            // 31 not valid day (array from 0)
            "30/12/2022",
            "21/05/1900",
            "50/50/10000",
            "01/01/1500"
        };

        [TestMethod]
        public void TestSanitiseYear()
        {
            // testing valid year
            Assert.IsTrue(TestSanitisation.SanitiseYear(TestDates[0][6..]));

            // testing invalid year
            Assert.IsFalse(TestSanitisation.SanitiseYear(TestDates[2][6..]));
        }

        [TestMethod]
        public void TestSanitiseMonth()
        {
            // testing valid month
            Assert.IsTrue(TestSanitisation.SanitiseMonth(TestDates[0][2..5]));

            // testing invalid month
            Assert.IsFalse(TestSanitisation.SanitiseMonth(TestDates[2][3..5]));
        }

        [TestMethod]
        public void TestSanitiseDay()
        {
            // testing valid day
            Assert.IsTrue(TestSanitisation.SanitiseDay(TestDates[0][..2]));

            // testing invalid day
            Assert.IsFalse(TestSanitisation.SanitiseDay(TestDates[2][..2]));
        }

        [TestMethod]
        public void TestGetLeapYear()
        {
            // leap year
            Assert.IsTrue(TestSanitisation.GetLeapYear(TestDates[1][6..]));

            // non leap year
            Assert.IsFalse(TestSanitisation.GetLeapYear(TestDates[0][6..]));
        }

        [TestMethod]
        public void TestGetCalendarType()
        {
            Assert.IsTrue(TestSanitisation.GetCalendarType(TestDates[0][6..], TestDates[0][3..5], TestDates[0][..2]));
        }
    }

    [TestClass]
    public class TestDateCalculatorProgram
    {
        public TestDateCalculatorProgram() { }

        public DateCalculatorProgram TestProgram = new DateCalculatorProgram();

        private readonly string[] RangeExampleOne = new string[] { "1", "2", "3", "4", "5" };

        [TestMethod]
        public void TestGetRange()
        {
            // test same arrays
            CollectionAssert.AreEqual(RangeExampleOne, TestProgram.GetRange(5));

            // test different
            CollectionAssert.AreNotEqual(RangeExampleOne, TestProgram.GetRange(10));
        }

        // set up known dates for testing, friday = 5, tuesday = 2
        // has to be -1 for month, day
        // real values: gregorian 2023/02/24, julian 1752/09/01
        private readonly string[] KnownGregorian = new string[] { "2023", "1", "23", "5" };
        private readonly string[] KnownJulian = new string[] { "1752", "8", "0", "2" };

        [TestMethod]
        public void TestGregorian()
        {
            // test gregorian known date
            Assert.AreEqual(int.Parse(KnownGregorian[3]), TestProgram.GetDayOfWeekGregorian(KnownGregorian[0], KnownGregorian[1], KnownGregorian[2]));
        }

        [TestMethod]
        public void TestJulian()
        {
            // test julian known date
            Assert.AreEqual(int.Parse(KnownJulian[3]), TestProgram.GetDayofWeekJulian(KnownJulian[0], KnownJulian[1], KnownJulian[2]));
        }
    }
}