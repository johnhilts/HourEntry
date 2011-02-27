using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HourEntry.Services;
using HourEntry.Services.Data;
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
            PresenterService presenterService = new PresenterService();
            List<string> amPmList = new List<string> { "AM", "PM" };
            List<short> hourList = presenterService.GetDefaultTimeSheet().HourList;
            List<short> minuteList = presenterService.GetDefaultTimeSheet().MinuteList;

            // action
            var result = (new HoursController()).TimeSheet();
            Assert.That(result, Is.Not.Null, "Time Sheet Controller Result NULL");
            TimeSheetModel model = (TimeSheetModel)result.ViewData.Model;
            dynamic viewBag = result.ViewBag;

            // assert
            Assert.That(model, Is.Not.Null, "Time Sheet Model NULL");
            Assert.That(viewBag.StartHourList.Count, Is.EqualTo(hourList.Count), "Wrong Start Hour List Count");
            Assert.That(model.StartHour, Is.EqualTo(1), "Wrong Start Hour");
            Assert.That(viewBag.StartMinuteList.Count, Is.EqualTo(minuteList.Count), "Wrong Start Minute List Count");
            Assert.That(model.StartMinute, Is.EqualTo(1), "Wrong Start Minute");
            Assert.That(viewBag.StartAmPmList.Count, Is.EqualTo(amPmList.Count), "Wrong Start AM PM List Count");
            Assert.That(model.StartAmPm, Is.EqualTo("AM"), "Wrong AM PM List Selection");
            Assert.That(viewBag.EndHourList.Count, Is.EqualTo(hourList.Count), "Wrong End Hour List Count");
            Assert.That(model.EndHour, Is.EqualTo(1), "Wrong End Hour");
            Assert.That(viewBag.EndMinuteList.Count, Is.EqualTo(minuteList.Count), "Wrong End Minute List Count");
            Assert.That(model.EndMinute, Is.EqualTo(1), "Wrong End Minute");
            Assert.That(viewBag.EndAmPmList.Count, Is.EqualTo(amPmList.Count), "Wrong End AM PM List Count");
            Assert.That(model.EndAmPm, Is.EqualTo("AM"), "Wrong AM PM List Selection");
            Assert.That(model.StartDate, Is.EqualTo(DateTime.Today), "Wrong Start Date");
            Assert.That(model.EndDate, Is.EqualTo(DateTime.Today), "Wrong End Date");
        }

    }
}
