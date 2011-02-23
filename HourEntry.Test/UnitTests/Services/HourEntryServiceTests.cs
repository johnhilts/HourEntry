using System;
using System.Data;
using System.Linq;
using Bll.HourEntry.Dal;
using HourEntry.Infrastructure.Database;
using HourEntry.Infrastructure.Database.Data;
using HourEntry.Services;
using NUnit.Framework;
using Moq;
using Bll.HourEntry;
using UnitTests.HourEntry.Helpers;
using System.Collections.Generic;

namespace HourEntry.Test.UnitTests.Services
{
    [TestFixture]
    public class HourEntryServiceTests
    {
        [Test]
        public void Should_Be_Able_To_Get_List_Of_All_HourEntry_Entries()
        {
            // arrange
            Mock<IHourEntryRepository> mockHourEntryRepository = this.GetMockHourEntryRepository_List();
            HourEntryService hourEntryService = new HourEntryService(mockHourEntryRepository.Object);

            // action
            List<HourEntryData> hourEntryList = hourEntryService.GetListOfAllHourEntries();

            // assert
            Assert.That(hourEntryList, Is.Not.Null, "Hour Entries NULL");
            Assert.That(hourEntryList.Count, Is.GreaterThan(0), "Hour Entries - no data");
            mockHourEntryRepository.VerifyAll();
        }

        private Mock<IHourEntryRepository> GetMockHourEntryRepository_List()
        {
            Mock<IHourEntryRepository> mockData = new Mock<IHourEntryRepository>();
            mockData.Setup(x => x.GetHourEntryList()).Returns(helper.GetMockHoursList());

            return mockData;
        }

        [Test]
        public void With_Valid_HourEntry_ID_Should_Be_Able_To_Get_Record()
        {
            // arrange
            int hourEntryId = 1;
            Mock<IHourEntryRepository> mockHourEntryRepository = this.GetMockHourEntryRepository_GetRecord(hourEntryId);
            HourEntryService hourEntryService = new HourEntryService(mockHourEntryRepository.Object);

            // action
            HourEntryData hourEntryData = hourEntryService.GetHourEntryDataByHourEntryId(hourEntryId);

            // assert
            Assert.That(hourEntryData.HourEntryId, Is.EqualTo(1), "Hour ID Wrong");
            Assert.That(hourEntryData.Hours, Is.EqualTo(1.5M), "Hours Wrong");
            Assert.That(hourEntryData.StartDate, Is.EqualTo(DateTime.Today), "Start Date Wrong Wrong");
            Assert.That(hourEntryData.EndDate, Is.EqualTo(DateTime.Today), "End Date Wrong Wrong");
            Assert.That(hourEntryData.ProjectId, Is.EqualTo(hourEntryId), "Project ID Wrong");
            Assert.That(hourEntryData.Comments, Is.EqualTo("This is a Test."), "Comments Wrong");

            mockHourEntryRepository.VerifyAll();
        }

        private Mock<IHourEntryRepository> GetMockHourEntryRepository_GetRecord(int hourEntryId)
        {
            Mock<IHourEntryRepository> mockData = new Mock<IHourEntryRepository>();
            mockData.Setup(x => x.GetHourEntryDataByHourEntryId(hourEntryId)).Returns(helper.GetMockHoursList().Where(x => x.HourEntryId == hourEntryId).SingleOrDefault());

            return mockData;
        }

        [Test]
        public void Non_Existent_HourEntry_ID_Should_Get_No_Records()
        {
            // arrange
            int hourEntryId = 0;
            Mock<IHourEntryRepository> mockHourEntryRepository = this.GetMockHourEntryRepository_GetRecord(hourEntryId);
            HourEntryService hourEntryService = new HourEntryService(mockHourEntryRepository.Object);

            // action
            HourEntryData hourEntryData = hourEntryService.GetHourEntryDataByHourEntryId(hourEntryId);

            // assert
            Assert.That(hourEntryData, Is.Null, "Should be no hour entry data (NULL)");
            mockHourEntryRepository.VerifyAll();
        }

        [Test]
        public void Should_Be_Able_To_Update()
        {
            // arrange
            int hourEntryId = 1;
            HourEntryData hourEntryData = helper.GetMockHoursList().Where(x => x.HourEntryId == hourEntryId).SingleOrDefault();
            Mock<IHourEntryRepository> mockHourEntryRepository = this.GetMockHourEntryRepository_ForSaving(hourEntryData);
            HourEntryService hourEntryService = new HourEntryService(mockHourEntryRepository.Object);
            string comments = "This is a Test.";

            // action
            hourEntryData.Comments = comments;
            hourEntryService.SaveHourEntryData(hourEntryData);

            // assert
            mockHourEntryRepository.VerifyAll();
        }

        private Mock<IHourEntryRepository> GetMockHourEntryRepository_ForSaving(HourEntryData hourEntryData)
        {
            Mock<IHourEntryRepository> mockData = new Mock<IHourEntryRepository>();
            mockData.Setup(x => x.SaveHourEntryData(hourEntryData));

            return mockData;
        }

        [Test]
        public void Should_Be_Able_To_Add()
        {
            // arrange
            HourEntryData hourEntryData = new HourEntryData { Comments = "Test Comment", };
            Mock<IHourEntryRepository> mockHourEntryRepository = this.GetMockHourEntryRepository_ForSaving(hourEntryData);
            HourEntryService hourEntryService = new HourEntryService(mockHourEntryRepository.Object);

            // action
            hourEntryService.SaveHourEntryData(hourEntryData);

            // assert
            mockHourEntryRepository.VerifyAll();
        }

    }

}
