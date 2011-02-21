using System;
using System.Data;
using System.Web.UI.WebControls;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using mock = NMock2;

using Bll.HourEntry;
using PresentationLayer.HourEntry;
using UnitTests.HourEntry.Helpers;

namespace UnitTests.Presentation.HourEntry.Hours
{
    [TestFixture]
    public class HourEditPresenterFixture
    {
        private int _RowId = 1;
        TextBox _txtHours;
        TextBox _txtProject;
        DropDownList _ddlProjects;
        Calendar _calStartDate;
        Calendar _calEndDate;
        TextBox _txtComments;

        private int _ProjectId;
        private string _ProjectName = "New Project Name";
        private decimal _Hours = 1.5M;
        private DateTime _StartDate = DateTime.Today.AddDays(-2);
        private DateTime _EndDate = DateTime.Today.AddDays(-1);
        private string _Comments = "This is a Test.";

        private bool _AddNewProject;

        [SetUp]
        protected void SetUp()
        {
            this._txtHours = new TextBox();
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
            mock.Mockery mockery = new mock.Mockery();
            IHourEditView mockView = this.GetMockView(mockery);
            IHour mockHour = this.GetMockHour(mockery);
            IProject mockProject = this.GetMockProject(mockery);

            HourEditPresenter p = new HourEditPresenter(mockView, mockHour, mockProject);
            p.PageLoad(null, null);

            // TODO: need to get rid of these magic values and combine with the ones in the helper class
            Assert.That(this._txtHours.Text, Is.EqualTo("1.5"), "Wrong Hours");
            Assert.That(this._ddlProjects.Items.Count, Is.GreaterThan(0), "No data in Project List");
            Assert.That(this._ddlProjects.Items.Count, Is.EqualTo(1), "Project List: wrong row count");
            Assert.That(this._ddlProjects.SelectedValue, Is.EqualTo(this._ProjectId.ToString()), "Wrong Selected Project ID");
            Assert.That(this._calStartDate.SelectedDate, Is.EqualTo(this._StartDate),
                "Start Date should default to date in data table");
            Assert.That(this._calStartDate.VisibleDate, Is.EqualTo(this._StartDate), "Wrong Visible Date (Start Date)");
            Assert.That(this._calEndDate.SelectedDate, Is.EqualTo(this._EndDate),
                "End Date should default to date in data table");
            Assert.That(this._calEndDate.VisibleDate, Is.EqualTo(this._EndDate), "Wrong Visible Date (End Date)");
            Assert.That(this._txtComments.Text, Is.EqualTo("This is a Test."), "Wrong Comments");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private IHourEditView GetMockView(mock.Mockery mockery)
        {
            IHourEditView mockView = (IHourEditView)mockery.NewMock(typeof(IHourEditView));
            mock.Expect.Once.On(mockView).GetProperty("IsPostBack").Will(mock.Return.Value(false));
            mock.Expect.Once.On(mockView).GetProperty("RowId").Will(mock.Return.Value(this._RowId));
            mock.Expect.Once.On(mockView).GetProperty("Hours").Will(mock.Return.Value(this._txtHours));
            mock.Expect.Once.On(mockView).GetProperty("Projects").Will(mock.Return.Value(this._ddlProjects));
            mock.Expect.Once.On(mockView).GetProperty("StartDate").Will(mock.Return.Value(this._calStartDate));
            mock.Expect.Once.On(mockView).GetProperty("EndDate").Will(mock.Return.Value(this._calEndDate));
            mock.Expect.Once.On(mockView).GetProperty("Comments").Will(mock.Return.Value(this._txtComments));
            this.LoadMockProjects(mockView);
            mock.Expect.Once.On(mockView).GetProperty("Project").Will(mock.Return.Value(this._txtProject));
            mock.Expect.Once.On(mockView).Method("PageEditFocus").WithNoArguments();

            return mockView;
        }
        private void LoadMockProjects(IHourEditView mockView)
        {
            mock.Expect.Once.On(mockView).GetProperty("Projects").Will(mock.Return.Value(this._ddlProjects));
        }
        private IHour GetMockHour(mock.Mockery mockery)
        {
            IHour mockHours = (IHour)mockery.NewMock(typeof(IHour));
            // mock.Expect.Once.On(mockHours).SetProperty("DataPath").To(this._DataPath);
            this.GetHourRecord(mockHours);

            return mockHours;
        }
        private void GetHourRecord(IHour mockHours)
        {
            mock.Expect.Once.On(mockHours).Method("GetRecord")
                .With(this._RowId)
                .Will(mock.Return.Value(helper.GetMockHoursData(this._StartDate, this._EndDate)));
        }
        private IProject GetMockProject(mock.Mockery mockery)
        {
            IProject mockProjects = (IProject)mockery.NewMock(typeof(IProject));
            this.ListProjects(mockProjects);

            return mockProjects;
        }
        private void ListProjects(IProject mockProjects)
        {
            mock.Expect.Once.On(mockProjects).Method("List").Will(mock.Return.Value(helper.GetMockProjectData()));
        }

        [Test]
        public void ClickEditButton()
        {
            this._ProjectId = 1;

            this.Edit();
        }
        [Test]
        public void ClickEditButtonWithNewProjectName()
        {
            this._ProjectId = 1;

            this._AddNewProject = true;

            this.Edit();
        }
        private void Edit()
        {
            mock.Mockery mockery = new mock.Mockery();
            IHourEditView mockView = this.GetMockView_Edit(mockery);
            IHour mockHour = this.GetMockHours_Edit(mockery);
            IProject mockProject = this.GetMockProject_Edit(mockery);

            HourEditPresenter p = new HourEditPresenter(mockView, mockHour, mockProject);
            p.Edit(null, null);

            Assert.That(this._txtProject.Text, Is.Empty,
                "Need to clear out the Project Text Box so the same name doesn't get added twice");
            if (this._AddNewProject)
                Assert.That(this._ddlProjects.SelectedValue, Is.EqualTo(this._ProjectId.ToString()), 
                    "Drop-down should be set to new project");
            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private IHourEditView GetMockView_Edit(mock.Mockery mockery)
        {
            this.PopulateViewControls();
            IHourEditView mockView = (IHourEditView)mockery.NewMock(typeof(IHourEditView));
            mock.Expect.Once.On(mockView).GetProperty("RowId").Will(mock.Return.Value(this._RowId));
            mock.Expect.Once.On(mockView).GetProperty("Project").Will(mock.Return.Value(this._txtProject));
            if (!this._AddNewProject)
                this.LoadMockProjects(mockView);
            mock.Expect.Once.On(mockView).GetProperty("Hours").Will(mock.Return.Value(this._txtHours));
            mock.Expect.Once.On(mockView).GetProperty("StartDate").Will(mock.Return.Value(this._calStartDate));
            mock.Expect.Once.On(mockView).GetProperty("EndDate").Will(mock.Return.Value(this._calEndDate));
            mock.Expect.Once.On(mockView).GetProperty("Comments").Will(mock.Return.Value(this._txtComments));
            if (this._AddNewProject)
            {
                this.LoadMockProjects(mockView);
                mock.Expect.Once.On(mockView).GetProperty("Project").Will(mock.Return.Value(this._txtProject));
            }

            mock.Expect.Once.On(mockView).Method("Redirect").With("HourEntry.aspx");

            return mockView;
        }
        private void PopulateViewControls()
        {
            this._txtHours.Text = this._Hours.ToString();
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
        private IHour GetMockHours_Edit(mock.Mockery mockery)
        {
            IHour mockHours = (IHour)mockery.NewMock(typeof(IHour));
            mock.Expect.Once.On(mockHours).Method("Update")
                .With(this._RowId, this._ProjectId, this._Hours, this._StartDate, this._EndDate, this._Comments);
            // this.GetHourRecord(mockHours);

            return mockHours;
        }
        private IProject GetMockProject_Edit(mock.Mockery mockery)
        {
            IProject mockProjects = (IProject)mockery.NewMock(typeof(IProject));
            if (this._AddNewProject)
            {
                mock.Expect.Once.On(mockProjects).Method("Add")
                    .With(this._ProjectName)
                    .Will(mock.Return.Value(this._ProjectId));
                this.ListProjects(mockProjects);
            }

            return mockProjects;
        }
    }
}
