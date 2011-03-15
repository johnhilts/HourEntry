using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HourEntry.Services;
using HourEntry.Services.Data;
using NUnit.Framework;

namespace HourEntry.Test.UnitTests.Services
{
    [TestFixture]
    public class PresenterServiceTests
    {
        [Test]
        public void Default_TimeSheet_Hour_Should_Match_Current_Time()
        {
            // arrange
            PresenterService presenterService = new PresenterService();
            DateTime currentTime = new DateTime(2011, 2, 26, 21, 30, 0);

            // action
            DefaultTimeSheet defaultTimeSheet = presenterService.GetDefaultTimeSheet(currentTime);

            // assert
            Assert.That(defaultTimeSheet.Hour, Is.EqualTo(9), "Wrong Default Time Sheet Hour");
        }
    }
}
