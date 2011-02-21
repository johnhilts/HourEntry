using System;
using System.Data;
using System.Web.UI.WebControls;

using Bll.HourEntry;
using System.Collections.Generic;

namespace PresentationLayer.HourEntry
{
    public class StatisticsPresenter
    {
        public enum DateRangeEnum { Today, ThisWeek, LastWeek, ThisMonth, LastMonth, }
        private enum Columns { Project, Hours, Comments, StartDate, }

        private IStatisticsView _View;
        private IProject _Project;
        private IStatisticAnalyzer _Statistics;
        private IHour _Hour;

        private bool _IsPostBack;

        public StatisticsPresenter(IStatisticsView view)
            : this(view, new Project(), new StatisticAnalyzer(), new Hour())
        {
        }
        public StatisticsPresenter(IStatisticsView view, IProject project, IStatisticAnalyzer statistics, IHour hour)
        {
            this._View = view;
            this._Project = project;
            this._Statistics = statistics;
            this._Hour = hour;

            this._IsPostBack = this._View.IsPostBack;
        }

        public void PageLoad(object sender, EventArgs e)
        {

            if (this._IsPostBack)
                return;

            this.LoadDefaults();
        }

        private void LoadDefaults()
        {
            this._View.FromDate.Text = DateTime.Today.ToShortDateString();
            this._View.ToDate.Text = DateTime.Today.ToShortDateString();

            this.LoadDateRangePicker();

            this.LoadProjects();
        }
        private void LoadDateRangePicker()
        {
            ListItemCollection dateRangeList = new ListItemCollection();
            dateRangeList.Add(new ListItem("Today", DateRangeEnum.Today.ToString("d")));
            dateRangeList.Add(new ListItem("Last Week", DateRangeEnum.LastWeek.ToString("d")));
            dateRangeList.Add(new ListItem("This Week", DateRangeEnum.ThisWeek.ToString("d")));
            dateRangeList.Add(new ListItem("Last Month", DateRangeEnum.LastMonth.ToString("d")));
            dateRangeList.Add(new ListItem("This Month", DateRangeEnum.ThisMonth.ToString("d")));

            RadioButtonList dateRangePicker = this._View.DateRangePicker;
            dateRangePicker.DataTextField = "Text";
            dateRangePicker.DataValueField = "Value";
            dateRangePicker.DataSource = dateRangeList;
            dateRangePicker.DataBind();
            dateRangePicker.SelectedIndex = 0;
        }
        private DataTable _ProjectTable;
        private void LoadProjects()
        {
            DropDownList projectList = this._View.Projects;
            projectList.DataTextField = "Text";
            projectList.DataValueField = "Value";
            projectList.DataSource = this.GetProjectList();
            projectList.DataBind();
        }
        private ListItemCollection GetProjectList()
        {
            ListItemCollection projectList = new ListItemCollection();
            this._ProjectTable = this._Project.List();
            foreach (DataRow dr in this._ProjectTable.Rows)
            {
                projectList.Add(new ListItem(dr["Description"].ToString(), dr["ProjectID"].ToString()));
            }
            projectList.Insert(0, new ListItem("All Projects", "0"));

            return projectList;
        }

        public void ShowReports_Click(object sender, EventArgs e)
        {
            DateTime fromDate;
            if (!DateTime.TryParse(this._View.FromDate.Text, out fromDate))
                fromDate = DateTime.Today;

            DateTime toDate;
            if (!DateTime.TryParse(this._View.ToDate.Text, out toDate))
                toDate = DateTime.Today;

            int projectId;
            if (!int.TryParse(this._View.Projects.SelectedValue, out projectId))
                projectId = 0;
            
            string comments;
            comments = this._View.Comments.Text;

            this._Statistics.Hours = this._Hour;
            this._Statistics.Projects = this._Project;

            GridView gvStats = this._View.Statistics;
            List<StatisticEntry> hours = this._Statistics.sumhours(fromDate, toDate, projectId, comments);
            gvStats.DataSource = hours;
            gvStats.DataBind();

            this.AddFooter(gvStats, hours);
        }
        private void AddFooter(GridView gvStats, List<StatisticEntry> hours)
        {
            if (hours.Count == 0) return;

            TableCell hoursColumn = gvStats.FooterRow.Cells[(int)Columns.Hours];
            hoursColumn.Font.Bold = true;
            decimal hourTotal = 0;
            hours.ForEach(delegate(StatisticEntry s) { hourTotal += s.Hours; });
            hoursColumn.Text = hourTotal.ToString();
        }

        public void DateRangePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateUtil.DateRangeEnum dateRange = 
                (DateUtil.DateRangeEnum)Convert.ToInt32(this._View.DateRangePicker.SelectedIndex);
            DateUtil dateUtil = new DateUtil();
            dateUtil.GetDateRange(dateRange);
            this._View.FromDate.Text = dateUtil.StartDate.ToShortDateString();
            this._View.ToDate.Text = dateUtil.EndDate.ToShortDateString();
        }
    }
}
