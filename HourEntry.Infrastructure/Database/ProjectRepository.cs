using System;
using System.Collections.Generic;
using System.Linq;
using HourEntry.Infrastructure.Database.Data;

namespace HourEntry.Infrastructure.Database
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public List<ProjectData> GetProjectList()
        {
            var projectTable = base.GetData<ProjectData>();

            IQueryable<ProjectData> projectList = projectTable;

            return projectList.ToList();
        }

        public ProjectData GetProjectDataByProjectId(int projectId)
        {
            var projectTable = base.GetData<ProjectData>();

            IQueryable<ProjectData> projectData = projectTable;

            return projectData.Where(m => m.ProjectId.Equals(projectId)).SingleOrDefault();
        }

        public void SaveProjectData(ProjectData projectData)
        {
            if (this.GetProjectDataByProjectId(projectData.ProjectId) == null)
            {
                base.InsertData(projectData);
            }
            else
            {
                base.UpdateData(projectData);
            }
        }
    }
}