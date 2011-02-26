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
            Hashtable startHourList = new Hashtable {{"1", "1"}, {"2", "2"}};
            this._browserTester.VerifySelect("StartHour", "Start Hour List", startHourList);
            Hashtable amPmList = new Hashtable {{"AM", "AM"}, {"PM", "PM"}};
            this._browserTester.VerifySelect("AmPm", "AM PM List", amPmList);
            this._browserTester.VerifyText("StartDate", "Start Date", DateTime.Today.ToShortDateString());
            this._browserTester.VerifyText("EndDate", "End Date", DateTime.Today.ToShortDateString());
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
