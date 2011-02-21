using System;
using System.Data;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using mock = NMock2;

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
            mock.Mockery mockery = new mock.Mockery();

            StatisticAnalyzer stats = new StatisticAnalyzer();
            stats.Hours = this.GetMockHours(mockery);
            stats.Projects = this.GetMockProjects(mockery);

            List<StatisticEntry> hoursTable = stats.sumhours(startDate, endDate, projectId, "");
            decimal hours = 0;
            foreach (StatisticEntry dr in hoursTable)
            {
                hours += dr.Hours;
            }
            Assert.That(hours, Is.GreaterThan(0), "No Hours");

            mockery.VerifyAllExpectationsHaveBeenMet();

            return hours;
        }
        private IHour GetMockHours(mock.Mockery mockery)
        {
            IHour mockHour = (IHour)mockery.NewMock(typeof(IHour));
            DataTable mockHourTable = helper.GetMockHoursData(true);
            mock.Expect.Once.On(mockHour).Method("List").Will(mock.Return.Value(mockHourTable));

            return mockHour;
        }
        private IProject GetMockProjects(mock.Mockery mockery)
        {
            IProject mockProject = (IProject)mockery.NewMock(typeof(IProject));
            DataTable mockProjectTable = helper.GetMockProjectData(true);
            mock.Expect.Once.On(mockProject).Method("List").Will(mock.Return.Value(mockProjectTable));

            return mockProject;
        }
    }
}
