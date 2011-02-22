using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace HourEntry.Infrastructure.Database.Data
{
    [Table(Name = "HourEntryInfo")]
    public class HourEntryData
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int HourEntryId { get; set; }

        [Column]
        public int ProjectId { get; set; }

        [Column(DbType = "tinyint")]
        public short Hours { get; set; }

        [Column(DbType = "smalldatetime")]
        public DateTime StartDate { get; set; }

        [Column(DbType = "smalldatetime")]
        public DateTime EndDate { get; set; }

        [Column]
        public string Comments { get; set; }
    }
}
