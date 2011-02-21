using System;
using System.Data;
using System.Configuration;

namespace Bll.HourEntry
{
    public class Project : BllBase, IProject
    {
        private int _ProjectId;
        public int ProjectId
        {
            get { return _ProjectId; }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
        }

        public Project()
            : base()
        {
        }
        public Project(string dataPath)
            : base(dataPath)
        {
        }

        public DataTable List()
        {
            DataTable projects = this._DataHelper.GetData("Projects");
            return projects;
        }
        public DataTable GetRecord(int projectId)
        {
            DataTable projects = ((IProject)this).List();
            DataRow[] selectedProjects = projects.Select("ProjectID = " + projectId.ToString());
            if (selectedProjects.Length == 0)
            {
                projects.Clear();
                return projects;
            }

            DataRow project = selectedProjects[0];
            this._ProjectId = (int)project["ProjectID"];
            this._Description = project["Description"].ToString();

            // TODO: this is dumb - fix it - or at least put it into a Util function
            DataTable temp = projects.Clone();
            DataRow drTemp = temp.NewRow();
            foreach (DataColumn c in projects.Columns)
                drTemp[c.ColumnName] = project[c.ColumnName];
            temp.Rows.Add(drTemp);
            return temp;
        }
        int IProject.Add(string description)
        {
            DataTable projects = ((IProject)this).List();
            DataRow newProject = projects.NewRow();
            projects.Rows.Add(newProject);
            int newProjectId = (int)projects.Rows[projects.Rows.Count - 1]["ProjectID"];
            this.SaveRecord(projects, newProject, description);
            return newProjectId;
        }
        void IProject.Update(int projectId, string description)
        {
            DataTable projects = ((IProject)this).List();
            DataRow project = projects.Select("ProjectID = " + projectId.ToString())[0];
            this.SaveRecord(projects, project, description);
        }
        private void SaveRecord(DataTable projects, DataRow project, string description)
        {
            project["Description"] = description;
            this._DataHelper.SetData("Projects", projects);
        }
    }
}
