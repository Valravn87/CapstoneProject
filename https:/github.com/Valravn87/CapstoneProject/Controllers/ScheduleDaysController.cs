using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapstoneProject.Models;
using CapstoneProject.Models.ViewModels;

namespace CapstoneProject.Controllers
{
    public class ScheduleDaysController : Controller
    {
        private CapstoneDBEntities db = new CapstoneDBEntities();

        // GET: ScheduleDays
		[Authorize]
        public ActionResult Index(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ViewBag.RestaurantId = id;
			var rest = db.Restaurants.Find(id);
			ViewBag.RestaurantName = new RestaurantVM(rest).Name;
			var employeeSchedules = rest.People.Select(x => new EmployeeScheduleVM(x));
            return View(employeeSchedules.ToList());
        }

		// GET: ScheduleDays
		[Authorize]
		[ChildActionOnly]
		public ActionResult _IndexPane(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var rest = db.Restaurants.Find(id);
			var employeeSchedules = rest.People.Select(x => new EmployeeScheduleVM(x));
			return PartialView(employeeSchedules.ToList());
		}

		//// Get
		//[Authorize]
		//public ActionResult Action()
		//{
		//	EmployeeScheduleVM scheduleVM = new EmployeeScheduleVM();
		//	return View(scheduleVM);
		//}

		//[HttpPost]
		//[Authorize]
		//public ActionResult Action(EmployeeScheduleVM scheduleVM)
		//{
		//	var x = ModelState.IsValid;
		//	return View(scheduleVM);
		//}

		// GET: ScheduleDays/Details/5
		[Authorize]
		public ActionResult Details(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Person employee = db.People.Find(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			ViewBag.RestaurantId = employee.RestaurantId;
			EmployeeScheduleVM scheduleVM = new EmployeeScheduleVM(employee);
			return View(scheduleVM);
		}

		// GET: ScheduleDays/Details/5
		[Authorize]
		public ActionResult _DetailsPane(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Person employee = db.People.Find(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			ViewBag.RestaurantId = employee.RestaurantId;
			EmployeeScheduleVM scheduleVM = new EmployeeScheduleVM(employee);
			return PartialView(scheduleVM);
		}

		// GET:
		[Authorize]
		public ActionResult EditSchedule(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Person employee = db.People.Find(id);
			if (employee == null)
			{
				return HttpNotFound();
			}
			ViewBag.RestaurantId = employee.RestaurantId;
			EmployeeScheduleVM scheduleVM = new EmployeeScheduleVM(employee);			
			return View(scheduleVM);
		}

		// Post
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult EditSchedule(EmployeeScheduleVM scheduleVM, int? id)
		{
			if (ModelState.IsValid)
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Person employee = db.People.Find(id);
				if (employee == null)
				{
					return HttpNotFound();
				}
				employee.UpdateSchedule(scheduleVM, db);
				db.Entry(employee).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index", new { id = employee.RestaurantId });
			}
			return View(scheduleVM);
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
