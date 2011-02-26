using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HourEntry.Services.Data;

namespace HourEntry.Services
{
    /// <summary>
    /// Provides re-usable API for Presentation layer
    /// </summary>
    public class PresenterService
    {
        public DefaultTimeSheet GetDefaultTimeSheet()
        {
            return new DefaultTimeSheet
                       {
                           HourList = this.GetHourList(),
                       };
        }

        private List<short> GetHourList()
        {
            List<short> hourList = new List<short>();
            for (short i = 1; i < 12; i++)
            {
                hourList.Add(i);
            }

            return hourList;
        }

    }
}
