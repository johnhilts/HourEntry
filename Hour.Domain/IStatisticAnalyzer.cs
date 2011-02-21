using System;
using System.Data;
using System.Collections.Generic;

namespace Bll.HourEntry
{
    public interface IStatisticAnalyzer
    {
        IHour Hours { set; }
        IProject Projects { set; }

        List<StatisticEntry> sumhours(DateTime startDate, DateTime endDate, int projectId, string comment);
    }
}
