using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace HourEntry.Infrastructure.Database.Data
{
    [Table(Name = "ProjectInfo")]
    public class ProjectData
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ProjectId { get; set; }

        [Column]
        public string Description { get; set; }
    }
}
