using System;
using System.Web.UI.WebControls;

namespace PresentationLayer.HourEntry
{
    public interface ITimeSheetView : IBaseView
    {
        DropDownList StartHour { get; }
        DropDownList StartTime { get; }
        DropDownList StartAmPm { get; }
        DropDownList EndHour { get; }
        DropDownList EndTime { get; }
        DropDownList EndAmPm { get; }
        TextBox Comments { get; }
        TextBox Project { get; }
        DropDownList Projects { get; }
        Calendar StartDate { get; }
        Calendar EndDate { get; }
    }
}
