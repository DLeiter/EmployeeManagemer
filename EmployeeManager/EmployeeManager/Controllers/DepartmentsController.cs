using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManager.Models;

namespace EmployeeManager.Controllers
{
	public class DepartmentsController : Controller
	{
		private readonly EmployeeManager_DBContext _context;

		public DepartmentsController(EmployeeManager_DBContext context)
		{
			_context = context;
		}

		// GET: Departments
		public async Task<IActionResult> Index()
		{
			//return View(await _context.Departments.ToListAsync());
			ViewBag.Departments = _context.Departments.ToList();
			ViewBag.Employees = _context.Employees.Where(e => !string.IsNullOrWhiteSpace(e.ExtraPermissions)).ToList();
			//var permissionList = await _context.Permissions.ToListAsync();
			return View(await _context.Permissions.ToListAsync());
		}

		[HttpPost]
		public JsonResult UpdatePermissions(string id, string table, string[] permissionIdList, bool[] checkedList)
		{
			if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(table))
				return Json(new { result = "EMPTY VALUES" });
			//return;

			var permissionList = "";

			if (!checkedList.Contains(false))
			{
				permissionList = "*";
			}
			else if (checkedList.Contains(true))
			{
				for (int i = 0; i < permissionIdList.Length; i++)
				{
					if (checkedList[i])
					{
						permissionList += permissionIdList[i] + ",";
					}
				}
			}

			permissionList = permissionList.TrimEnd(',');

			switch (table)
			{
				case "Departments":
					var department = _context.Departments.Where(d => d.Id == id).FirstOrDefault();

					if (department.Permissions != permissionList)
					{
						_context.UpdateActivityLogAsync(ActionEnum.Update, $"Dep ID:{department.Id} Change:Permissions '{department.Permissions}' -> '{permissionList}'", GetType());
					}

					department.Permissions = permissionList;
					_context.Update(department);
					break;
				case "Employees":
					var employee = _context.Employees.Where(d => d.Id == id).FirstOrDefault();

					if (employee.ExtraPermissions != permissionList)
					{
						_context.UpdateActivityLogAsync(ActionEnum.Update, $"Emp ID:{employee.Id} Change:Permissions '{employee.ExtraPermissions}' -> '{permissionList}'", employee.GetType());
					}

					employee.ExtraPermissions = permissionList;
					_context.Update(employee);
					break;
			}
			_context.SaveChanges();
			return Json(new { result = "SUCCESS" });
			//return;
		}

		// GET: Departments/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var departments = await _context.Departments
				.FirstOrDefaultAsync(m => m.Id == id);
			if (departments == null)
			{
				return NotFound();
			}

			return View(departments);
		}

		// GET: Departments/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Departments/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Department,Permissions,AddDate,LastModified")] Departments departments)
		{
			try
			{
				int maxNum = _context.Departments.Select(x => int.Parse(x.Id)).Max();
				departments.Id = $"{++maxNum}";
			}
			catch (Exception)
			{
				//Use this for logging in the future
				throw;
			}

			departments.AddDate = DateTime.UtcNow;

			if (ModelState.IsValid)
			{
				_context.Add(departments);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(departments);
		}

		// GET: Departments/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var departments = await _context.Departments.FindAsync(id);
			if (departments == null)
			{
				return NotFound();
			}
			return View(departments);
		}

		// POST: Departments/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("Id,Department,Permissions,AddDate,LastModified")] Departments departments)
		{
			if (id != departments.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(departments);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!DepartmentsExists(departments.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(departments);
		}

		// GET: Departments/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var departments = await _context.Departments
				.FirstOrDefaultAsync(m => m.Id == id);
			if (departments == null)
			{
				return NotFound();
			}

			return View(departments);
		}

		// POST: Departments/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var departments = await _context.Departments.FindAsync(id);
			_context.Departments.Remove(departments);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool DepartmentsExists(string id)
		{
			return _context.Departments.Any(e => e.Id == id);
		}
	}
}
