using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HourEntry.Web.Controllers;
using HourEntry.Web.Models;
using NUnit.Framework;

namespace HourEntry.Test.UnitTests.Controller
{
    [TestFixture]
    public class HoursControllerTests
    {
        [Test]
        public void Loading_Page_Should_Auto_Populate_All_Fields()
        {
            // arrange
            List<string> amPmList = new List<string> { "AM", "PM" };

            // action
            var result = (new HoursController()).TimeSheet();
            Assert.That(result, Is.Not.Null, "Time Sheet Controller Result NULL");
            TimeSheetModel model = (TimeSheetModel)result.ViewData.Model;

            // assert
            Assert.That(model, Is.Not.Null, "Time Sheet Model NULL");
            Assert.That(model.AmPm, Is.EqualTo("AM"), "Wrong AM PM List Selection");
            Assert.That(model.StartDate, Is.EqualTo(DateTime.Today), "Wrong Start Date");
            Assert.That(model.EndDate, Is.EqualTo(DateTime.Today), "Wrong End Date");
        }
    }
}
