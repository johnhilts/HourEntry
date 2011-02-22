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
    public class HourPresenterFixture
    {
        //private enum CommandType { Select, Update, };
        //private enum UpdateType { Insert, Update, View, };

        private GridView _gvHoursList;
        TextBox _txtHours;
        TextBox _txtProject;
        DropDownList _ddlProjects;
        Calendar _calStartDate;
        Calendar _calEndDate;
        TextBox _txtComments;

        private int _ProjectId;
        private string _ProjectName = "New Project Name";
        private decimal _Hours = 1.5M;
        private DateTime _StartDate = DateTime.Today;
        private DateTime _EndDate = DateTime.Today;
        private string _Comments = "This is a Test.";

        private bool _AddNewProject;

        [SetUp]
        protected void SetUp()
        {
            //this._DataPath = "/DataFiles";

            this._txtHours = new TextBox();
            this._txtComments = new TextBox();
            this._txtProject = new TextBox();
            this._ddlProjects = new DropDownList();
            this._calStartDate = new Calendar();
            this._calEndDate = new Calendar();

            this._gvHoursList = new GridView();

            this._ProjectId = 2;

            this._AddNewProject = false;
        }

        [Test]
        public void PageLoad()
        {
            this._ProjectId = 1;
            mock.Mockery mockery = new mock.Mockery();
            IHourEntryView mockView = this.GetMockView(mockery);
            IHour mockHour = this.GetMockHour(mockery);
            IProject mockProject = this.GetMockProject(mockery);

            HourEntryPresenter p = new HourEntryPresenter(mockView, mockHour, mockProject);
            p.PageLoad(null, null);

            Assert.That(this._ddlProjects.Items.Count, Is.GreaterThan(0), "No data in Project List");
            Assert.That(this._ddlProjects.Items.Count, Is.EqualTo(1), "Project List: wrong row count");
            Assert.That(this._ddlProjects.SelectedValue, Is.EqualTo(this._ProjectId.ToString()), "Wrong Selected Project ID");
            Assert.That(this._gvHoursList.Rows.Count, Is.GreaterThan(0), "No data in Hours Entry List");
            Assert.That(this._gvHoursList.Rows.Count, Is.EqualTo(1), "Hours Entry List: wrong row count");
            //Assert.That(this._gvHoursList.Rows[0].Cells[2].Text, Is.EqualTo(this._ProjectName), "Wrong Project Name");
            Assert.That(this._calStartDate.SelectedDate, Is.EqualTo(DateTime.Today),
                "Start Date should default to Today's date");
            Assert.That(this._calEndDate.SelectedDate, Is.EqualTo(DateTime.Today),
                "End Date should default to Today's date");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private IHourEntryView GetMockView(mock.Mockery mockery)
        {
            IHourEntryView mockView = (IHourEntryView)mockery.NewMock(typeof(IHourEntryView));
            mock.Expect.Once.On(mockView).GetProperty("IsPostBack").Will(mock.Return.Value(false));
            mock.Expect.Once.On(mockView).GetProperty("StartDate").Will(mock.Return.Value(this._calStartDate));
            mock.Expect.Once.On(mockView).GetProperty("EndDate").Will(mock.Return.Value(this._calEndDate));
            this.LoadMockProjects(mockView);
            mock.Expect.Once.On(mockView).GetProperty("Project").Will(mock.Return.Value(this._txtProject));
            this.LoadMockHourEntries(mockView);
            mock.Expect.Once.On(mockView).Method("PageEditFocus").WithNoArguments();

            return mockView;
        }
        private void LoadMockProjects(IHourEntryView mockView)
        {
            mock.Expect.Once.On(mockView).GetProperty("Projects").Will(mock.Return.Value(this._ddlProjects));
        }
        private void LoadMockHourEntries(IHourEntryView mockView)
        {
            mock.Expect.Once.On(mockView).GetProperty("HoursList").Will(mock.Return.Value(this._gvHoursList));
        }
        private IHour GetMockHour(mock.Mockery mockery)
        {
            IHour mockHours = (IHour)mockery.NewMock(typeof(IHour));
            // mock.Expect.Once.On(mockHours).SetProperty("DataPath").To(this._DataPath);
            this.ListHours(mockHours);

            return mockHours;
        }
        private void ListHours(IHour mockHours)
        {
            mock.Expect.Once.On(mockHours).Method("List").Will(mock.Return.Value(helper.GetMockHoursData()));
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
            mock.Mockery mockery = new mock.Mockery();
            IHourEntryView mockView = this.GetMockView_AddNew(mockery);
            IHour mockHour = this.GetMockHours_AddNew(mockery);
            //IProject mockProject = (this._AddNewProject) ? this.GetMockProject_AddNew(mockery) : null;
            IProject mockProject = this.GetMockProject_AddNew(mockery);

            HourEntryPresenter p = new HourEntryPresenter(mockView, mockHour, mockProject);
            p.Edit(null, null);

            Assert.That(this._gvHoursList.Rows.Count, Is.GreaterThan(0), "No data in Hours Entry List");
            Assert.That(this._gvHoursList.Rows.Count, Is.EqualTo(1), "Hours Entry List: wrong row count");

            Assert.That(this._txtProject.Text, Is.Empty,
                "Need to clear out the Project Text Box so the same name doesn't get added twice");
            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private IHourEntryView GetMockView_AddNew(mock.Mockery mockery)
        {
            this.PopulateViewControls();
            IHourEntryView mockView = (IHourEntryView)mockery.NewMock(typeof(IHourEntryView));
            mock.Expect.Once.On(mockView).GetProperty("Project").Will(mock.Return.Value(this._txtProject));
            if (!this._AddNewProject)
                this.LoadMockProjects(mockView);
            mock.Expect.Once.On(mockView).GetProperty("Hours").Will(mock.Return.Value(this._txtHours));
            mock.Expect.Once.On(mockView).GetProperty("StartDate").Will(mock.Return.Value(this._calStartDate));
            mock.Expect.Once.On(mockView).GetProperty("EndDate").Will(mock.Return.Value(this._calEndDate));
            mock.Expect.Once.On(mockView).GetProperty("Comments").Will(mock.Return.Value(this._txtComments));
            this.LoadMockHourEntries(mockView);
            if (this._AddNewProject)
            {
                this.LoadMockProjects(mockView);
                mock.Expect.Once.On(mockView).GetProperty("Project").Will(mock.Return.Value(this._txtProject));
            }

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
        private IHour GetMockHours_AddNew(mock.Mockery mockery)
        {
            IHour mockHours = (IHour)mockery.NewMock(typeof(IHour));
            mock.Expect.Once.On(mockHours).Method("Add")
                .With(this._ProjectId, this._Hours, this._StartDate, this._EndDate, this._Comments)
                .Will(mock.Return.Value(1));
            this.ListHours(mockHours);

            return mockHours;
        }
        private IProject GetMockProject_AddNew(mock.Mockery mockery)
        {
            IProject mockProjects = (IProject)mockery.NewMock(typeof(IProject));
            if (this._AddNewProject)
            {
                mock.Expect.Once.On(mockProjects).Method("Add")
                    .With(this._ProjectName)
                    .Will(mock.Return.Value(this._ProjectId));
            }
            if (this._AddNewProject)
                this.ListProjects(mockProjects);

            return mockProjects;
        }

    }
}
