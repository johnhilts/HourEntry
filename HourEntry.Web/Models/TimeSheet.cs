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
        public List<short> HourList { get; set; }

        public List<short> MinuteList { get; set; }
        
        public List<string> AmPmList { get; set; }
        
        public List<string> ProjectList { get; set; }
        
        public string Comments { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
    }
}