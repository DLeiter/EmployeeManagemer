using System;
using System.Collections.Generic;

namespace EmployeeManager.Models
{
    public partial class ActivityLog
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public string AffectedTable { get; set; }
        public DateTime AddDate { get; set; }
    }
}
