using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManager.Models;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeManager.Controllers
{
	public class ActivityLogController : Controller
	{
		private readonly EmployeeManager_DBContext _context;
		private readonly IMemoryCache _cache;

		public ActivityLogController(EmployeeManager_DBContext context, IMemoryCache cache)
		{
			_context = context;
			_cache = cache;
		}

		// GET: ActivityLog
		public async Task<IActionResult> Index()
		{
			ResetNotifications();
			return View(await _context.ActivityLog.ToListAsync());
		}

		private void ResetNotifications()
		{
			_cache.Set("OldActivityTime", DateTime.UtcNow);
		}

		[HttpGet]
		public JsonResult GetNotifications()
		{
			if (!_cache.TryGetValue("OldActivityTime", out DateTime oldActivityTime))
			{
				//notifications = _context.
				_cache.Set("OldActivityTime", DateTime.UtcNow);
			}
			else
			{
				_cache.Set("OldActivityTime", oldActivityTime);
			}

			int notificationCount = _context.ActivityLog.Where(a => a.AddDate > oldActivityTime).Count();

			return Json(new { message = notificationCount });
		}

		// GET: ActivityLog/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var activityLog = await _context.ActivityLog
				.FirstOrDefaultAsync(m => m.Id == id);
			if (activityLog == null)
			{
				return NotFound();
			}

			return View(activityLog);
		}

		//// GET: ActivityLog/Create
		//public IActionResult Create()
		//{
		//    return View();
		//}

		//// POST: ActivityLog/Create
		//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Id,Action,Details,AffectedTable,AddDate")] ActivityLog activityLog)
		//{
		//    if (ModelState.IsValid)
		//    {
		//        _context.Add(activityLog);
		//        await _context.SaveChangesAsync();
		//        return RedirectToAction(nameof(Index));
		//    }
		//    return View(activityLog);
		//}

		//// GET: ActivityLog/Edit/5
		//public async Task<IActionResult> Edit(string id)
		//{
		//    if (id == null)
		//    {
		//        return NotFound();
		//    }

		//    var activityLog = await _context.ActivityLog.FindAsync(id);
		//    if (activityLog == null)
		//    {
		//        return NotFound();
		//    }
		//    return View(activityLog);
		//}

		//// POST: ActivityLog/Edit/5
		//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(string id, [Bind("Id,Action,Details,AffectedTable,AddDate")] ActivityLog activityLog)
		//{
		//    if (id != activityLog.Id)
		//    {
		//        return NotFound();
		//    }

		//    if (ModelState.IsValid)
		//    {
		//        try
		//        {
		//            _context.Update(activityLog);
		//            await _context.SaveChangesAsync();
		//        }
		//        catch (DbUpdateConcurrencyException)
		//        {
		//            if (!ActivityLogExists(activityLog.Id))
		//            {
		//                return NotFound();
		//            }
		//            else
		//            {
		//                throw;
		//            }
		//        }
		//        return RedirectToAction(nameof(Index));
		//    }
		//    return View(activityLog);
		//}

		//// GET: ActivityLog/Delete/5
		//public async Task<IActionResult> Delete(string id)
		//{
		//    if (id == null)
		//    {
		//        return NotFound();
		//    }

		//    var activityLog = await _context.ActivityLog
		//        .FirstOrDefaultAsync(m => m.Id == id);
		//    if (activityLog == null)
		//    {
		//        return NotFound();
		//    }

		//    return View(activityLog);
		//}

		//// POST: ActivityLog/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> DeleteConfirmed(string id)
		//{
		//    var activityLog = await _context.ActivityLog.FindAsync(id);
		//    _context.ActivityLog.Remove(activityLog);
		//    await _context.SaveChangesAsync();
		//    return RedirectToAction(nameof(Index));
		//}

		private bool ActivityLogExists(string id)
		{
			return _context.ActivityLog.Any(e => e.Id == id);
		}
	}
}
