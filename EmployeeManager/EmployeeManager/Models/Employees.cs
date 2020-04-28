using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Models
{
	public partial class Employees
	{
		public Employees()
		{
			InverseManager = new HashSet<Employees>();
		}

		public string Id { get; set; }
		[Required]
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		[Required]
		public string LastName { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string PositionId { get; set; }
		public string DepartmentId { get; set; }
		public string ExtraPermissions { get; set; }
		[Required]
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public bool Terminated { get; set; }
		public string TerminationNotes { get; set; }
		[Required]
		public string Shift { get; set; }
		public string ManagerId { get; set; }
		public string PhotoId { get; set; }
		public string FavoriteColor { get; set; }

		public Departments Department { get; set; }
		public Employees Manager { get; set; }
		public Positions Position { get; set; }
		public ICollection<Employees> InverseManager { get; set; }
	}
}
