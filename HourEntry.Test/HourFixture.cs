using System;
using System.Data;
using Bll.HourEntry.Dal;
using NUnit.Framework;
using Moq;

using Bll.HourEntry;
using dal = Bll.HourEntry.Dal;
using UnitTests.HourEntry.Helpers;

namespace UnitTests.HourEntry
{
    [TestFixture]
    public class HourFixture
    {
        [Test]
        public void List()
        {
            Mock<dal.IDataHelper> mockData = this.GetMockData();
            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = mockData.Object;
            DataTable hours = ((IHour)hour).List();
            Assert.That(hours, Is.Not.Null, "Hours NULL");
            Assert.That(hours.Rows.Count, Is.GreaterThan(0), "Hours - no data");

            mockData.VerifyAll();
        }
        private Mock<dal.IDataHelper> GetMockData()
        {
            Mock<dal.IDataHelper> mockData = new Mock<IDataHelper>();
            DataTable mockHours = helper.GetMockHoursData();
            mockData.Setup(x => x.GetData("HourEntries")).Returns(mockHours);

            return mockData;
        }

        [Test]
        public void GetRecord()
        {
            Mock<dal.IDataHelper> mockData = this.GetMockData();
            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = mockData.Object;
            int rowId = 1;
            ((IHour)hour).GetRecord(rowId);
            Assert.That(hour.RowId, Is.EqualTo(1), "Hour ID Wrong");
            Assert.That(hour.Hours, Is.EqualTo(1.5M), "Hours Wrong");
            Assert.That(hour.StartDate, Is.EqualTo(DateTime.Today), "Start Date Wrong Wrong");
            Assert.That(hour.EndDate, Is.EqualTo(DateTime.Today), "End Date Wrong Wrong");
            Assert.That(hour.ProjectId, Is.EqualTo(1), "Project ID Wrong");
            Assert.That(hour.Comments, Is.EqualTo("This is a Test."), "Comments Wrong");

            mockData.VerifyAll();
        }
        [Test]
        public void GetRecord_NoRecords()
        {
            Mock<dal.IDataHelper> mockData = this.GetMockData();
            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = mockData.Object;
            int rowId = 0;
            DataTable hourTable = ((IHour)hour).GetRecord(rowId);
            Assert.That(hourTable.Rows.Count, Is.EqualTo(0), "Should be no rows");

            mockData.VerifyAll();
        }

        [Test]
        public void Update()
        {
            Mock<dal.IDataHelper> mockData = this.GetMockData_ForSaving();

            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = mockData.Object;

            int rowId = 1;
            int projectId = 1;
            decimal hours = 1.5M;
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            string comments = "This is a Test.";
            ((IHour)hour).Update(rowId, projectId, hours, startDate, endDate, comments);

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

            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = mockData.Object;

            int projectId = 1;
            decimal hours = 1.5M;
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            string comments = "This is a Test.";
            int hourId = ((IHour)hour).Add(projectId, hours, startDate, endDate, comments);
            Assert.That(hourId, Is.EqualTo(2), "Wrong New Row Id");

            mockData.VerifyAll();
        }
    }
}
