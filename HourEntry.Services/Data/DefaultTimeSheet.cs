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
        public int Hour { get; set; }
        public List<short> HourList { get; set; }
        public List<string> MinuteList { get; set; }
        public List<string> AmPmList { get; set; }
    }
}
