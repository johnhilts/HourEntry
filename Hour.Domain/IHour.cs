using System;
using System.Data;

namespace Bll.HourEntry
{
    public interface IHour
    {
        string DataPath { set;}

        DataTable List();
        DataTable GetRecord(int id);
        void Update(int id, int projectId, decimal hours, DateTime startDate, DateTime endDate, string comments);
        int Add(int projectId, decimal hours, DateTime startDate, DateTime endDate, string comments);
    }
}
