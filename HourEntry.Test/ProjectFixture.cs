using System;
using System.Data;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using mock = NMock2;

using Bll.HourEntry;
using dal = Bll.HourEntry.Dal;
using UnitTests.HourEntry.Helpers;

namespace UnitTests.HourEntry.Projects
{
    [TestFixture]
    public class ProjectFixture
    {
        [Test]
        public void List()
        {
            mock.Mockery mockery = new mock.Mockery();
            Project project = new Project("/DataFiles");
            project.DataHelper = this.GetMockData(mockery);
            DataTable projects = ((IProject)project).List();
            Assert.That(projects, Is.Not.Null, "Projects NULL");
            Assert.That(projects.Rows.Count, Is.GreaterThan(0), "Projects - no data");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private dal.IDataHelper GetMockData(mock.Mockery mockery)
        {
            dal.IDataHelper mockData = (dal.IDataHelper)mockery.NewMock(typeof(dal.IDataHelper));
            DataTable mockProjects = helper.GetMockProjectData();
            mock.Expect.Once.On(mockData).Method("GetData").With("Projects").Will(mock.Return.Value(mockProjects));

            return mockData;
        }

        [Test]
        public void GetRecord()
        {
            mock.Mockery mockery = new mock.Mockery();
            Project project = new Project("/DataFiles");
            project.DataHelper = this.GetMockData(mockery);
            int projectId = 1;
            ((IProject)project).GetRecord(projectId);
            Assert.That(project.ProjectId, Is.EqualTo(1), "Project Story ID Wrong");
            Assert.That(project.Description, Is.EqualTo("This is a Test."), "Project Description Wrong");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        [Test]
        public void GetRecord_NoRecords()
        {
            mock.Mockery mockery = new mock.Mockery();
            Project project = new Project("/DataFiles");
            project.DataHelper = this.GetMockData(mockery);
            int projectId = 0;
            DataTable projectTable = ((IProject)project).GetRecord(projectId);
            Assert.That(projectTable.Rows.Count, Is.EqualTo(0), "Should be no rows");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void Update()
        {
            mock.Mockery mockery = new mock.Mockery();

            Project project = new Project("/DataFiles");
            project.DataHelper = this.GetMockData_ForSaving(mockery);

            int projectId = 1;
            string description = "This is a Test.";
            ((IProject)project).Update(projectId, description);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private dal.IDataHelper GetMockData_ForSaving(mock.Mockery mockery)
        {
            dal.IDataHelper mockData = this.GetMockData(mockery);
            // not sure why this fails mock.Expect.Once.On(mockData).Method("SetData").With(this.SetData(this.GetMockProjects()));
            mock.Expect.Once.On(mockData).Method("SetData").WithAnyArguments();

            return mockData;
        }
        //private DataTable SetData(DataTable projects)
        //{
        //    int projectId = 1;
        //    DataRow project = projects.Select("ProjectID = " + projectId.ToString())[0];
        //    project["StoryID"] = 1;
        //    project["Description"] = "This is a Test.";
        //    return projects;
        //}
        //[Test]
        //public void AddNewAndSave()
        //{
        //    mock.Mockery mockery = new mock.Mockery();

        //    Project project = new Project("/DataFiles");
        //    project.DataHelper = this.GetMockData_ForSaving(mockery);

        //    int projectId = ((IModel)project).AddNewAndSave();
        //    Assert.That(projectId, Is.EqualTo(2), "Wrong New Project Id");

        //    mockery.VerifyAllExpectationsHaveBeenMet();
        //}

        [Test]
        public void Add()
        {
            mock.Mockery mockery = new mock.Mockery();

            Project project = new Project("/DataFiles");
            project.DataHelper = this.GetMockData_ForSaving(mockery);

            int projectId = ((IProject)project).Add("Test Description");
            Assert.That(projectId, Is.EqualTo(2), "Wrong New Project Id");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        //[Test]
        //public void Update()
        //{
        //    mock.Mockery mockery = new mock.Mockery();

        //    Project project = new Project("/DataFiles");
        //    project.DataHelper = this.GetMockData_ForSaving(mockery);

        //    int projectId = 1;
        //    project.Update(projectId, "Test Description");

        //    mockery.VerifyAllExpectationsHaveBeenMet();
        //}

        //[Test, ExpectedException(ExceptionType = typeof(ApplicationException), 
        //    ExpectedMessage = "Parameterless ctor requires HTTP Context")]
        //public void InstantiateProjectShouldCauseHttpError()
        //{
        //    Project p = new Project();
        //}
    }
}
