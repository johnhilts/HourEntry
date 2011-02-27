using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HourEntry.Services.Data
{
    /// <summary>
    /// DTO
    /// </summary>
    public class DefaultTimeSheet
    {
        public List<short> HourList { get; set; }
        public List<short> MinuteList { get; set; }
        public List<string> AmPmList { get; set; }
    }
}
