using System;
using System.Data;
using System.Web.UI.WebControls;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using mock = NMock2;

using Bll.HourEntry;
using PresentationLayer.HourEntry;
using UnitTests.HourEntry.Helpers;

namespace UnitTests.Presentation.HourEntry.Projects
{
    [TestFixture]
    public class ProjectFixture
    {
        private enum CommandType { Select, Update, };
        private enum UpdateType { Insert, Update, View, };

        private GridView gvList;
        private DetailsView dv;
        private string _DataPath;
        private int _StoryId = 1;
        private GridView gvStoryList;

        [SetUp]
        protected void SetUp()
        {
            this._DataPath = "/DataFiles";

            this.gvList = new GridView();
            this.dv = new DetailsView();

            this.gvStoryList = new GridView();
        }

        [Test]
        public void PageLoad()
        {
            mock.Mockery mockery = new mock.Mockery();
            IProjectView mockView = this.GetMockView(mockery);

            ProjectPresenter p = new ProjectPresenter(mockView);
            p.PageLoad(null, null);
            Assert.That(this.dv.Visible, Is.False, "Details should be invisible");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private IProjectView GetMockView(mock.Mockery mockery)
        {
            IProjectView mockView = (IProjectView)mockery.NewMock(typeof(IProjectView));
            mock.Expect.Once.On(mockView).GetProperty("IsPostBack").Will(mock.Return.Value(false));
            mock.Expect.Exactly(2).On(mockView).GetProperty("DataPath").Will(mock.Return.Value(this._DataPath));
            mock.Expect.Once.On(mockView).GetProperty("Details").Will(mock.Return.Value(this.dv));

            return mockView;
        }
        private IModel GetMockProject(mock.Mockery mockery)
        {
            IModel mockProject = (IModel)mockery.NewMock(typeof(IModel));
            mock.Expect.Once.On(mockProject).SetProperty("DataPath").To(this._DataPath);
            mock.Expect.Once.On(mockProject).Method("List").Will(mock.Return.Value(helper.GetMockProjectData()));

            return mockProject;
        }

        [Test()]
        public void RowCommand_Edit()
        {
            mock.Mockery mockery = new mock.Mockery();
            IProjectView mockView = (IProjectView)mockery.NewMock(typeof(IProjectView));

            GridViewCommandEventArgs e = this.GetGridViewCommandEventArgs(CommandType.Update);
            mock.Expect.Exactly(2).On(mockView).GetProperty("DataPath").Will(mock.Return.Value(this._DataPath));
            mock.Expect.Once.On(mockView).Method("ListFocus");

            ProjectPresenter presenter = new ProjectPresenter(mockView);
            presenter.RowCommand(null, e);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private GridViewCommandEventArgs GetGridViewCommandEventArgs(CommandType commandType)
        {
            CommandEventArgs originalArgs;
            GridViewCommandEventArgs e = null;
            switch (commandType)
            {
                case CommandType.Update:
                    originalArgs = new CommandEventArgs("Edit", null);
                    e = new GridViewCommandEventArgs(null, originalArgs);
                    break;
                case CommandType.Select:
                    originalArgs = new CommandEventArgs("Select", null);
                    e = new GridViewCommandEventArgs(null, originalArgs);
                    break;
            }
            return e;
        }
        private IModel GetMockStories(mock.Mockery mockery)
        {
            IModel mockStories = (IModel)mockery.NewMock(typeof(IModel));
            mock.Expect.Once.On(mockStories).SetProperty("DataPath").To(this._DataPath);
            mock.Expect.Once.On(mockStories).Method("List")
                .With(this._StoryId)
                .Will(mock.Return.Value(this.GetMockStoryData()));

            return mockStories;
        }
        private DataTable GetMockStoryData()
        {
            DataTable stories = new DataTable("Stories");
            stories.Columns.Add("SelectButtonPlaceHolder", typeof(System.String));
            stories.Columns.Add("StoryId", typeof(System.Int32));
            stories.Columns.Add("Name", typeof(System.String));
            DataRow dr = stories.NewRow();
            dr["SelectButtonPlaceHolder"] = DBNull.Value;
            dr["StoryId"] = this._StoryId;
            dr["Name"] = "This is a Test.";
            stories.Rows.Add(dr);
            return stories;
        }

        [Test()]
        public void StoryListSelectedIndexChanged()
        {
            mock.Mockery mockery = new mock.Mockery();
            IProjectView mockView = this.GetMockView_GetStoryList(mockery);

            ProjectPresenter presenter = new ProjectPresenter(mockView);
            presenter.StoryListSelectedIndexChanged(null, null);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private IProjectView GetMockView_GetStoryList(mock.Mockery mockery)
        {
            IProjectView mockView = (IProjectView)mockery.NewMock(typeof(IProjectView));
            mock.Expect.Exactly(2).On(mockView).GetProperty("DataPath").Will(mock.Return.Value(this._DataPath));
            this.gvStoryList.DataSource = this.GetMockStoryData();
            this.gvStoryList.DataBind();
            this.gvStoryList.SelectedIndex = 0;
            mock.Expect.Once.On(mockView).GetProperty("StoryList").Will(mock.Return.Value(this.gvStoryList));
            mock.Expect.Once.On(mockView).Method("Redirect").With("Stories.aspx?StoryId=" + this._StoryId.ToString());

            return mockView;
        }

        [Test]
        public void AddNew()
        {
            mock.Mockery mockery = new mock.Mockery();
            IProjectView mockView = this.GetMockView_AddNew(mockery);

            ProjectPresenter p = new ProjectPresenter(mockView);
            p.AddNew(null, null);

            Assert.That(this.dv.CurrentMode, Is.EqualTo(DetailsViewMode.Insert), 
                "Details View should be in insert mode");
            Assert.That(this.dv.Visible, Is.EqualTo(true),"Details View should be visible");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private IProjectView GetMockView_AddNew(mock.Mockery mockery)
        {
            IProjectView mockView = (IProjectView)mockery.NewMock(typeof(IProjectView));
            mock.Expect.Exactly(2).On(mockView).GetProperty("DataPath").Will(mock.Return.Value(this._DataPath));
            mock.Expect.Once.On(mockView).GetProperty("Details").Will(mock.Return.Value(this.dv));
            this.dv.Visible = false;
            this.dv.ChangeMode(DetailsViewMode.Insert);
            mock.Expect.Once.On(mockView).Method("DetailsFocus");

            return mockView;
        }

        [Test()]
        public void ItemCommand_Edit()
        {
            mock.Mockery mockery = new mock.Mockery();
            IProjectView mockView = (IProjectView)mockery.NewMock(typeof(IProjectView));

            DetailsViewCommandEventArgs e = this.GetDetailsViewCommandEventArgs(UpdateType.Update);
            // DetailsView dv = this.GetProjectDetails(UpdateType.Update);
            mock.Expect.Exactly(2).On(mockView).GetProperty("DataPath").Will(mock.Return.Value(this._DataPath));
            mock.Expect.Once.On(mockView).Method("DetailsFocus");

            ProjectPresenter presenter = new ProjectPresenter(mockView);
            presenter.ItemCommand(null, e);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private DetailsView GetProjectDetails(UpdateType updateType)
        {
            DetailsView dvProjectDetails = new DetailsView();

            DataTable projectRecord = new DataTable();
            projectRecord.Columns.Add("ProjectId", typeof(Int32));
            projectRecord.Columns.Add("Description", typeof(String));
            DataRow dr = projectRecord.NewRow();
            dr["ProjectId"] = 3;
            dr["Description"] = "Test Project";
            projectRecord.Rows.Add(dr);

            dvProjectDetails.DataSource = projectRecord;
            dvProjectDetails.DataBind();

            switch (updateType)
            {
                case UpdateType.Insert:
                    dvProjectDetails.ChangeMode(DetailsViewMode.Insert);
                    break;
                case UpdateType.Update:
                    dvProjectDetails.ChangeMode(DetailsViewMode.Edit);
                    break;
                //case EditGatewaysPresenterFixture.UpdateType.View:
                //    dvProjectDetails.Rows(p.DetailColumns.PaymentTypeID).Cells[1].Controls.Add(Me.GetPaymentTypeIDLabel);
                //    break;
            }

            return dvProjectDetails;

        }
        private DetailsViewCommandEventArgs GetDetailsViewCommandEventArgs(UpdateType updateType)
        {
            CommandEventArgs originalArgs = new CommandEventArgs("New", null);
            DetailsViewCommandEventArgs e = new DetailsViewCommandEventArgs(null, originalArgs);

            return e;
        }

        [Test()]
        public void ItemInserted()
        {
            mock.Mockery mockery = new mock.Mockery();
            IProjectView mockView = (IProjectView)mockery.NewMock(typeof(IProjectView));
            mock.Expect.Exactly(2).On(mockView).GetProperty("DataPath").Will(mock.Return.Value(this._DataPath));
            mock.Expect.Once.On(mockView).GetProperty("Details").Will(mock.Return.Value(this.dv));

            ProjectPresenter presenter = new ProjectPresenter(mockView);
            presenter.ItemInserted(null, null);

            Assert.That(this.dv.Visible, Is.False, "Details view should be invisible");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        //[Test()]
        //public void ListStories()
        //{
        //    mock.Mockery mockery = new mock.Mockery();
        //    IProjectView mockView = (IProjectView)mockery.NewMock(typeof(IProjectView));
        //    //mock.Expect.Exactly(2).On(mockView).GetProperty("DataPath").Will(mock.Return.Value(this._DataPath));

        //    IStory mockStory = (IStory)mockery.NewMock(typeof(IStory));
        //    //mock.Expect.Once.On(mockStory).SetProperty("DataPath").To(this._DataPath);
        //    mock.Expect.Once.On(mockStory).Method("List").With(this._StoryId).Will(mock.Return.Value(this.GetMockStoryData()));

        //    ProjectPresenter presenter = new ProjectPresenter(mockView, null, mockStory);
        //    presenter.ListStories(this._StoryId);

        //    mockery.VerifyAllExpectationsHaveBeenMet();
        //}
    }
}
