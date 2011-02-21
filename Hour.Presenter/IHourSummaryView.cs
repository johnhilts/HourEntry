using System;
using System.Web.UI.WebControls;

namespace PresentationLayer.HourEntry
{
    public interface IHourSummaryView : IBaseView
    {
        GridView HoursPerMonthList { get; }
        GridView HoursPerMonthPerProjectList { get; }
        GridView HoursPerWeekPerProjectList { get; }
    }
}
