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
                           MinuteList = this.GetMinuteList(),
                           AmPmList = this.GetAmPmList(),
                       };
        }

        private List<short> GetHourList()
        {
            List<short> hourList = new List<short>();
            for (short hour = 1; hour <= 12; hour++)
            {
                hourList.Add(hour);
            }

            return hourList;
        }

        private List<short> GetMinuteList()
        {
            List<short> minuteList = new List<short>();
            for (short minute = 0; minute <= 45; minute += 15)
            {
                minuteList.Add(minute);
            }

            return minuteList;
        }

        private List<string> GetAmPmList()
        {
            return new List<string> { "AM", "PM" };
        }
    }
}
