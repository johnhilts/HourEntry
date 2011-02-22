using System;
using System.Data;
using Bll.HourEntry.Dal;
using NUnit.Framework;
using Moq;

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
            Mock<dal.IDataHelper> mockData = this.GetMockData();
            Project project = new Project("/DataFiles");
            project.DataHelper = mockData.Object;
            DataTable projects = ((IProject)project).List();
            Assert.That(projects, Is.Not.Null, "Projects NULL");
            Assert.That(projects.Rows.Count, Is.GreaterThan(0), "Projects - no data");

            mockData.VerifyAll();
        }
        private Mock<dal.IDataHelper> GetMockData()
        {
            Mock<dal.IDataHelper> mockData = new Mock<IDataHelper>();
            DataTable mockProjects = helper.GetMockProjectData();
            mockData.Setup(x => x.GetData("Projects")).Returns(mockProjects);

            return mockData;
        }

        [Test]
        public void GetRecord()
        {
            Mock<dal.IDataHelper> mockData = this.GetMockData();
            Project project = new Project("/DataFiles");
            project.DataHelper = mockData.Object;
            int projectId = 1;
            ((IProject)project).GetRecord(projectId);
            Assert.That(project.ProjectId, Is.EqualTo(1), "Project Story ID Wrong");
            Assert.That(project.Description, Is.EqualTo("This is a Test."), "Project Description Wrong");

            mockData.VerifyAll();
        }
        [Test]
        public void GetRecord_NoRecords()
        {
            Mock<dal.IDataHelper> mockData = this.GetMockData();
            Project project = new Project("/DataFiles");
            project.DataHelper = mockData.Object;
            int projectId = 0;
            DataTable projectTable = ((IProject)project).GetRecord(projectId);
            Assert.That(projectTable.Rows.Count, Is.EqualTo(0), "Should be no rows");

            mockData.VerifyAll();
        }

        [Test]
        public void Update()
        {
            Mock<dal.IDataHelper> mockData = this.GetMockData_ForSaving();
            Project project = new Project("/DataFiles");
            project.DataHelper = mockData.Object;

            int projectId = 1;
            string description = "This is a Test.";
            ((IProject)project).Update(projectId, description);

            mockData.VerifyAll();
        }
        private Mock<dal.IDataHelper> GetMockData_ForSaving()
        {
            Mock<dal.IDataHelper> mockData = this.GetMockData();
            // not sure why this fails mock.Expect.Once.On(mockData).Method("SetData").With(this.SetData(this.GetMockProjects()));
            //mockData.Setup(x => x.SetData("Projects", helper.GetMockProjectData()));
            //mock.Expect.Once.On(mockData).Method("SetData").WithAnyArguments();

            return mockData;
        }

        [Test]
        public void Add()
        {
            Mock<dal.IDataHelper> mockData = this.GetMockData_ForSaving();
            Project project = new Project("/DataFiles");
            project.DataHelper = mockData.Object;

            int projectId = ((IProject)project).Add("Test Description");
            Assert.That(projectId, Is.EqualTo(2), "Wrong New Project Id");

            mockData.VerifyAll();
        }

    }
}
