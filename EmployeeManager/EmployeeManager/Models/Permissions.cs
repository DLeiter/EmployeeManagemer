using System;
using System.Collections.Generic;

namespace EmployeeManager.Models
{
    public partial class Permissions
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Permission { get; set; }
        public string Details { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
