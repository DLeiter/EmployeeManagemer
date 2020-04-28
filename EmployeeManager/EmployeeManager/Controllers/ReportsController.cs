using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Controllers
{
	public class ReportsController : Controller
	{
		private readonly EmployeeManager_DBContext _context;

		public ReportsController(EmployeeManager_DBContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			ViewBag.Terminations = _context.Employees.Where(e => e.Terminated).Count();
			ViewData["Controller"] = "Reports";
			ViewData["BarGraphAction"] = "GetBarGraphDetailsHireWeek";
			ViewData["PieChartAction"] = "GetPieChartDeatilsRetnetionRate";
			return View();
		}

		public IActionResult DepartmentSpread()
		{
			var departments = _context.Departments.ToList();
			var employeesAssignedToDepartments = _context.Employees.Where(e => !string.IsNullOrWhiteSpace(e.DepartmentId));

			//var jsonObj = Json
			ViewData["Controller"] = "Reports";
			ViewData["BarGraphAction"] = "GetBarGraphDetailsDepartmentSpread";

			return View();
		}

		public IActionResult ManagerSpread()
		{
			var departments = _context.Departments.ToList();
			var employeesAssignedToDepartments = _context.Employees.Where(e => !string.IsNullOrWhiteSpace(e.DepartmentId));

			//var jsonObj = Json
			ViewData["Controller"] = "Reports";
			ViewData["BarGraphAction"] = "GetBarGraphDetailsManagerSpread";

			return View();
		}

		[HttpGet]
		public JsonResult GetBarGraphDetailsManagerSpread()
		{
			//could be a dictionary
			var Managers = _context.Employees.Where(e => _context.Employees.Where(em => em.ManagerId == e.Id).Count() > 0).ToArray();
			List<int> EmployeesPerManager = new List<int>();

			foreach (var m in Managers)
			{
				EmployeesPerManager.Add(_context.Employees.Where(e => e.ManagerId == m.Id).Count());
			}

			return new JsonResult(new
			{
				Categories = Managers.Select(m => $"ID:{m.Id.Trim()} Name:{m.FirstName + " " + m.LastName}").ToArray(),
				CategoryNumbers = EmployeesPerManager.ToArray()
			});
		}

		[HttpGet]
		public JsonResult GetBarGraphDetailsDepartmentSpread()
		{
			//could be a dictionary
			var Departments = _context.Departments.ToArray();
			List<int> EmployeesPerDepartment = new List<int>();

			foreach (var d in Departments)
			{
				EmployeesPerDepartment.Add(_context.Employees.Where(e => e.DepartmentId == d.Id).Count());
			}

			return new JsonResult(new
			{
				Categories = Departments.Select(d => d.Department).ToArray(),
				CategoryNumbers = EmployeesPerDepartment.ToArray()
			});
		}

		[HttpGet]
		public JsonResult GetBarGraphDetailsHireWeek()
		{
			var date = new DateTime(2020, 04, 19);
			return new JsonResult(new
			{
				Categories = new string[] { "03/29", "04/05", "04/12", "04/19" },
				CategoryNumbers = new int[] { 0, 0, 0, _context.Employees.Where(e => e.StartDate > date).Count() }
			});
		}

		[HttpGet]
		public JsonResult GetPieChartDeatilsRetnetionRate()
		{
			return new JsonResult(new
			{
				Terminated = _context.Employees.Where(e => e.Terminated).Count(),
				Quit = _context.Employees.Where(e => !e.Terminated && e.EndDate != null).Count(),
				Employeed = _context.Employees.Where(e => e.EndDate == null).Count(),
			});
		}
	}
}