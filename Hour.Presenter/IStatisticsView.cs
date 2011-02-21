using System.Web.UI.WebControls;

namespace PresentationLayer.HourEntry
{
    public interface IStatisticsView : IBaseView
    {
        // input
        TextBox FromDate { get; }
        TextBox ToDate { get; }

        RadioButtonList DateRangePicker { get; }

        DropDownList Projects { get; }

        TextBox Comments { get; }

        // output
        GridView Statistics { get; }
    }
}
