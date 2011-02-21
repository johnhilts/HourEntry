using System;
using System.Web.UI.WebControls;
using System.Data;

using Bll.HourEntry;

namespace PresentationLayer.HourEntry
{
    public class HourSummaryPresenter
    {
        private IHourSummaryView _View;
        private IHour _Hour;
        private IProject _Project;

        public HourSummaryPresenter(IHourSummaryView view, IHour hour, IProject project)
        {
            this._View = view;
            this._Hour = hour;
            this._Project = project;
        }
        public HourSummaryPresenter(IHourSummaryView view)
            : this(view, new Hour(view.DataPath), new Project(view.DataPath))
        {
        }

        public void PageLoad(object sender, EventArgs e)
        {
            if (this._View.IsPostBack) return;

            this.LoadHourEntries();
        }
        private DataTable _ProjectTable;
        private void LoadHourEntries()
        {
            DataTable hourEntries = this._Hour.List();
            DataTable hoursPerMonth = this.GetHoursPerMonth(hourEntries);
            GridView hoursPerMonthList = this._View.HoursPerMonthList;
            hoursPerMonthList.DataSource = hoursPerMonth;
            hoursPerMonthList.DataBind();

            DataView hoursPerMonthPerProjectSorted = new DataView();
            DataTable hoursPerMonthPerProject = this.GetHoursPerMonthPerProject(hourEntries);
            hoursPerMonthPerProjectSorted.Table = hoursPerMonthPerProject;
            hoursPerMonthPerProjectSorted.Sort = "Year DESC, Month DESC, ProjectName";
            GridView hoursPerMonthListPerProject = this._View.HoursPerMonthPerProjectList;
            hoursPerMonthListPerProject.DataSource = hoursPerMonthPerProjectSorted;
            hoursPerMonthListPerProject.DataBind();

            DataView hoursPerWeekPerProjectSorted = new DataView();
            DataTable hoursPerWeekPerProject = this.GetHoursPerWeekPerProject(hourEntries);
            hoursPerWeekPerProjectSorted.Table = hoursPerWeekPerProject;
            hoursPerWeekPerProjectSorted.Sort = "WeekStart DESC, ProjectName";
            GridView hoursPerWeekListPerProject = this._View.HoursPerWeekPerProjectList;
            hoursPerWeekListPerProject.DataSource = hoursPerWeekPerProjectSorted;
            hoursPerWeekListPerProject.DataBind();
        }
        // TODO: need to figure out a way to get rid of repetitive code 
        private DataTable GetHoursPerMonth(DataTable hourEntries)
        {
            DataTable hoursPerMonth = new DataTable();
            hoursPerMonth.Columns.Add("Year", typeof(System.Int32));
            hoursPerMonth.Columns.Add("Month", typeof(System.String));
            hoursPerMonth.Columns.Add("Hours", typeof(System.Decimal));
            int prevYear = 0;
            int prevMonth = 0;
            decimal hourSum = 0;
            bool initFlag = true;
            foreach (DataRow dr in hourEntries.Select("", "StartDate"))
            {
                DateTime startDate = Convert.ToDateTime(dr["StartDate"]);
                int currentYear = startDate.Year;
                int currentMonth = startDate.Month;

                if (initFlag)
                {
                    initFlag = false;

                    prevYear = currentYear;
                    prevMonth = currentMonth;
                }
                else
                {
                    if (prevYear != currentYear || prevMonth != currentMonth)
                    {
                        this.GroupProcessing_HoursPerMonth(hoursPerMonth, prevYear, prevMonth, hourSum);

                        prevYear = currentYear;
                        prevMonth = currentMonth;
                        hourSum = 0;
                    }
                }
                Decimal hours = Convert.ToDecimal(dr["Hours"]);
                hourSum += hours;
            }

            this.GroupProcessing_HoursPerMonth(hoursPerMonth, prevYear, prevMonth, hourSum);

            return hoursPerMonth;
        }
        private void GroupProcessing_HoursPerMonth(DataTable hoursPerMonth, int prevYear, int prevMonth, decimal hourSum)
        {
            DataRow newRow = hoursPerMonth.NewRow();
            newRow["Year"] = prevYear;
            newRow["Month"] = new DateTime(1, prevMonth, 1).ToString("MMMM");
            newRow["Hours"] = hourSum;
            hoursPerMonth.Rows.Add(newRow);
        }
        private DataTable GetHoursPerMonthPerProject(DataTable hourEntries)
        {
            DataTable hoursPerMonthPerProject = new DataTable("HoursPerMonthPerProject");
            hoursPerMonthPerProject.Columns.Add("Year", typeof(System.Int32));
            hoursPerMonthPerProject.Columns.Add("Month", typeof(System.String));
            hoursPerMonthPerProject.Columns.Add("ProjectName", typeof(System.String));
            hoursPerMonthPerProject.Columns.Add("Hours", typeof(System.Decimal));
            int prevYear = 0;
            int prevMonth = 0;
            decimal hourSum = 0;
            int prevProjectId = 0;
            bool initFlag = true;
            foreach (DataRow dr in hourEntries.Select("", "ProjectID, StartDate"))
            {
                DateTime startDate = Convert.ToDateTime(dr["StartDate"]);
                int currentYear = startDate.Year;
                int currentMonth = startDate.Month;
                int currentProjectId = Convert.ToInt32(dr["ProjectId"]);

                if (initFlag)
                {
                    initFlag = false;

                    prevYear = currentYear;
                    prevMonth = currentMonth;
                    prevProjectId = currentProjectId;
                }
                else
                {
                    if (prevYear != currentYear || prevMonth != currentMonth || prevProjectId != currentProjectId)
                    {
                        this.GroupProcessing_HoursPerMonthPerProject(hoursPerMonthPerProject,
                            prevYear, prevMonth, prevProjectId, hourSum);

                        prevYear = currentYear;
                        prevMonth = currentMonth;
                        prevProjectId = currentProjectId;
                        hourSum = 0;
                    }
                }
                Decimal hours = Convert.ToDecimal(dr["Hours"]);
                hourSum += hours;
            }

            this.GroupProcessing_HoursPerMonthPerProject(hoursPerMonthPerProject,
                prevYear, prevMonth, prevProjectId, hourSum);

            return hoursPerMonthPerProject;
        }
        private void GroupProcessing_HoursPerMonthPerProject(DataTable hoursPerMonthPerProject,
            int prevYear, int prevMonth, int prevProjectId, decimal hourSum)
        {
            DataRow newRow = hoursPerMonthPerProject.NewRow();
            newRow["Year"] = prevYear;
            newRow["Month"] = new DateTime(1, prevMonth, 1).ToString("MMMM");
            newRow["Hours"] = hourSum;
            newRow["ProjectName"] = this.GetProjectName(prevProjectId.ToString());
            hoursPerMonthPerProject.Rows.Add(newRow);
        }
        private DataTable GetHoursPerWeekPerProject(DataTable hourEntries)
        {
            DataTable hoursPerWeekPerProject = new DataTable("HoursPerWeekPerProject");
            hoursPerWeekPerProject.Columns.Add("WeekStart", typeof(System.DateTime));
            hoursPerWeekPerProject.Columns.Add("WeekEnd", typeof(System.DateTime));
            hoursPerWeekPerProject.Columns.Add("ProjectName", typeof(System.String));
            hoursPerWeekPerProject.Columns.Add("Hours", typeof(System.Decimal));
            DateTime weekStart = DateTime.MinValue;
            DateTime prevDate = DateTime.MinValue;
            decimal hourSum = 0;
            int prevProjectId = 0;
            bool initFlag = true;
            // TODO: if I just switch the sort items, will this keep me from having to use the dataview sort???
            foreach (DataRow dr in hourEntries.Select("", "ProjectID, StartDate"))
            {
                DateTime startDate = Convert.ToDateTime(dr["StartDate"]);
                DateTime currentDate = startDate;
                int currentProjectId = Convert.ToInt32(dr["ProjectId"]);

                if (initFlag)
                {
                    initFlag = false;

                    prevDate = currentDate;
                    prevProjectId = currentProjectId;
                    weekStart = this.GetWeekStart(currentDate);
                }
                else
                {
                    bool weekChanged;
                    weekChanged = (weekStart != currentDate && (weekStart.AddDays(6) < currentDate.Date));
                        //|| currentDate.DayOfWeek == DayOfWeek.Sunday));
                    if (weekChanged || prevProjectId != currentProjectId)
                    {
                        this.GroupProcessing_HoursPerWeekPerProject(hoursPerWeekPerProject,
                            prevDate, prevProjectId, hourSum, weekStart);

                        prevDate = currentDate;
                        prevProjectId = currentProjectId;
                        weekStart = this.GetWeekStart(currentDate);
                        hourSum = 0;
                    }
                }
                Decimal hours = Convert.ToDecimal(dr["Hours"]);
                hourSum += hours;
            }

            this.GroupProcessing_HoursPerWeekPerProject(hoursPerWeekPerProject,
                prevDate, prevProjectId, hourSum, weekStart);

            return hoursPerWeekPerProject;
        }
        private void GroupProcessing_HoursPerWeekPerProject(DataTable hoursPerWeekPerProject,
            DateTime prevDate, int prevProjectId, decimal hourSum, DateTime weekStart)
        {
            DataRow newRow = hoursPerWeekPerProject.NewRow();
            newRow["Hours"] = hourSum;
            newRow["ProjectName"] = this.GetProjectName(prevProjectId.ToString());

            DateTime weekEnd = this.GetWeekEnd(weekStart);
            newRow["WeekStart"] = weekStart;
            newRow["WeekEnd"] = weekEnd;
            hoursPerWeekPerProject.Rows.Add(newRow);
        }
        private DateTime GetWeekStart(DateTime currentDate)
        {
            return currentDate.AddDays((int)currentDate.DayOfWeek * -1);
        }
        private DateTime GetWeekEnd(DateTime weekStart)
        {
            int addDays = 6 - (int)weekStart.DayOfWeek;
            return weekStart.AddDays(addDays);
        }
        private string GetProjectName(string projectId)
        {
            if (this._ProjectTable == null)
                this._ProjectTable = this._Project.List();

            DataRow[] dr = this._ProjectTable.Select("ProjectID=" + projectId);
            if (dr.Length == 1)
                return dr[0]["Description"].ToString();
            else
                return projectId.ToString();

        }
    }
}
