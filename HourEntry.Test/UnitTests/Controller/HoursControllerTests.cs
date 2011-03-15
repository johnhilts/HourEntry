using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
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
            List<short> hourList = presenterService.GetDefaultTimeSheet(DateTime.Now).HourList;
            List<string> minuteList = presenterService.GetDefaultTimeSheet(DateTime.Now).MinuteList;
            string currentHour = Convert.ToInt32(DateTime.Now.ToString("hh")).ToString();

            // action
            var result = (new HoursController()).TimeSheet();
            Assert.That(result, Is.Not.Null, "Time Sheet Controller Result NULL");
            TimeSheetModel model = (TimeSheetModel)result.ViewData.Model;
            dynamic viewBag = result.ViewBag;
            List<SelectListItem> startHourList = viewBag.StartHourList;
            List<SelectListItem> endHourList = viewBag.endHourList;

            // assert
            Assert.That(model, Is.Not.Null, "Time Sheet Model NULL");
            Assert.That(startHourList.Count, Is.EqualTo(hourList.Count), "Wrong Start Hour List Count");
            Assert.That(model.StartHour, Is.EqualTo(Convert.ToInt32(DateTime.Now.ToString("hh"))), "Wrong Start Hour");
            Assert.That(viewBag.StartMinuteList.Count, Is.EqualTo(minuteList.Count), "Wrong Start Minute List Count");
            Assert.That(model.StartMinute, Is.EqualTo(1), "Wrong Start Minute");
            Assert.That(viewBag.StartAmPmList.Count, Is.EqualTo(amPmList.Count), "Wrong Start AM PM List Count");
            Assert.That(model.StartAmPm, Is.EqualTo("AM"), "Wrong AM PM List Selection");
            Assert.That(endHourList.Count, Is.EqualTo(hourList.Count), "Wrong End Hour List Count");
            Assert.That(model.EndHour, Is.EqualTo(Convert.ToInt32(DateTime.Now.ToString("hh"))), "Wrong End Hour");
            Assert.That(viewBag.EndMinuteList.Count, Is.EqualTo(minuteList.Count), "Wrong End Minute List Count");
            Assert.That(model.EndMinute, Is.EqualTo(1), "Wrong End Minute");
            Assert.That(viewBag.EndAmPmList.Count, Is.EqualTo(amPmList.Count), "Wrong End AM PM List Count");
            Assert.That(model.EndAmPm, Is.EqualTo("AM"), "Wrong AM PM List Selection");
            Assert.That(model.StartDate, Is.EqualTo(DateTime.Today), "Wrong Start Date");
            Assert.That(model.EndDate, Is.EqualTo(DateTime.Today), "Wrong End Date");
        }

    }
}
