using System;
using System.Web.UI.WebControls;
using System.Data;

using Bll.HourEntry;

namespace PresentationLayer.HourEntry
{
    public class HourEntryPresenter
    {
        private IHourEntryView _View;
        private IHour _Hour;
        private IProject _Project;

        public HourEntryPresenter(IHourEntryView view, IHour hour, IProject project)
        {
            this._View = view;
            this._Hour = hour;
            this._Project = project;
        }
        public HourEntryPresenter(IHourEntryView view)
            : this(view, new Hour(view.DataPath), new Project(view.DataPath))
        {
        }

        public void PageLoad(object sender, EventArgs e)
        {
            if (this._View.IsPostBack) return;

            this._View.StartDate.SelectedDate = DateTime.Today;
            this._View.EndDate.SelectedDate = DateTime.Today;

            this.LoadProjects();

            this.LoadHourEntries();

            this._View.PageEditFocus();
        }
        private DataTable _ProjectTable;
        private void LoadProjects()
        {
            DropDownList projectList = this._View.Projects;
            projectList.DataTextField = "Text";
            projectList.DataValueField = "Value";
            projectList.DataSource = this.GetProjectList();
            projectList.DataBind();

            this._View.Project.Text = "";
        }
        private void LoadHourEntries()
        {
            GridView hoursList = this._View.HoursList;
            DataView hourEntriesSorted = new DataView();
            DataTable hourEntries = this._Hour.List();
            hourEntriesSorted.Table = hourEntries;
            hourEntriesSorted.Sort = "StartDate DESC";
            hoursList.DataSource = hourEntriesSorted;
            hoursList.DataBind();
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
        private ListItemCollection GetProjectList()
        {
            ListItemCollection projectList = new ListItemCollection();
            this._ProjectTable = this._Project.List();
            foreach (DataRow dr in this._ProjectTable.Rows)
            {
                projectList.Add(new ListItem(dr["Description"].ToString(), dr["ProjectID"].ToString()));
            }

            return projectList;
        }

        private int _ProjectId;
        private decimal _Hours;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private string _Comments;

        public void Edit(object sender, EventArgs e)
        {
            this.GetUserInput();
            this._Hour.Add(this._ProjectId, this._Hours, this._StartDate, this._EndDate, this._Comments);
            this.LoadHourEntries();
            //this._View.Update();
            //// this._View.Redirect("HourEntry.aspx");
        }
        // TODO: make this error proof
        private void GetUserInput()
        {
            TextBox txtProject = this._View.Project;
            if (string.IsNullOrEmpty(txtProject.Text))
            {
                this._ProjectId = Convert.ToInt32(this._View.Projects.SelectedValue);
            }
            else
            {
                // TODO: need to do a get record by name, and then get project ID
                //System.Diagnostics.Debug.Assert(true, "Get Project ID by Name");
                this._ProjectId = this._Project.Add(txtProject.Text);
                this.LoadProjects();
            }
            if (!decimal.TryParse(this._View.Hours.Text, out this._Hours))
                this._Hours = 0;
            // TODO: make dates nullable
            this._StartDate = this._View.StartDate.SelectedDate;
            this._EndDate = this._View.EndDate.SelectedDate;
            this._Comments = this._View.Comments.Text;
        }

        private enum HourEntryDetailsColumns { RowID, Hours, Project, StartDate, EndDate, Comments, EditLink };

        public void HoursRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            string projectId = e.Row.Cells[(int)HourEntryDetailsColumns.Project].Text;
            e.Row.Cells[(int)HourEntryDetailsColumns.Project].Text = this.GetProjectName(projectId);

            string rowId = e.Row.Cells[(int)HourEntryDetailsColumns.RowID].Text;
            HyperLink editLink = (HyperLink)e.Row.Cells[(int)HourEntryDetailsColumns.EditLink].Controls[1];
            editLink.NavigateUrl = string.Format("HourEntryDetailEdit.aspx?{0}={1}", Constants.RowId, rowId);
        }

        public void HoursSorting(Object sender, GridViewSortEventArgs e)
        {
        }

        //public void RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    switch (e.CommandName)
        //    {
        //        case "Edit":
        //            this._View.QuestionDetailsFocus();
        //            break;
        //    }

        //}
        //private delegate void Focus();
        //public void AddNew(object sender, EventArgs e)
        //{
        //    string itemName = ((Button)sender).CommandName;
        //    DetailsView dv;
        //    Focus focus;
        //    switch (itemName)
        //    {
        //        case "NewQuestion":
        //            dv = this._View.QuestionDetails;
        //            focus = new Focus(this._View.QuestionDetailsFocus);
        //            break;
        //        case "NewNote":
        //            dv = this._View.NoteDetails;
        //            focus = new Focus(this._View.NoteDetailsFocus);
        //            break;
        //        case "NewTest":
        //            dv = this._View.TestDetails;
        //            focus = new Focus(this._View.TestDetailsFocus);
        //            break;
        //        case "NewHour":
        //            dv = this._View.HourDetails;
        //            focus = new Focus(this._View.HourDetailsFocus);
        //            break;
        //        default:
        //            System.Diagnostics.Debug.Fail("Shouldn't be here");
        //            return;
        //    }

        //    dv.Visible = true;
        //    dv.ChangeMode(DetailsViewMode.Insert);
        //    focus.Invoke();
        //}

        //public void ItemCommand(object sender, DetailsViewCommandEventArgs e)
        //{
        //    switch (e.CommandName)
        //    {
        //        case "New":
        //        case "Edit":
        //            this._View.QuestionDetailsFocus();
        //            break;
        //    }
        //}

        //public void QuestionDetailView_ItemCreated(object sender, EventArgs e)
        //{
        //    //DetailsView dvQuestion = this._View.QuestionDetails;
        //    //DetailsViewRow row = dvQuestion.Rows[0];

        //    //row.Cells[2].Text = this._View.StoryId.ToString();
        //}

        //public void QuestionDetailView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        //{
        //    e.Values["StoryId"] = this._View.StoryId;
        //}

        //public void ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        //{
        //    this._View.QuestionDetails.Visible = false;
        //}

    }
}
