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
using Microsoft.AspNet.Identity.EntityFramework;

namespace CapstoneProject.Controllers
{
    public class RestaurantsController : Controller
    {
        private CapstoneDBEntities db = new CapstoneDBEntities();


		// GET: Restaurants
		[Authorize]
        public ActionResult Index()
        {
			var restaurants = db.Restaurants.Include(r => r.RestaurantGroup);
			List<Restaurant> restaurantsSelected;

			if (User.IsInRole("owner"))
			{
				var ownerID = User.Identity.GetUserId();
				restaurantsSelected = restaurants.Where(x => x.RestaurantGroup.OwnerId == ownerID).ToList();
			}
			else
			{
				var managerID = User.Identity.GetUserId();
				var manager = db.UserProfiles.Find(managerID);
				var person = manager.Person;
				return RedirectToAction("Details", new { id = person.RestaurantId });
			}
			var restaurantVMs = restaurantsSelected.Select(x => new RestaurantVM(x));
			return View(restaurantVMs);
			//return View(restaurants.Where(x => x.RestaurantGroup.OwnerId == ownerID).ToList());
		}

        // GET: Restaurants/Details/5
		[Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
			ViewBag.Managers = new List<UserProfile>(db.UserProfiles.Where(x => x.Person.RestaurantId == id));
			ViewBag.OtherLocations = new List<Restaurant>(db.Restaurants.
				Where(x => x.GroupId == restaurant.GroupId && x.Id != id));
			var rvm = new RestaurantVM(restaurant);
            return View(rvm);
        }

		// GET: Restaurants/_DetailsPane/5
		[Authorize]
		public ActionResult _DetailsPane(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Restaurant restaurant = db.Restaurants.Find(id);
			if (restaurant == null)
			{
				return HttpNotFound();
			}
			ViewBag.Managers = new List<UserProfile>(db.UserProfiles.Where(x => x.Person.RestaurantId == id));
			var rvm = new RestaurantVM(restaurant);
			return PartialView(rvm);
		}

		// GET: Restaurants/Create
		[Authorize(Roles = "owner")]
        public ActionResult Create()
        {
			var ownerID = User.Identity.GetUserId();
			ViewBag.GroupId = new SelectList(db.RestaurantGroups.Where(x => x.OwnerId == ownerID), "Id", "Name");
			ViewBag.OwnerName = db.UserProfiles.Find(ownerID).Person.Name;
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "owner")]
		public ActionResult Create(RestaurantVM rvm)
        {
            if (ModelState.IsValid)
            {
				var userId = User.Identity.GetUserId();
				Restaurant restaurant = new Restaurant(rvm);
				if (rvm.CreateGroup)
				{
					restaurant.RestaurantGroup = new RestaurantGroup
					{
						Name = rvm.GroupName,
						OwnerId = userId
					};
				}
				else
				{
					restaurant.GroupId = rvm.GroupId;
				}
                db.Restaurants.Add(restaurant);
			
				
                db.SaveChanges();
                return RedirectToAction("Index");
            }

			var ownerID = User.Identity.GetUserId();
			ViewBag.GroupId = new SelectList(db.RestaurantGroups.Where(x => x.OwnerId == ownerID), 
				"Id", "Name", rvm.GroupId);
            return View(rvm);
        }

        // GET: Restaurants/Edit/5
		[Authorize(Roles = "owner")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
			var ownerID = User.Identity.GetUserId();
			ViewBag.GroupId = new SelectList(db.RestaurantGroups.Where(x => x.OwnerId == ownerID),
				"Id", "Name", restaurant.GroupId);
			var rvm = new RestaurantVM(restaurant);
            return View(rvm);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "owner")]
		public ActionResult Edit(RestaurantVM rvm)
        {
			var restaurant = db.Restaurants.Find(rvm.Id);
			restaurant.UpdateFromVM(rvm);
            if (ModelState.IsValid)
            {
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
			var ownerID = User.Identity.GetUserId();
			ViewBag.GroupId = new SelectList(db.RestaurantGroups.Where(x => x.OwnerId == ownerID), "Id", "Name", restaurant.GroupId);
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
		[Authorize(Roles = "owner")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
			var rvm = new RestaurantVM(restaurant);
            return View(rvm);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "owner")]
		public ActionResult DeleteConfirmed(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurant);
            db.SaveChanges();
            return RedirectToAction("Index");
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
