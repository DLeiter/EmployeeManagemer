using System;
using System.Collections.Generic;

namespace EmployeeManager.Models
{
    public partial class Photos
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public byte[] Photo { get; set; }
    }
}
