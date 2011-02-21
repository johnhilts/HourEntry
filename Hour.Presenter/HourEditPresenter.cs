using System;
using System.Web.UI.WebControls;
using System.Data;

using Bll.HourEntry;

namespace PresentationLayer.HourEntry
{
    public class HourEditPresenter
    {
        private IHourEditView _View;
        private IHour _Hour;
        private IProject _Project;

        public HourEditPresenter(IHourEditView view, IHour hour, IProject project)
        {
            this._View = view;
            this._Hour = hour;
            this._Project = project;
        }
        public HourEditPresenter(IHourEditView view)
            : this(view, new Hour(view.DataPath), new Project(view.DataPath))
        {
        }

        public void PageLoad(object sender, EventArgs e)
        {
            if (this._View.IsPostBack) return;

            this._RowId = this._View.RowId;

            this.LoadProjects();

            this.LoadHourEntries();

            this._View.PageEditFocus();
        }
        private DataTable _ProjectTable;
        private void LoadProjects()
        {
            this.LoadProjects("");
        }
        private void LoadProjects(string projectId)
        {
            DropDownList projectList = this._View.Projects;
            projectList.DataTextField = "Text";
            projectList.DataValueField = "Value";
            projectList.DataSource = this.GetProjectList();
            projectList.DataBind();

            this._View.Project.Text = "";

            if (!string.IsNullOrEmpty(projectId))
                projectList.SelectedValue = projectId;
        }
        private void LoadHourEntries()
        {
            DataTable hourEntryTable = this._Hour.GetRecord(this._RowId);
            DataRow hourEntry = hourEntryTable.Rows[0];
            this._View.Hours.Text = hourEntry["Hours"].ToString();
            this._View.Projects.SelectedValue = hourEntry["ProjectId"].ToString();
            this._View.Comments.Text = hourEntry["Comments"].ToString();
            this.LoadCalendar(hourEntry, this._View.StartDate, "StartDate");
            this.LoadCalendar(hourEntry, this._View.EndDate, "EndDate");
        }
        private void LoadCalendar(DataRow hourEntry, Calendar calendar, string fieldName)
        {
            // TODO: change to Try/Parse
            DateTime date = (DateTime)hourEntry[fieldName];
            calendar.SelectedDate = date;
            calendar.VisibleDate = date;
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

        private int _RowId;
        private int _ProjectId;
        private decimal _Hours;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private string _Comments;

        public void Edit(object sender, EventArgs e)
        {
            this._RowId = this._View.RowId;
            this.GetUserInput();
            this._Hour.Update(this._RowId,
                this._ProjectId, this._Hours, this._StartDate, this._EndDate, this._Comments);

            this._View.Redirect("HourEntry.aspx");
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
                this.LoadProjects(this._ProjectId.ToString());
            }
            if (!decimal.TryParse(this._View.Hours.Text, out this._Hours))
                this._Hours = 0;
            // TODO: make dates nullable
            this._StartDate = this._View.StartDate.SelectedDate;
            this._EndDate = this._View.EndDate.SelectedDate;
            this._Comments = this._View.Comments.Text;
        }
    }
}
