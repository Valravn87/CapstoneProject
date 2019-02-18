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
    public class AlertsController : Controller
    {
        private CapstoneDBEntities db = new CapstoneDBEntities();

        // GET: Alerts
		[Authorize]
        public ActionResult Index(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Restaurant rest = db.Restaurants.Find(id);
			if (rest == null)
			{
				return HttpNotFound();
			}
			ViewBag.RestaurantId = id;
			ViewBag.RestaurantName = new RestaurantVM(rest).Name;
			var alerts = db.Alerts.Where(x => x.ItemAlerts.FirstOrDefault().RestaurantItem.RestaurantId == id).Include(a => a.Person);
            return View(alerts.ToList());
        }

		// GET: Alerts
		[Authorize]
		[ChildActionOnly]
		public ActionResult _IndexPane(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Restaurant rest = db.Restaurants.Find(id);
			if (rest == null)
			{
				return HttpNotFound();
			}
			var alerts = db.Alerts.Where(x => x.ItemAlerts.FirstOrDefault().RestaurantItem.RestaurantId == id).Include(a => a.Person);
			return PartialView(alerts.ToList());
		}

		// GET: Alerts/Details/5
		[Authorize]
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alert alert = db.Alerts.Find(id);
            if (alert == null)
            {
                return HttpNotFound();
            }
			ViewBag.RestaurantId = alert.ItemAlerts.FirstOrDefault().RestaurantItem.RestaurantId;
            return View(alert);
        }

		[ChildActionOnly]
		public ActionResult _DetailsPartial(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Alert alert = db.Alerts.Find(id);
			if (alert == null)
			{
				return HttpNotFound();
			}
			return PartialView(alert);
		}

		// GET
		[Authorize]
		public ActionResult PostAlert(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Restaurant rest = db.Restaurants.Find(id);
			if (rest == null)
			{
				return HttpNotFound();
			}
			return PartialView(id);
		}

		// POST 
		[HttpPost,ActionName("PostAlert")]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult PostAlertConfirmed(int id)
		{
			Restaurant rest = db.Restaurants.Find(id);
			var redItems = rest.RestaurantItems.Where(x => x.StockCode == 3);
			if (redItems.Any())
			{
				var alert = new Alert { TimeCreated = DateTime.Now };
				var alertItems = new List<ItemAlert>();
				foreach (var item in redItems)
				{
					alertItems.Add(new ItemAlert
					{
						RestaurantItemId = item.Id,
						StockCode = item.StockCode
					});
				}
				
				alert.ItemAlerts = alertItems;
				db.Alerts.Add(alert);
				db.SaveChanges();
				SelectEmployees(rest, alert);
			}
			return RedirectToAction("Index", new { id });
		}

		// GET
		[Authorize]
		public ActionResult PostAlertBoth(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Restaurant rest = db.Restaurants.Find(id);
			if (rest == null)
			{
				return HttpNotFound();
			}
			return PartialView(id);
		}

		// POST 
		[HttpPost, ActionName("PostAlertBoth")]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult PostAlertBothConfirmed(int id)
		{
			Restaurant rest = db.Restaurants.Find(id);
			var redAndYellowItems = rest.RestaurantItems.Where(x => x.StockCode == 3 || x.StockCode == 2);
			if (redAndYellowItems.Any())
			{
				var alert = new Alert { TimeCreated = DateTime.Now };
				var alertItems = new List<ItemAlert>();
				foreach (var item in redAndYellowItems)
				{
					alertItems.Add(new ItemAlert
					{
						RestaurantItemId = item.Id,
						StockCode = item.StockCode
					});
				}

				alert.ItemAlerts = alertItems;
				db.Alerts.Add(alert);
				db.SaveChanges();
				SelectEmployees(rest, alert);
			}
			return RedirectToAction("Index", new { id });
		}

		// GET Alerts/Respond/5?4
		public ActionResult Respond(int? id, int? employeeId)
		{
			if (id == null || employeeId == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var alert = db.Alerts.Find(id);
			var employee = db.People.Find(employeeId);
			if (alert == null || employee == null)
			{
				return HttpNotFound();
			}
			var evm = new EmployeeAlertVM(alert, employee);
			return View(evm);
		}

		// POST Alerts/Respond/...
		[HttpPost]
		public ActionResult Respond(EmployeeAlertVM evm)
		{
			var alert = db.Alerts.Find(evm.AlertId);
			var employee = db.People.Find(evm.EmployeeId);
			if (employee != null && alert.Person == null)
			{
				alert.TimeResponded = DateTime.Now;
				alert.PersonId = evm.EmployeeId;
				if (ModelState.IsValid)
				{
					db.Entry(alert).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index", "Home");
				}
			}
			return View(evm);
		}



		// GET: Alerts/Delete/5
		[Authorize]
		public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alert alert = db.Alerts.Find(id);
            if (alert == null)
            {
                return HttpNotFound();
            }
			ViewBag.RestaurantId = alert.ItemAlerts.FirstOrDefault().RestaurantItem.RestaurantId;
            return View(alert);
        }

        // POST: Alerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult DeleteConfirmed(int id)
        {
            Alert alert = db.Alerts.Find(id);
			int restaurantId = alert.ItemAlerts.FirstOrDefault().RestaurantItem.RestaurantId;
			var alertItems = db.ItemAlerts.Where(x => x.AlertId == id);
			db.ItemAlerts.RemoveRange(alertItems);
            db.Alerts.Remove(alert);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = restaurantId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

		private void SelectEmployees(Restaurant restaurant, Alert alert)
		{
			string restName = new RestaurantVM(restaurant).Name;
			int today = (int)DateTime.Today.DayOfWeek;
			var employees = db.ScheduleDays
				.Where(w => w.DayOfWeek == today).ToList()
				.Where(x => x.StartTime > DateTime.Now - DateTime.Today)
				.Select(y => y.Person)
				.Concat(restaurant.People.Where(x => x.UserProfiles.Any()))
				.Concat(new Person[] { restaurant.RestaurantGroup.OwnerProfile.Person })
				.Distinct()
				.Where(z => z.RestaurantId == restaurant.Id || z.RestaurantId == null).ToList();
			foreach (var employee in employees)
			{
				alert.SendAlert(employee, restName, Url);
			}
		}
	}
}
