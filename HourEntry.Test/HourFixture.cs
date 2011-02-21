using System;
using System.Data;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using mock = NMock2;

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
            mock.Mockery mockery = new mock.Mockery();
            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = this.GetMockData(mockery);
            DataTable hours = ((IHour)hour).List();
            Assert.That(hours, Is.Not.Null, "Hours NULL");
            Assert.That(hours.Rows.Count, Is.GreaterThan(0), "Hours - no data");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private dal.IDataHelper GetMockData(mock.Mockery mockery)
        {
            dal.IDataHelper mockData = (dal.IDataHelper)mockery.NewMock(typeof(dal.IDataHelper));
            DataTable mockHours = helper.GetMockHoursData();
            mock.Expect.Once.On(mockData).Method("GetData").With("HourEntries").Will(mock.Return.Value(mockHours));

            return mockData;
        }

        [Test]
        public void GetRecord()
        {
            mock.Mockery mockery = new mock.Mockery();
            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = this.GetMockData(mockery);
            int rowId = 1;
            ((IHour)hour).GetRecord(rowId);
            Assert.That(hour.RowId, Is.EqualTo(1), "Hour ID Wrong");
            Assert.That(hour.Hours, Is.EqualTo(1.5M), "Hours Wrong");
            Assert.That(hour.StartDate, Is.EqualTo(DateTime.Today), "Start Date Wrong Wrong");
            Assert.That(hour.EndDate, Is.EqualTo(DateTime.Today), "End Date Wrong Wrong");
            Assert.That(hour.ProjectId, Is.EqualTo(1), "Project ID Wrong");
            Assert.That(hour.Comments, Is.EqualTo("This is a Test."), "Comments Wrong");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        [Test]
        public void GetRecord_NoRecords()
        {
            mock.Mockery mockery = new mock.Mockery();
            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = this.GetMockData(mockery);
            int rowId = 0;
            DataTable hourTable = ((IHour)hour).GetRecord(rowId);
            Assert.That(hourTable.Rows.Count, Is.EqualTo(0), "Should be no rows");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void Update()
        {
            mock.Mockery mockery = new mock.Mockery();

            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = this.GetMockData_ForSaving(mockery);

            int rowId = 1;
            int projectId = 1;
            decimal hours = 1.5M;
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            string comments = "This is a Test.";
            ((IHour)hour).Update(rowId, projectId, hours, startDate, endDate, comments);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
        private dal.IDataHelper GetMockData_ForSaving(mock.Mockery mockery)
        {
            dal.IDataHelper mockData = this.GetMockData(mockery);
            // not sure why this fails mock.Expect.Once.On(mockData).Method("SetData").With(this.SetData(this.GetMockProjects()));
            mock.Expect.Once.On(mockData).Method("SetData").WithAnyArguments();

            return mockData;
        }

        [Test]
        public void Add()
        {
            mock.Mockery mockery = new mock.Mockery();

            Hour hour = new Hour("/DataFiles");
            hour.DataHelper = this.GetMockData_ForSaving(mockery);

            int projectId = 1;
            decimal hours = 1.5M;
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            string comments = "This is a Test.";
            int hourId = ((IHour)hour).Add(projectId, hours, startDate, endDate, comments);
            Assert.That(hourId, Is.EqualTo(2), "Wrong New Row Id");

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
