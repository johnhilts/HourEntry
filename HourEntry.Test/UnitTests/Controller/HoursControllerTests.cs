using System;
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
            var result = (new HoursController()).TimeSheet();
            Assert.That(result, Is.Not.Null, "Time Sheet Controller Result NULL");
            TimeSheetModel model = (TimeSheetModel)result.ViewData.Model;
            Assert.That(model, Is.Not.Null, "Time Sheet Model NULL");
        }
    }
}
