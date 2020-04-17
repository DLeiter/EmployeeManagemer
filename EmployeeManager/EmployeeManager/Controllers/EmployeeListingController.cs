using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeManager.Models;

namespace EmployeeManager.Controllers
{
	public class EmployeeListingController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}