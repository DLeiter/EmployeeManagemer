using System;
using System.Collections.Generic;

namespace EmployeeManager.Models
{
    public partial class Positions
    {
        public Positions()
        {
            Employees = new HashSet<Employees>();
        }

        public string Id { get; set; }
        public string DpeartmentId { get; set; }
        public string Position { get; set; }
        public string Details { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? LastModified { get; set; }

        public ICollection<Employees> Employees { get; set; }
    }
}
