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
	public class EmployeesController : Controller
	{
		private readonly EmployeeManager_DBContext _context;

		public EmployeesController(EmployeeManager_DBContext context)
		{
			_context = context;
		}

		// GET: Employees
		public async Task<IActionResult> Index()
		{
			var employeeManager_DBContext = _context.Employees.OrderBy(e => e.StartDate).Include(e => e.Department).Include(e => e.Manager).Include(e => e.Position);
			return View(await employeeManager_DBContext.ToListAsync());
		}

		// GET: Employees/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.Include(e => e.Department)
				.Include(e => e.Manager)
				.Include(e => e.Position)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// GET: Employees/Create
		public IActionResult Create()
		{
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department");
			ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "LastName", null);
			ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position");
			return View();
		}

		// POST: Employees/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,FirstName,MiddleName,LastName,Address,Email,PhoneNumber,PositionId,DepartmentId,ExtraPermissions,StartDate,EndDate,Terminated,TerminationNotes,Shift,ManagerId,PhotoId,FavoriteColor")] Employees employee)
		{
			try
			{
				int maxNum = _context.Employees.Select(x => int.Parse(x.Id)).Max();
				employee.Id = $"{++maxNum}";
			}
			catch (Exception)
			{
				//Use this for logging in the future
				throw;
			}

			if (ModelState.IsValid)
			{
				_context.Add(employee);
				_context.UpdateActivityLogAsync(ActionEnum.Create, $"Created new employee. ID:{employee.Id}", GetType());
				//_context.UpdateActivityLogAsync("Create", $"Created new employee:{employee.Id}", "Employee");
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department", employee.DepartmentId);
			ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "LastName", employee.ManagerId);
			ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position", employee.PositionId);
			return View(employee);
		}

		// GET: Employees/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department", employee.DepartmentId);
			ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "LastName", employee.ManagerId);
			ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position", employee.PositionId);
			return View(employee);
		}

		// POST: Employees/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,MiddleName,LastName,Address,Email,PhoneNumber,PositionId,DepartmentId,ExtraPermissions,StartDate,EndDate,Terminated,TerminationNotes,Shift,ManagerId,PhotoId,FavoriteColor")] Employees employee)
		{
			if (id != employee.Id)
			{
				return NotFound();
			}

			if (employee.Terminated && employee.EndDate == null)
			{
				employee.EndDate = DateTime.UtcNow;
			}

			if (ModelState.IsValid)
			{
				try
				{
					var oldEmployee = _context.Employees.Where(e => e.Id == id).FirstOrDefault();
					_context.Entry(oldEmployee).State = EntityState.Detached;

					//var changes = "";

					if (employee.ExtraPermissions != oldEmployee.ExtraPermissions)
					{
						_context.UpdateActivityLogAsync(ActionEnum.Update, $"Emp ID:{employee.Id} Change:Permissions '{oldEmployee.ExtraPermissions}' -> '{employee.ExtraPermissions}'", GetType());
					}
					if (employee.ManagerId != oldEmployee.ManagerId)
					{
						_context.UpdateActivityLogAsync(ActionEnum.Update, $"Emp ID:{employee.Id} Change:Manager '{oldEmployee.ManagerId}' -> '{employee.ManagerId}'", GetType());
					}
					if (employee.PositionId != oldEmployee.PositionId)
					{
						_context.UpdateActivityLogAsync(ActionEnum.Update, $"Emp ID:{employee.Id} Change:Position '{oldEmployee.PositionId}' -> '{employee.PositionId}'", GetType());
					}

					_context.Update(employee);

					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!EmployeesExists(employee.Id))
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
			ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Department", employee.DepartmentId);
			ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "LastName", employee.ManagerId);
			ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Position", employee.PositionId);
			return View(employee);
		}

		// GET: Employees/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees
				.Include(e => e.Department)
				.Include(e => e.Manager)
				.Include(e => e.Position)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);
		}

		// POST: Employees/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var employee = await _context.Employees.FindAsync(id);
			_context.Employees.Remove(employee);
			_context.UpdateActivityLogAsync(ActionEnum.Delete, $"Emp ID:{employee.Id}", GetType());
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool EmployeesExists(string id)
		{
			return _context.Employees.Any(e => e.Id == id);
		}
	}
}
