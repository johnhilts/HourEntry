using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HourEntry.Infrastructure.Database;
using HourEntry.Infrastructure.Database.Data;

namespace HourEntry.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public List<ProjectData> GetListOfAllProjects()
        {
            return this._projectRepository.GetProjectList();
        }

        public ProjectData GetProjectDataByProjectId(int projectId)
        {
            return this._projectRepository.GetProjectDataByProjectId(projectId);
        }

        public void SaveProjectData(ProjectData projectData)
        {
            this._projectRepository.SaveProjectData(projectData);
        }
    }
}
