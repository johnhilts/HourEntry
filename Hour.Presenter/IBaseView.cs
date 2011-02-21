using System;

namespace PresentationLayer.HourEntry
{
    public interface IBaseView
    {
        bool IsPostBack { get; }
        void Redirect(string path);
        void Update();

        string DataPath { get; }

        void PageEditFocus();

        void PageValidate();
        bool IsValid { get; }
    }

    public class Constants
    {
        public static readonly string RowId = "RowId";
    }
}
