using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HourEntry.Web.Models
{
    /// <summary>
    /// DTO to carry data from controller to view
    /// </summary>
    public class TimeSheetModel
    {
        [Required]
        [DisplayName("Start Time:")]
        public int StartHour { get; set; }

        [Required]
        public short StartMinute { get; set; }

        [Required]
        public string StartAmPm { get; set; }

        [Required]
        [DisplayName("End Time:")]
        public int EndHour { get; set; }

        [Required]
        public short EndMinute { get; set; }

        [Required]
        public string EndAmPm { get; set; }

        [Required]
        [DisplayName("Project:")]
        public List<string> ProjectList { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Comments:")]
        public string Comments { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Start Date:")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("End Date:")]
        public DateTime EndDate { get; set; }
    }
}