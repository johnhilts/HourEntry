using System;
using System.Web.UI.WebControls;
using System.Data;

using Bll.HourEntry;

namespace PresentationLayer.HourEntry
{
    public class TimeSheetPresenter
    {
        private ITimeSheetView _View;
        private IHour _Hour;
        private IProject _Project;

        public TimeSheetPresenter(ITimeSheetView view, IHour hour, IProject project)
        {
            this._View = view;
            this._Hour = hour;
            this._Project = project;
        }
        public TimeSheetPresenter(ITimeSheetView view)
            : this(view, new Hour(view.DataPath), new Project(view.DataPath))
        {
        }

        public void PageLoad(object sender, EventArgs e)
        {
            if (this._View.IsPostBack)
            {
                this._View.PageValidate();
                return;
            };

            this._View.StartDate.SelectedDate = DateTime.Today;
            this._View.EndDate.SelectedDate = DateTime.Today;

            this.LoadTimeInfo();
            this.LoadProjects();

            this._View.PageEditFocus();
        }

        private void LoadTimeInfo()
        {
            this.GetHours(this._View.StartHour);
            this.GetTime(this._View.StartTime);
            this.GetAmPm(this._View.StartAmPm);
            this.GetHours(this._View.EndHour);
            this.GetTime(this._View.EndTime);
            this.GetAmPm(this._View.EndAmPm);
        }
        private void GetHours(DropDownList dropDownList)
        {
            dropDownList.DataSource = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, };
            dropDownList.DataBind();
            dropDownList.SelectedValue = Convert.ToInt32(DateTime.Now.ToString("hh")).ToString();
        }
        private void GetTime(DropDownList dropDownList)
        {
            dropDownList.DataSource = new string[] { "00", "15", "30", "45", };
            dropDownList.DataBind();
            int currentMinute = DateTime.Now.Minute;
            int selectMinute = TimeSheetPresenter.ConvertMinutes(currentMinute);
            dropDownList.SelectedValue = selectMinute.ToString("00");
        }
        // TODO: is there a helper class we can put this in?
        public static int ConvertMinutes(int currentMinute)
        {
            int selectMinute;
            if (currentMinute < 15)
                selectMinute = 0;
            else if (currentMinute < 30)
                selectMinute = 15;
            else if (currentMinute < 45)
                selectMinute = 30;
            else if (currentMinute < 60)
                selectMinute = 45;
            else
            {
                selectMinute = 0;
                System.Diagnostics.Debug.Assert(true, "Shouldn't be here!");
            }
            return selectMinute;
        }
        private void GetAmPm(DropDownList dropDownList)
        {
            //projectList.DataTextField = "Text";
            //projectList.DataValueField = "Value";
            dropDownList.DataSource = new string[] { "AM", "PM", };
            dropDownList.DataBind();
            dropDownList.SelectedValue = DateTime.Now.ToString("tt").ToString();
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
        private int _StartHour;
        private int _StartTime;
        private string _StartAmPm;
        private int _EndHour;
        private int _EndTime;
        private string _EndAmPm;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private string _Comments;

        public void Edit(object sender, EventArgs e)
        {
            this._View.PageValidate();
            if (!this._View.IsValid) return;

            this.GetUserInput();
            decimal hours = this.CalculateHours();
            DateTime start = this.GetTime(this._StartDate, this._StartHour, this._StartTime, this._StartAmPm);
            DateTime end = this.GetTime(this._EndDate, this._EndHour, this._EndTime, this._EndAmPm);
            this._Hour.Add(this._ProjectId, hours, start, end, this._Comments);

            this._View.Redirect("HourEntry.aspx");
        }
        private decimal CalculateHours()
        {
            DateTime start = this.GetTime(this._StartDate, this._StartHour, this._StartTime, this._StartAmPm);
            DateTime end = this.GetTime(this._EndDate, this._EndHour, this._EndTime, this._EndAmPm);
            TimeSpan time = end - start;
            decimal hours = time.Hours;
            if (time.Minutes < 15)
                hours += 0;
            else if (time.Minutes < 30)
                hours += 0.25M;
            else if (time.Minutes < 45)
                hours += 0.5M;
            else if (time.Minutes < 60)
                hours += 0.75M;
            else // == 60 -- would this ever happen? 
                hours += 1;

            return hours;
        }
        private DateTime GetTime(DateTime inputDate, int inputHour, int inputTime, string inputAmPm)
        {
            int hour = inputHour;
            if (inputAmPm.Equals("PM"))
            {
                if (hour < 12)
                {
                    hour += 12;
                }
            }
            else
            {
                if (hour == 12)
                {
                    hour = 0;
                }
            }
            DateTime time = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, hour, inputTime, 0);

            return time;
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
            this.GetUserInput_TimeFields();
            this._Comments = this._View.Comments.Text;
        }
        private void GetUserInput_TimeFields()
        {
            this._StartHour = Convert.ToInt32(this._View.StartHour.SelectedValue);
            this._StartTime = Convert.ToInt32(this._View.StartTime.SelectedValue);
            this._StartAmPm = this._View.StartAmPm.SelectedValue;
            this._EndHour = Convert.ToInt32(this._View.EndHour.SelectedValue);
            this._EndTime = Convert.ToInt32(this._View.EndTime.SelectedValue);
            this._EndAmPm = this._View.EndAmPm.SelectedValue;
            // TODO: make dates nullable
            this._StartDate = this._View.StartDate.SelectedDate;
            this._EndDate = this._View.EndDate.SelectedDate;
        }

        public void TimeValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = false;

            this.GetUserInput_TimeFields();
            decimal hours = this.CalculateHours();
            if (hours < 0)
                return;

            args.IsValid = true;
        }
    }
}
