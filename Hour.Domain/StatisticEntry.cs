using System;

namespace Bll.HourEntry
{
    public sealed class StatisticEntry
    {
        private string _Project;
        private decimal _Hours;
        private string _Comments;
        private DateTime _StartDate;

        public string Project { get { return this._Project; } }
        public decimal Hours { get { return this._Hours; } }
        public string Comments { get { return this._Comments; } }
        public DateTime StartDate { get { return this._StartDate; } }

        public StatisticEntry(string project, decimal hours, string comments, DateTime startDate)
        {
            this._Project = project;
            this._Hours = hours;
            this._Comments = comments;
            this._StartDate = startDate;
        }
    }
}
