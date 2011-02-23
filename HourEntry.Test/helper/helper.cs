using System;
using System.Collections.Generic;
using System.Data;
using HourEntry.Infrastructure.Database.Data;

namespace UnitTests.HourEntry.Helpers
{
    internal class helper
    {
        internal static DataTable GetMockHoursData()
        {
            return helper.GetMockHoursData(false);
        }
        internal static DataTable GetMockHoursData(bool addMultiple)
        {
            return helper.GetMockHoursData(DateTime.Today, DateTime.Today, addMultiple);
        }
        internal static DataTable GetMockHoursData(DateTime startDate, DateTime endDate)
        {
            return helper.GetMockHoursData(startDate, endDate, false);
        }
        internal static DataTable GetMockHoursData(DateTime startDate, DateTime endDate, bool addMultiple)
        {
            DataTable hours = new DataTable("Hours");
            hours.Columns.Add("RowID", typeof(System.Int32));
            hours.Columns["RowID"].AutoIncrement = true;
            hours.Columns["RowID"].AutoIncrementSeed = 0;
            hours.Columns["RowID"].AutoIncrementStep = 1;
            hours.Columns.Add("Hours", typeof(System.Decimal));
            hours.Columns.Add("ProjectID", typeof(System.Int32));
            hours.Columns.Add("StartDate", typeof(System.DateTime));
            hours.Columns.Add("EndDate", typeof(System.DateTime));
            hours.Columns.Add("Comments", typeof(System.String));
            AddHourRow(startDate, endDate, hours, 1);
            if (addMultiple)
            {
                AddHourRow(startDate, endDate, hours, 2);
                AddHourRow(startDate.AddDays(5), endDate.AddDays(5), hours, 2);
            }

            return hours;
        }
        private static void AddHourRow(DateTime startDate, DateTime endDate, DataTable hours, int projectId)
        {
            DataRow dr = hours.NewRow();
            // TODO: need to get rid of these magic values
            dr["RowID"] = 1;
            dr["Hours"] = 1.5;
            dr["ProjectID"] = projectId;
            dr["StartDate"] = startDate;
            dr["EndDate"] = endDate;
            dr["Comments"] = "This is a Test.";
            hours.Rows.Add(dr);
        }

        internal static List<HourEntryData> GetMockHoursList()
        {
            return helper.GetMockHoursList(false);
        }

        internal static List<HourEntryData> GetMockHoursList(bool addMultiple)
        {
            return helper.GetMockHoursList(DateTime.Today, DateTime.Today, addMultiple);
        }

        internal static List<HourEntryData> GetMockHoursList(DateTime startDate, DateTime endDate)
        {
            return helper.GetMockHoursList(startDate, endDate, false);
        }

        internal static List<HourEntryData> GetMockHoursList(DateTime startDate, DateTime endDate, bool addMultiple)
        {
            List<HourEntryData> hourList = new List<HourEntryData>();
            AddHourRow(startDate, endDate, hourList, 1);
            if (addMultiple)
            {
                AddHourRow(startDate, endDate, hourList, 2);
                AddHourRow(startDate.AddDays(5), endDate.AddDays(5), hourList, 2);
            }

            return hourList;
        }

        private static void AddHourRow(DateTime startDate, DateTime endDate, List<HourEntryData> hourList, int projectId)
        {
            // TODO: need to get rid of these magic values
            hourList.Add(new HourEntryData
                          {
                              HourEntryId = 1,
                              Hours = 1.5m,
                              ProjectId = projectId,
                              StartDate = startDate,
                              EndDate = endDate,
                              Comments = "This is a Test.",
                          });
        }


        internal static DataTable GetMockProjectData()
        {
            return GetMockProjectData(false);
        }
        internal static DataTable GetMockProjectData(bool addMultiple)
        {
            DataTable projects = new DataTable("Projects");
            projects.Columns.Add("ProjectID", typeof(System.Int32));
            projects.Columns.Add("Description", typeof(System.String));
            projects.Columns["ProjectID"].AutoIncrement = true;
            projects.Columns["ProjectID"].AutoIncrementSeed = 0;
            projects.Columns["ProjectID"].AutoIncrementStep = 1;
            AddProjectRow(projects, 1, "This is a Test.");
            if (addMultiple)
                AddProjectRow(projects, 2, "This is another Test.");
            return projects;
        }

        internal static List<ProjectData> GetMockProjectList()
        {
            return GetMockProjectList(false);
        }

        internal static List<ProjectData> GetMockProjectList(bool addMultiple)
        {
            List<ProjectData> projectList = new List<ProjectData>();
            AddProjectRow(projectList, 1, "This is a Test.");
            if (addMultiple)
                AddProjectRow(projectList, 2, "This is another Test.");
            return projectList;
        }

        private static void AddProjectRow(DataTable projects, int projectId, string description)
        {
            DataRow dr = projects.NewRow();
            dr["ProjectID"] = projectId;
            dr["Description"] = description;
            projects.Rows.Add(dr);
        }

        private static void AddProjectRow(List<ProjectData> projectList, int projectId, string description)
        {
            projectList.Add(new ProjectData
                                {
                                    ProjectId = projectId,
                                    Description = description,
                                });
        }
    }
}
