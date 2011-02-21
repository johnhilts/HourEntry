using System;
using System.Data;
using System.Web.UI.WebControls;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using _ = NMock2;

using Bll.HourEntry;
using PresentationLayer.HourEntry;
using UnitTests.HourEntry.Helpers;

namespace UnitTests.Presentation.HourEntry.Hours
{
    [TestFixture]
    public class TimeSheetPresenterFixture
    {
        private DropDownList _ddlStartHour;
        private DropDownList _ddlStartTime;
        private DropDownList _ddlStartAmPm;
        private DropDownList _ddlEndHour;
        private DropDownList _ddlEndTime;
        private DropDownList _ddlEndAmPm;
        private TextBox _txtProject;
        private DropDownList _ddlProjects;
        private Calendar _calStartDate;
        private Calendar _calEndDate;
        private TextBox _txtComments;

        private int _ProjectId;
        private string _ProjectName = "New Project Name";
        private int _StartHour = 8;
        private int _StartTime = 30;
        private int _EndHour = 10;
        private int _EndTime = 0;
        private DateTime _StartDate = DateTime.Today;
        private DateTime _EndDate = DateTime.Today;
        private string _Comments = "This is a Test.";

        private bool _AddNewProject;

        [SetUp]
        protected void SetUp()
        {
            this._ddlStartHour = new DropDownList();
            this._ddlStartTime = new DropDownList();
            this._ddlStartAmPm = new DropDownList();
            this._ddlEndHour = new DropDownList();
            this._ddlEndTime = new DropDownList();
            this._ddlEndAmPm = new DropDownList();
            this._txtComments = new TextBox();
            this._txtProject = new TextBox();
            this._ddlProjects = new DropDownList();
            this._calStartDate = new Calendar();
            this._calEndDate = new Calendar();

            this._ProjectId = 2;

            this._AddNewProject = false;
        }

        [Test]
        public void PageLoad()
        {
            this._ProjectId = 1;
            _.Mockery mockery = new _.Mockery();
            ITimeSheetView mockView = this.GetMockView(mockery);
            // IHour mockHour = this.GetMockHour(mockery);
            IHour mockHour = null;
            IProject mockProject = this.GetMockProject(mockery);

            TimeSheetPresenter p = new TimeSheetPresenter(mockView, mockHour, mockProject);
            p.PageLoad(null, null);

            this.PageLoadAssertions();

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private void PageLoadAssertions()
        {
            Assert.That(this._ddlProjects.Items.Count, Is.GreaterThan(0), "No data in Project List");
            Assert.That(this._ddlProjects.Items.Count, Is.EqualTo(1), "Project List: wrong row count");
            Assert.That(this._ddlProjects.SelectedValue, Is.EqualTo(this._ProjectId.ToString()),
                "Wrong Selected Project ID");
            Assert.That(this._calStartDate.SelectedDate, Is.EqualTo(DateTime.Today),
                "Start Date should default to Today's date");
            Assert.That(Convert.ToInt32(this._ddlStartHour.SelectedValue),
                Is.EqualTo(Convert.ToInt32(DateTime.Now.ToString("hh"))),
                "Start Hour should default to current hour");
            Assert.That(Convert.ToInt32(this._ddlStartTime.SelectedValue),
                Is.EqualTo(TimeSheetPresenter.ConvertMinutes(Convert.ToInt32(DateTime.Now.ToString("mm")))),
                "Start Minute should default to current minute converted to 0, 15, 30, or 45");
            Assert.That(this._ddlStartAmPm.SelectedValue, Is.EqualTo(DateTime.Now.ToString("tt")),
                "Start AmPm should default to current AmPm");
            Assert.That(this._calEndDate.SelectedDate, Is.EqualTo(DateTime.Today),
                "End Date should default to Today's date");
            Assert.That(Convert.ToInt32(this._ddlEndHour.SelectedValue),
                Is.EqualTo(Convert.ToInt32(DateTime.Now.ToString("hh"))),
                "End Hour should default to current hour");
            Assert.That(Convert.ToInt32(this._ddlEndTime.SelectedValue),
                Is.EqualTo(TimeSheetPresenter.ConvertMinutes(Convert.ToInt32(DateTime.Now.ToString("mm")))),
                "End Minute should default to current minute converted to 0, 15, 30, or 45");
            Assert.That(this._ddlEndAmPm.SelectedValue, Is.EqualTo(DateTime.Now.ToString("tt")),
                "End AmPm should default to current AmPm");
        }
        private ITimeSheetView GetMockView(_.Mockery mockery)
        {
            ITimeSheetView mockView = (ITimeSheetView)mockery.NewMock(typeof(ITimeSheetView));
            _.Expect.Once.On(mockView).GetProperty("IsPostBack").Will(_.Return.Value(false));

            _.Expect.Once.On(mockView).GetProperty("StartHour").Will(_.Return.Value(this._ddlStartHour));
            _.Expect.Once.On(mockView).GetProperty("StartTime").Will(_.Return.Value(this._ddlStartTime));
            _.Expect.Once.On(mockView).GetProperty("StartAmPm").Will(_.Return.Value(this._ddlStartAmPm));
            _.Expect.Once.On(mockView).GetProperty("EndHour").Will(_.Return.Value(this._ddlEndHour));
            _.Expect.Once.On(mockView).GetProperty("EndTime").Will(_.Return.Value(this._ddlEndTime));
            _.Expect.Once.On(mockView).GetProperty("EndAmPm").Will(_.Return.Value(this._ddlEndAmPm));
            _.Expect.Once.On(mockView).GetProperty("StartDate").Will(_.Return.Value(this._calStartDate));
            _.Expect.Once.On(mockView).GetProperty("EndDate").Will(_.Return.Value(this._calEndDate));
            this.LoadMockProjects(mockView);
            _.Expect.Once.On(mockView).GetProperty("Project").Will(_.Return.Value(this._txtProject));
            _.Expect.Once.On(mockView).Method("PageEditFocus").WithNoArguments();

            return mockView;
        }
        private void LoadMockProjects(ITimeSheetView mockView)
        {
            _.Expect.Once.On(mockView).GetProperty("Projects").Will(_.Return.Value(this._ddlProjects));
        }
        private IProject GetMockProject(_.Mockery mockery)
        {
            IProject mockProjects = (IProject)mockery.NewMock(typeof(IProject));
            this.ListProjects(mockProjects);

            return mockProjects;
        }
        private void ListProjects(IProject mockProjects)
        {
            _.Expect.Once.On(mockProjects).Method("List").Will(_.Return.Value(helper.GetMockProjectData()));
        }

        [Test]
        public void ClickAddNewButton()
        {
            this._ProjectId = 1;

            this.AddNew();
        }
        [Test]
        public void ClickAddNewButtonWithNewProjectName()
        {
            this._ProjectId = 1;

            this._AddNewProject = true;

            this.AddNew();
        }
        private void AddNew()
        {
            _.Mockery mockery = new _.Mockery();
            ITimeSheetView mockView = this.GetMockView_AddNew(mockery);
            IHour mockHour = this.GetMockHours_AddNew(mockery);
            IProject mockProject = this.GetMockProject_AddNew(mockery);

            TimeSheetPresenter p = new TimeSheetPresenter(mockView, mockHour, mockProject);
            p.Edit(null, null);

            Assert.That(this._txtProject.Text, Is.Empty,
                "Need to clear out the Project Text Box so the same name doesn't get added twice");
            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private ITimeSheetView GetMockView_AddNew(_.Mockery mockery)
        {
            this.PopulateViewControls();
            ITimeSheetView mockView = (ITimeSheetView)mockery.NewMock(typeof(ITimeSheetView));
            _.Expect.Once.On(mockView).Method("PageValidate").WithNoArguments();
            _.Expect.Once.On(mockView).GetProperty("IsValid").Will(_.Return.Value(true));
            _.Expect.Once.On(mockView).GetProperty("Project").Will(_.Return.Value(this._txtProject));
            if (!this._AddNewProject)
                this.LoadMockProjects(mockView);
            _.Expect.Once.On(mockView).GetProperty("StartHour").Will(_.Return.Value(this._ddlStartHour));
            _.Expect.Once.On(mockView).GetProperty("StartTime").Will(_.Return.Value(this._ddlStartTime));
            _.Expect.Once.On(mockView).GetProperty("StartAmPm").Will(_.Return.Value(this._ddlStartAmPm));
            _.Expect.Once.On(mockView).GetProperty("EndHour").Will(_.Return.Value(this._ddlEndHour));
            _.Expect.Once.On(mockView).GetProperty("EndTime").Will(_.Return.Value(this._ddlEndTime));
            _.Expect.Once.On(mockView).GetProperty("EndAmPm").Will(_.Return.Value(this._ddlEndAmPm));
            _.Expect.Once.On(mockView).GetProperty("StartDate").Will(_.Return.Value(this._calStartDate));
            _.Expect.Once.On(mockView).GetProperty("EndDate").Will(_.Return.Value(this._calEndDate));
            _.Expect.Once.On(mockView).GetProperty("Comments").Will(_.Return.Value(this._txtComments));
            if (this._AddNewProject)
            {
                this.LoadMockProjects(mockView);
                _.Expect.Once.On(mockView).GetProperty("Project").Will(_.Return.Value(this._txtProject));
            }

            _.Expect.Once.On(mockView).Method("Redirect").With("HourEntry.aspx");
            
            return mockView;
        }
        private void PopulateViewControls()
        {
            string[] hours = new string[] { "1", "1", "2", "2", "3", "3", "4", "4", "5", "5", "6", "6", "7", "7", "8", "8", "9", "9", "10", "10", "11", "11", "12", "12", };
            string[] minutes = new string[] { "0", "0", "15", "15", "30", "30", "45", "45", };
            this._ddlStartHour = this.GetDropDownList(hours);
            this._ddlStartHour.SelectedValue = this._StartHour.ToString();
            this._ddlStartTime = this.GetDropDownList(minutes);
            this._ddlStartTime.SelectedValue = this._StartTime.ToString();
            this._ddlEndHour = this.GetDropDownList(hours);
            this._ddlEndHour.SelectedValue = this._EndHour.ToString();
            this._ddlEndTime = this.GetDropDownList(minutes);
            this._ddlEndTime.SelectedValue = this._EndTime.ToString();
            this._txtComments.Text = this._Comments;
            if (this._AddNewProject)
                this._txtProject.Text = this._ProjectName;
            string[] items = { "Project 1", "1", "Project 2", "2", "Project 3", "3" };
            this._ddlProjects = this.GetDropDownList(items);
            this._ddlProjects.SelectedValue = this._ProjectId.ToString();
            Assert.That(this._ddlProjects.SelectedValue, Is.EqualTo(this._ProjectId.ToString()),
                "Wrong Selected Project in dropdown");
            this._calStartDate.SelectedDate = this._StartDate;
            this._calEndDate.SelectedDate = this._EndDate;
        }
        private DropDownList GetDropDownList(string[] items)
        {
            DropDownList dropdown = new DropDownList();

            int i = 0;
            string text = "";
            foreach (string item in items)
            {
                if (i % 2 == 0)
                    text = item;
                else
                {
                    string value = item;
                    dropdown.Items.Add(new ListItem(text, value));
                }
                i += 1;
            }

            return dropdown;
        }
        private IHour GetMockHours_AddNew(_.Mockery mockery)
        {
            IHour mockHours = (IHour)mockery.NewMock(typeof(IHour));
            decimal hours = 1.5M; //TODO: need to calc this
            _.Expect.Once.On(mockHours).Method("Add")
                .With(this._ProjectId, hours, 
                    this._StartDate.AddHours(this._StartHour).AddMinutes(this._StartTime),
                    this._EndDate.AddHours(this._EndHour).AddMinutes(this._EndTime), this._Comments)
                .Will(_.Return.Value(1));

            return mockHours;
        }
        private IProject GetMockProject_AddNew(_.Mockery mockery)
        {
            IProject mockProjects = (IProject)mockery.NewMock(typeof(IProject));
            if (this._AddNewProject)
            {
                _.Expect.Once.On(mockProjects).Method("Add")
                    .With(this._ProjectName)
                    .Will(_.Return.Value(this._ProjectId));
                this.ListProjects(mockProjects);
            }

            return mockProjects;
        }
    }
}
