using System;
using dat = System.Data;
using System.Data;
using System.Collections.Generic;

namespace Bll.HourEntry
{
    public class StatisticAnalyzer : IStatisticAnalyzer
    {
        IHour _Hours;
        public IHour Hours { set { this._Hours = value; } }
        IProject _Projects;
        public IProject Projects { set { this._Projects = value; } }

        //# p = Project ID#, c = comment
        //# ex: sumhours(5/1/2008, 4, "PDR")
        //# use p=0 to ignore project ID
        //# use c="" to ignore comments
        public List<StatisticEntry> sumhours(DateTime startDate, DateTime endDate, int p, string c)
        {
            dat.DataTable hours = this._Hours.List();

            string where = "";
            where += "StartDate >= #" + startDate.ToShortDateString() + "#";
            where += " AND StartDate < #" + endDate.AddDays(1).ToShortDateString() + "#";

            // this can be useful elsewhere
            //if (start.DayOfWeek != System.DayOfWeek.Sunday)
            //{
            //    string warning = "* * * * NOTE: Converting " + start.ToString("d") + " to Sunday's date: ";
            //    start = start.AddDays(System.Convert.ToInt32(start.DayOfWeek) * -1);
            //    warning += start.ToString("d");
            //    Console.WriteLine(warning);
            //}
            //where += "StartDate >= #" + start.ToShortDateString() + "#";
            //where += "AND StartDate < #" + start.AddDays(daySpan).ToShortDateString() + "#";

            if (p != 0)
                where += " AND ProjectID = " + p.ToString();
            if (c != "")
                where += " AND Comments LIKE '%" + c.ToString() + "%'";

            dat.DataTable projects = this._Projects.List();
            string project = "";
            if (p != 0)
                project = projects.Select("ProjectID = '" + p.ToString() + "'")[0]["Description"].ToString();

            List<StatisticEntry> hourStats = new List<StatisticEntry>();
            foreach (dat.DataRow dr in hours.Select(where))
            {
                if (p == 0)
                {
                    string pid = dr["ProjectID"].ToString();
                    dat.DataRow[] selectedProjects = projects.Select("ProjectID = '" + pid + "'");
                    if (selectedProjects.Length == 1)
                        project = selectedProjects[0]["Description"].ToString();
                    else
                        project = "(missing project for #" + pid + ")";
                }

                StatisticEntry hourRow = new StatisticEntry(project, Convert.ToDecimal(dr["Hours"]), dr["Comments"].ToString(), 
                    Convert.ToDateTime(dr["StartDate"]));
                hourStats.Add(hourRow);
            }
            return hourStats;
        }
    }
}
