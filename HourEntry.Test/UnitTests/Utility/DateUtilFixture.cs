using System;
using System.Data;

using NUnit.Framework;

using Bll.HourEntry;

namespace UnitTests.HourEntry
{
    [TestFixture]
    public class DateUtilFixture
    {
        [Test]
        public void GetTodaysDates()
        {
            this.GetDateRange(DateUtil.DateRangeEnum.Today, DateTime.Today, DateTime.Today);
        }
        [Test]
        public void GetThisWeeksDateRange()
        {
            DateTime startOfWeek = this.GetStartOfWeek(DateTime.Today);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            this.GetDateRange(DateUtil.DateRangeEnum.ThisWeek, startOfWeek, endOfWeek);
        }
        [Test]
        public void GetLastWeeksDateRange()
        {
            DateTime startOfWeek = this.GetStartOfWeek(DateTime.Today.AddDays(-7));
            DateTime endOfWeek = startOfWeek.AddDays(6);
            this.GetDateRange(DateUtil.DateRangeEnum.LastWeek, startOfWeek, endOfWeek);
        }
        [Test]
        public void GetThisMonthsDateRange()
        {
            DateTime startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            this.GetDateRange(DateUtil.DateRangeEnum.ThisMonth, startOfMonth, endOfMonth);
        }
        [Test]
        public void GetLastMonthsDateRange()
        {
            DateTime startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime endOfMonth = startOfMonth.AddDays(-1);
            startOfMonth = startOfMonth.AddMonths(-1);
            this.GetDateRange(DateUtil.DateRangeEnum.LastMonth, startOfMonth, endOfMonth);
        }
        private void GetDateRange(DateUtil.DateRangeEnum dateRange, DateTime expectedStartDate, DateTime expectedEndDate)
        {
            DateUtil dateUtil = new DateUtil();
            dateUtil.GetDateRange(dateRange);
            Assert.That(dateUtil.StartDate, Is.EqualTo(expectedStartDate), "Wrong Start Date");
            Assert.That(dateUtil.EndDate, Is.EqualTo(expectedEndDate), "Wrong End Date");
        }
        private DateTime GetStartOfWeek(DateTime date)
        {
            return date.AddDays(Convert.ToInt32(date.DayOfWeek) * -1);
        }
    }
}
