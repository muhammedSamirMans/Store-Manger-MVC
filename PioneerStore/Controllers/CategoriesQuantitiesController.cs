using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PioneerStore.Models;

namespace PioneerStore.Controllers
{
    public class CategoriesQuantitiesController : Controller
    {
        private StoreDBEntities2 db = new StoreDBEntities2();

        // GET: CategoriesQuantities
        public ActionResult Index()
        {
            var categoriesQuantities = db.CategoriesQuantities.Include(c => c.Category).Include(c => c.Store);
            return View(categoriesQuantities.ToList());
        }

        // GET: CategoriesQuantities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriesQuantity categoriesQuantity = db.CategoriesQuantities.Find(id);
            if (categoriesQuantity == null)
            {
                return HttpNotFound();
            }
            return View(categoriesQuantity);
        }

        // GET: CategoriesQuantities/Create
        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Store_Name");
            return View();
        }

        // POST: CategoriesQuantities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ItemID,StoreID,Quantity")] CategoriesQuantity categoriesQuantity)
        {
            var cheeckItem = db.Categories.Find(categoriesQuantity.ItemID);
            var cheeckStore = db.Stores.Find(categoriesQuantity.StoreID);
            if (ModelState.IsValid && (categoriesQuantity.Quantity > 0 || categoriesQuantity.Quantity == 0)&& categoriesQuantity.StoreID != 0 && categoriesQuantity.ItemID != 0 && cheeckItem != null && cheeckStore != null)
            {
                var categoriesQuantites = db.CategoriesQuantities.ToList();
                bool foundflag = false;
                foreach(var item in categoriesQuantites)
                {
                    if(item.ItemID==categoriesQuantity.ItemID && item.StoreID == categoriesQuantity.StoreID)
                    {
                        foundflag = true;
                    }
                }
                if (foundflag == false)
                {
                    db.CategoriesQuantities.Add(categoriesQuantity);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                    return RedirectToAction("Repeted");
            }

            ViewBag.ItemID = new SelectList(db.Categories, "ID", "Name", categoriesQuantity.ItemID);
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Store_Name", categoriesQuantity.StoreID);
            return View(categoriesQuantity);
        }

        // GET: CategoriesQuantities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriesQuantity categoriesQuantity = db.CategoriesQuantities.Find(id);
            if (categoriesQuantity == null)
            {
                return HttpNotFound();
            }
         
            ViewBag.ItemID = new SelectList(db.Categories, "ID", "Name", categoriesQuantity.ItemID);
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Store_Name", categoriesQuantity.StoreID);
            return View(categoriesQuantity);
        }

        // POST: CategoriesQuantities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include ="ID,Quantity")] CategoriesQuantity categoriesQuantity)
        {
            var cheekid = db.CategoriesQuantities.Find(categoriesQuantity.ID);
            if (ModelState.IsValid && cheekid!=null && (categoriesQuantity.Quantity > 0 || categoriesQuantity.Quantity == 0))
            {
                        var quantity = db.CategoriesQuantities.Find(categoriesQuantity.ID);
                        quantity.Quantity = categoriesQuantity.Quantity;
                        db.SaveChanges();
                        return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Categories, "ID", "Name", categoriesQuantity.ItemID);
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Store_Name", categoriesQuantity.StoreID);
            return View(categoriesQuantity);
        }

        //// GET: CategoriesQuantities/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CategoriesQuantity categoriesQuantity = db.CategoriesQuantities.Find(id);
        //    if (categoriesQuantity == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(categoriesQuantity);
        //}

        //// POST: CategoriesQuantities/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    CategoriesQuantity categoriesQuantity = db.CategoriesQuantities.Find(id);
        //    db.CategoriesQuantities.Remove(categoriesQuantity);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        public ActionResult Repeted()
        {
            return View();
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
