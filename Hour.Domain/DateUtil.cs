using System;

namespace Bll.HourEntry
{
    public class DateUtil
    {
        public enum DateRangeEnum { Today, LastWeek, ThisWeek, LastMonth, ThisMonth, }

        private DateTime _StartDate;
        private DateTime _EndDate;

        public DateTime StartDate { get { return this._StartDate; } }
        public DateTime EndDate { get { return this._EndDate; } }

        public void GetDateRange(DateRangeEnum dateRange)
        {
            switch (dateRange)
            {
                case DateRangeEnum.LastWeek:
                    this._StartDate = this.GetStartOfWeek(DateTime.Today.AddDays(-7));
                    this._EndDate = this._StartDate.AddDays(6);
                    break;
                case DateRangeEnum.ThisWeek:
                    this._StartDate = this.GetStartOfWeek(DateTime.Today);
                    this._EndDate = this._StartDate.AddDays(6);
                    break;
                case DateRangeEnum.LastMonth:
                    this._StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    this._EndDate = this._StartDate.AddDays(-1);
                    this._StartDate = this._StartDate.AddMonths(-1);
                    break;
                case DateRangeEnum.ThisMonth:
                    this._StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    this._EndDate = this._StartDate.AddMonths(1).AddDays(-1);
                    break;
                case DateRangeEnum.Today:
                default:
                    this._StartDate = DateTime.Today;
                    this._EndDate = DateTime.Today;
                    break;
            }
        }
        private DateTime GetStartOfWeek(DateTime date)
        {
            return date.AddDays(Convert.ToInt32(date.DayOfWeek) * -1);
        }
    }
}
