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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CapstoneProject.Controllers
{
    public class EmployeesController : Controller
    {
        private CapstoneDBEntities db = new CapstoneDBEntities();
		
        // GET: Employees
		[Authorize]
        public ActionResult Index(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ViewBag.RestaurantId = id;
			Restaurant restaurant = db.Restaurants.Find(id);
			ViewBag.RestaurantName = new RestaurantVM(restaurant).Name;
			return View(restaurant.People.ToList());
        }

		// GET: Employees
		[Authorize]
		[ChildActionOnly]
		public ActionResult _IndexPane(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ViewBag.RestaurantId = id;
			Restaurant restaurant = db.Restaurants.Find(id);
			return PartialView(restaurant.People.ToList());
		}

		// GET: Employees/Details/5
		[Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

		// GET: Employees/Create
		[Authorize]
        public ActionResult Create(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Person person = new Person { RestaurantId = id };
            return View(person);
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public ActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = person.RestaurantId});
            }

            //ViewBag.RestaurantId = new SelectList(db.Restaurants, "Id", "Address", person.RestaurantId);
            return View(person);
        }

        // GET: Employees/Edit/5
		[Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
			var ownerID = User.Identity.GetUserId();
            ViewBag.RestaurantId = new SelectList(db.Restaurants.Where(x => x.RestaurantGroup.OwnerId == ownerID), 
				"Id", "Address", person.RestaurantId);
            return View(person);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
        public ActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
				return RedirectToAction("Index", new { id = person.RestaurantId });
			}
			var ownerID = User.Identity.GetUserId();
			ViewBag.RestaurantId = new SelectList(db.Restaurants.Where(x => x.RestaurantGroup.OwnerId == ownerID),
				"Id", "Address", person.RestaurantId);
			return View(person);
        }

		// GET: Employees/Edit/5
		[Authorize]
		public ActionResult AddToBalance(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Person person = db.People.Find(id);
			if (person == null)
			{
				return HttpNotFound();
			}
			var balanceVM = new EmployeeBalanceVM(person);
			return View(balanceVM);
		}

		// POST: Employees/AddToBalance
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult AddToBalance(EmployeeBalanceVM balanceVM)
		{
			Person person = db.People.Find(balanceVM.EmployeeId);
			if (ModelState.IsValid)
			{
				person.BalanceOwed += balanceVM.ReceiptAmt + balanceVM.BonusAmt;
				db.Entry(person).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index", new { id = person.RestaurantId });
			}
			return View(balanceVM);
		}

		// GET: Employees/Edit/5
		[Authorize]
		public ActionResult PayOffBalance(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Person person = db.People.Find(id);
			if (person == null)
			{
				return HttpNotFound();
			}
			var balanceVM = new EmployeeBalanceVM(person);
			return View(balanceVM);
		}

		// POST: Employees/AddToBalance
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult PayOffBalance(EmployeeBalanceVM balanceVM)
		{
			Person person = db.People.Find(balanceVM.EmployeeId);
			if (ModelState.IsValid)
			{	
				if (person.BalanceOwed >= balanceVM.ReimburseAmt)
				{
					person.BalanceOwed -= balanceVM.ReimburseAmt;
					db.Entry(person).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index", new { id = person.RestaurantId });
				}
				else
				{
					ModelState.AddModelError("ReimburseAmt", "This amount exceeds the balance owed.");
				}
			}
			balanceVM.BalanceOwed = person.BalanceOwed;
			return View(balanceVM);
		}

		// GET: Employees/Delete/5
		[Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
			var restaurantId = person.RestaurantId;
			if (person.UserProfiles.Any())
			{
				using (var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>())
				{
					var user = userManager.FindById(person.UserProfiles.First().Id);
					userManager.Delete(user);
				}
				var profile = person.UserProfiles.SingleOrDefault();
				db.UserProfiles.Remove(profile);
			}
            db.People.Remove(person);
            db.SaveChanges();
			return RedirectToAction("Index", new { id = restaurantId });
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
