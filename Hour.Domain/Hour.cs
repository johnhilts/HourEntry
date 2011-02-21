using System;
using System.Data;

namespace Bll.HourEntry
{
    public class Hour : BllBase, IHour
    {
        private int _RowId;
        public int RowId
        {
            get { return _RowId; }
        }
        private int _ProjectId;
        public int ProjectId
        {
            get { return _ProjectId; }
        }
        private decimal _Hours;
        public decimal Hours
        {
            get { return _Hours; }
        }
        private DateTime _StartDate;
        public DateTime StartDate
        {
            get
            {
                return this._StartDate;
            }
        }
        private DateTime _EndDate;
        public DateTime EndDate
        {
            get
            {
                return this._EndDate;
            }
        }
        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
        }

        public Hour()
            : base()
        {
        }
        public Hour(string dataPath)
            : base(dataPath)
        {
        }

        public DataTable List()
        {
            DataTable hours = this._DataHelper.GetData("HourEntries");
            return hours;
        }
        public DataTable GetRecord(int rowId)
        {
            DataTable hours = ((IHour)this).List();
            DataRow[] selectedHours = hours.Select("RowID = " + rowId.ToString());
            if (selectedHours.Length == 0)
            {
                hours.Clear();
                return hours;
            }

            DataRow hourEntry = selectedHours[0];
            this._RowId = (int)hourEntry["RowID"];
            this._ProjectId = (int)hourEntry["ProjectID"];
            this._Hours = (decimal)hourEntry["Hours"];
            this._StartDate = (DateTime)hourEntry["StartDate"];
            this._EndDate = (DateTime)hourEntry["EndDate"];
            this._Comments = hourEntry["Comments"].ToString();

            // TODO: this is dumb - fix it - or at least put it into a Util function
            DataTable temp = hours.Clone();
            DataRow drTemp = temp.NewRow();
            foreach (DataColumn c in hours.Columns)
                drTemp[c.ColumnName] = hourEntry[c.ColumnName];
            temp.Rows.Add(drTemp);
            return temp;
        }
        int IHour.Add(int projectId, decimal hours, DateTime startDate, DateTime endDate, string comments)
        {
            DataTable hourTable = ((IHour)this).List();
            DataRow newHourEntry = hourTable.NewRow();
            hourTable.Rows.Add(newHourEntry);
            int newRowId = (int)hourTable.Rows[hourTable.Rows.Count - 1]["RowID"];
            this.SaveRecord(hourTable, newHourEntry, projectId, hours, startDate, endDate, comments);
            return newRowId;
        }
        void IHour.Update(int rowId, int projectId, decimal hours, DateTime startDate, DateTime endDate, string comments)
        {
            DataTable hourTable = ((IHour)this).List();
            DataRow hourEntry = hourTable.Select("RowID = " + rowId.ToString())[0];
            this.SaveRecord(hourTable, hourEntry, projectId, hours, startDate, endDate, comments);
        }
        private void SaveRecord(DataTable hourTable, DataRow hourEntry,
            int projectId, decimal hours, DateTime startDate, DateTime endDate, string comments)
        {
            hourEntry["ProjectID"] = projectId;
            hourEntry["Hours"] = hours;
            hourEntry["StartDate"] = startDate;
            hourEntry["EndDate"] = endDate;
            hourEntry["Comments"] = comments;
            this._DataHelper.SetData("HourEntries", hourTable);
        }
    }
}
