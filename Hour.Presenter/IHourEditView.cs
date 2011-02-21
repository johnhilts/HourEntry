using System;
using System.Web.UI.WebControls;

namespace PresentationLayer.HourEntry
{
    public interface IHourEditView : IBaseView
    {
        int RowId { get; }
        TextBox Hours { get; }
        TextBox Comments { get; }
        TextBox Project { get; }
        DropDownList Projects { get; }
        Calendar StartDate { get; }
        Calendar EndDate { get; }
    }
}
