using System.Collections.Generic;
using HourEntry.Infrastructure.Database.Data;

namespace HourEntry.Infrastructure.Database
{
    public interface IProjectRepository
    {
        List<ProjectData> GetProjectList();
        ProjectData GetProjectDataByProjectId(int projectId);
        void SaveProjectData(ProjectData projectData);
    }
}