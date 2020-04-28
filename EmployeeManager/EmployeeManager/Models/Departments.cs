using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EmployeeManager.Models
{
    [BindProperties(SupportsGet = true)]
    public partial class Departments
    {
        public Departments()
        {
            Employees = new HashSet<Employees>();
        }

        public string Id { get; set; }
        public string Department { get; set; }
        public string Permissions { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? LastModified { get; set; }

        public ICollection<Employees> Employees { get; set; }
    }
}
