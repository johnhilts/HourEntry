using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UxTestLibrary;

namespace HourEntry.Test.UI
{
    [TestFixture]
    public class TimeSheetUxTests
    {
        private BrowserTester _browserTester;
        private Tester _tester;

        [SetUp]
        protected void SetUp()
        {
            this._browserTester = new BrowserTester();
            this._tester = new Tester();
            this._tester.Domain = "http://localhost:63456";
        }

        [Test]
        public void Loading_Page_Should_Auto_Populate_All_Fields()
        {
            GoToTimesheet();
            /*
             * Login 
             * Verify controls populated with default values
             */
            VerifyControlsPopulatedWithDefaultValues();
        }

        private void GoToTimesheet()
        {
            this._browserTester.NavigateTo(new Uri(this._tester.Domain + "/Hours/TimeSheet/"));
        }

        private void VerifyControlsPopulatedWithDefaultValues()
        {
            Hashtable hourList = this.GetHourList();
            Hashtable minuteList = this.GetMinuteList();
            Hashtable amPmList = new Hashtable { { "AM", "AM" }, { "PM", "PM" } };
            this._browserTester.VerifySelect("StartHour", "Start Hour List", hourList);
            this._browserTester.VerifySelect("StartMinute", "Start Minute List", minuteList);
            this._browserTester.VerifySelect("StartAmPm", "AM PM List", amPmList);
            this._browserTester.VerifySelect("EndHour", "End Hour List", hourList);
            this._browserTester.VerifySelect("EndMinute", "End Minute List", minuteList);
            this._browserTester.VerifySelect("EndAmPm", "AM PM List", amPmList);
            this._browserTester.VerifyText("StartDate", "Start Date", DateTime.Today.ToShortDateString());
            this._browserTester.VerifyText("EndDate", "End Date", DateTime.Today.ToShortDateString());
        }

        private Hashtable GetHourList()
        {
            Hashtable hourList = new Hashtable();
            for (int hour = 1; hour <= 12; hour++)
            {
                hourList.Add(hour.ToString(), hour.ToString());
            }

            return hourList;
        }

        private Hashtable GetMinuteList()
        {
            Hashtable minuteList = new Hashtable();
            for (int minute = 0; minute < 46; minute += 15)
            {
                minuteList.Add(minute.ToString(), minute.ToString());
            }

            Assert.That(minuteList.ContainsKey("0"));
            Assert.That(minuteList.ContainsValue("0"));
            Assert.That(minuteList.ContainsKey("15"));
            Assert.That(minuteList.ContainsValue("15"));
            Assert.That(minuteList.ContainsKey("30"));
            Assert.That(minuteList.ContainsValue("30"));
            Assert.That(minuteList.ContainsKey("45"));
            Assert.That(minuteList.ContainsValue("45"));

            Assert.That(minuteList.Count, Is.EqualTo(4), "Should be only 4 items in minute list");

            return minuteList;
        }

        [TearDown]
        protected void TearDown()
        {
            this._browserTester.Close();
        }

        private class Tester
        {
            public string Domain { get; set; }
        }

    }
}
