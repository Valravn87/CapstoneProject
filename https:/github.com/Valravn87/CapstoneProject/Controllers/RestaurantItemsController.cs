using System;
using System.Collections;
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

namespace CapstoneProject.Controllers
{
    public class RestaurantItemsController : Controller
    {
        private CapstoneDBEntities db = new CapstoneDBEntities();

        // GET: RestaurantItems
		[Authorize]
        public ActionResult Index(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ViewBag.RestaurantId = id;
			ViewBag.AllStocks = db.Stocks.ToList();
			Restaurant restaurant = db.Restaurants.Find(id);
			ViewBag.RestaurantName = restaurant.RestaurantGroup.Name + " - " + restaurant.LocationName;
			
            return View(restaurant.RestaurantItems.ToList());
        }

		[Authorize]
		[ChildActionOnly]
		public ActionResult _IndexPane(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ViewBag.AllStocks = db.Stocks.ToList();
			ViewBag.RestaurantId = id;
			Restaurant restaurant = db.Restaurants.Find(id);
			return PartialView(restaurant.RestaurantItems.ToList());
		}

		// POST
		[HttpPost]
		[Authorize]
		public ActionResult ChangeStock(int? id, int? code)
		{
			var restItem = db.RestaurantItems.Find(id);
			if (id != null && code != null)
			{				
				restItem.StockCode = code.Value;
				db.Entry(restItem).State = EntityState.Modified;
				db.SaveChanges();
			}			
			return RedirectToAction("Index", new { id = restItem.RestaurantId });
		}

        // GET: RestaurantItems/Details/5
		[Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RestaurantItem restaurantItem = db.RestaurantItems.Find(id);
            if (restaurantItem == null)
            {
                return HttpNotFound();
            }
            return View(restaurantItem);
        }

        // GET: RestaurantItems/Create
		[Authorize(Roles = "owner")]
        public ActionResult Create(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			//var ownerID = User.Identity.GetUserId();
			//var ourItems = db.RestaurantItems.Where(x => x.Restaurant.RestaurantGroup.OwnerId == ownerID).Select(x => x.ItemId);
			var myItemIds = db.RestaurantItems.Where(x => x.RestaurantId == id).Select(x => x.ItemId);
			var newItemsA = db.RestaurantItems.Where(x => x.RestaurantId != id && !myItemIds.Contains(x.ItemId)).Select(y => y.Item).ToList();
			var newItemsB = db.Items.Where(x => !x.RestaurantItems.Any()).ToList();
			var newItems = newItemsA.Concat(newItemsB).ToList();
			newItems.Sort();
			var rvm = new RestaurantVM(db.Restaurants.Find(id));
			ViewBag.RestaurantName = rvm.Name;
			ViewBag.ItemId = new SelectList(newItems, "Id", "Name");			
			ItemVM itemVM = new ItemVM { RestaurantId = id.Value };
            return View(itemVM);
        }

        // POST: RestaurantItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "owner")]
        public ActionResult Create(ItemVM itemVM)
        {
            if (ModelState.IsValid)
            {
				RestaurantItem restItem = new RestaurantItem
				{	/*RestaurantId = itemVM.RestaurantId,*/
					StockCode = 1
				};
                if (itemVM.NewItem)
				{
					restItem.Item = new Item {
						Name = itemVM.Name 
					};
				}
				else
				{
					restItem.ItemId = itemVM.ItemId;
				}
				Restaurant rest = db.Restaurants.Find(itemVM.RestaurantId);
				rest.RestaurantItems.Add(restItem);
				//db.RestaurantItems.Add(restItem);
				db.Entry(rest).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index", new { id = itemVM.RestaurantId});
            }
			int id = itemVM.RestaurantId;
			var myItemIds = db.RestaurantItems.Where(x => x.RestaurantId == id).Select(x => x.ItemId);
			var newItemsA = db.RestaurantItems.Where(x => x.RestaurantId != id && !myItemIds.Contains(x.ItemId)).Select(y => y.Item);
			var newItemsB = db.Items.Where(x => !x.RestaurantItems.Any());
			var newItems = newItemsA.Concat(newItemsB);
			ViewBag.ItemId = new SelectList(newItems, "Id", "Name", itemVM.ItemId);
			return View(itemVM);
        }

        // GET: RestaurantItems/Delete/5
		[Authorize(Roles = "owner")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RestaurantItem restaurantItem = db.RestaurantItems.Find(id);
            if (restaurantItem == null)
            {
                return HttpNotFound();
            }
            return View(restaurantItem);
        }

        // POST: RestaurantItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "owner")]
        public ActionResult DeleteConfirmed(int id)
        {
            RestaurantItem restaurantItem = db.RestaurantItems.Find(id);
            db.RestaurantItems.Remove(restaurantItem);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = restaurantItem.RestaurantId});
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
