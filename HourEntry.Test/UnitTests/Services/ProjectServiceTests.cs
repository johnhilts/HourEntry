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
    public class ProjectServiceTests
    {
        [Test]
        public void Should_Be_Able_To_Get_List_Of_All_Projects()
        {
            // arrange
            Mock<IProjectRepository> mockProjectRepository = this.GetMockProjectRepository_List();
            ProjectService projectService = new ProjectService(mockProjectRepository.Object);

            // action
            List<ProjectData> projectList = projectService.GetListOfAllProjects();

            // assert
            Assert.That(projectList, Is.Not.Null, "Projects NULL");
            Assert.That(projectList.Count, Is.GreaterThan(0), "Projects - no data");
            mockProjectRepository.VerifyAll();
        }

        private Mock<IProjectRepository> GetMockProjectRepository_List()
        {
            Mock<IProjectRepository> mockData = new Mock<IProjectRepository>();
            mockData.Setup(x => x.GetProjectList()).Returns(helper.GetMockProjectList());

            return mockData;
        }

        [Test]
        public void With_Valid_Project_ID_Should_Be_Able_To_Get_Record()
        {
            // arrange
            int projectId = 1;
            Mock<IProjectRepository> mockProjectRepository = this.GetMockProjectRepository_GetRecord(projectId);
            ProjectService projectService = new ProjectService(mockProjectRepository.Object);

            // action
            ProjectData projectData = projectService.GetProjectDataByProjectId(projectId);

            // assert
            Assert.That(projectData.ProjectId, Is.EqualTo(projectId), "Project ID Wrong");
            Assert.That(projectData.Description, Is.EqualTo("This is a Test."), "Project Description Wrong");
            mockProjectRepository.VerifyAll();
        }

        private Mock<IProjectRepository> GetMockProjectRepository_GetRecord(int projectId)
        {
            Mock<IProjectRepository> mockData = new Mock<IProjectRepository>();
            mockData.Setup(x => x.GetProjectDataByProjectId(projectId)).Returns(helper.GetMockProjectList().Where(x => x.ProjectId == projectId).SingleOrDefault());

            return mockData;
        }

        [Test]
        public void Non_Existent_Project_ID_Should_Get_No_Records()
        {
            // arrange
            int projectId = 0;
            Mock<IProjectRepository> mockProjectRepository = this.GetMockProjectRepository_GetRecord(projectId);
            ProjectService projectService = new ProjectService(mockProjectRepository.Object);

            // action
            ProjectData projectData = projectService.GetProjectDataByProjectId(projectId);

            // assert
            Assert.That(projectData, Is.Null, "Should be no project data (NULL)");
            mockProjectRepository.VerifyAll();
        }

        [Test]
        public void Should_Be_Able_To_Update()
        {
            // arrange
            int projectId = 1;
            ProjectData projectData = helper.GetMockProjectList().Where(x => x.ProjectId == projectId).SingleOrDefault();
            Mock<IProjectRepository> mockProjectRepository = this.GetMockProjectRepository_ForSaving(projectData);
            ProjectService projectService = new ProjectService(mockProjectRepository.Object);
            string description = "This is a Test.";

            // action
            projectData.Description = description;
            projectService.SaveProjectData(projectData);

            // assert
            mockProjectRepository.VerifyAll();
        }

        private Mock<IProjectRepository> GetMockProjectRepository_ForSaving(ProjectData projectData)
        {
            Mock<IProjectRepository> mockData = new Mock<IProjectRepository>();
            mockData.Setup(x => x.SaveProjectData(projectData));

            return mockData;
        }

        [Test]
        public void Should_Be_Able_To_Add()
        {
            // arrange
            ProjectData projectData = new ProjectData {Description = "Test Description",};
            Mock<IProjectRepository> mockProjectRepository = this.GetMockProjectRepository_ForSaving(projectData);
            ProjectService projectService = new ProjectService(mockProjectRepository.Object);

            // action
            projectService.SaveProjectData(projectData);

            // assert
            mockProjectRepository.VerifyAll();
        }

    }

}
