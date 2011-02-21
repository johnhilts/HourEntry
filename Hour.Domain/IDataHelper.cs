using System;
using System.Data;

namespace Bll.HourEntry.Dal
{
    public interface IDataHelper
    {
        DataTable GetData(string dataObjectName);
        void SetData(string dataObjectName, DataTable dt);
    }
}
