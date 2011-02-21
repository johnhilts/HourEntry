using System;
using System.Data;

using NUnit.Framework;
using Moq;

using Bll.HourEntry;
using dal = Bll.HourEntry.Dal;
using UnitTests.HourEntry.Helpers;
using System.Collections.Generic;

namespace UnitTests.Statistics
{
    [TestFixture]
    public class StatisticsFixture
    {
        [Test]
        public void SumHours()
        {
            SumHours(0);
        }
        [Test]
        public void SumHours_ByProject()
        {
            this.SumHours(2);
        }
        [Test]
        public void SumHours_DateRange()
        {
            SumHours(0, DateTime.Today, DateTime.Today.AddDays(7));
        }
        [Test]
        public void SumHours_DateRangeShouldHaveMoreHoursThanOneDate()
        {
            decimal rangeHours = SumHours(0, DateTime.Today, DateTime.Today.AddDays(7));
            decimal singleDateHours = SumHours(0, DateTime.Today, DateTime.Today.AddDays(1));
            Assert.That(rangeHours, Is.GreaterThan(singleDateHours), "Date Range Should Have More Hours than One Date");
        }
        private decimal SumHours(int projectId)
        {
            return this.SumHours(projectId, DateTime.Today, DateTime.Today.AddDays(1));
        }
        private decimal SumHours(int projectId, DateTime startDate, DateTime endDate)
        {
            // arrange
            Mock<IHour> mockHours = this.GetMockHours();
            StatisticAnalyzer stats = new StatisticAnalyzer();
            stats.Hours = mockHours.Object;
            stats.Projects = this.GetMockProjects().Object;

            // action
            List<StatisticEntry> hoursTable = stats.sumhours(startDate, endDate, projectId, "");
            decimal hours = 0;
            foreach (StatisticEntry dr in hoursTable)
            {
                hours += dr.Hours;
            }

            // assert
            Assert.That(hours, Is.GreaterThan(0), "No Hours");
            mockHours.VerifyAll();

            return hours;
        }
        private Mock<IHour> GetMockHours()
        {
            Mock<IHour> mockHour = new Mock<IHour>();
            DataTable mockHourTable = helper.GetMockHoursData(true);
            mockHour.Setup<DataTable>(x => x.List()).Returns(mockHourTable);

            return mockHour;
        }
        private Mock<IProject> GetMockProjects()
        {
            Mock<IProject> mockProject = new Mock<IProject>();
            DataTable mockProjectTable = helper.GetMockProjectData(true);
            mockProject.Setup<DataTable>(x => x.List()).Returns(mockProjectTable);

            return mockProject;
        }
    }
}
