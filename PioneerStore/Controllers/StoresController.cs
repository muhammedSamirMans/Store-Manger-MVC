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
    public class StoresController : Controller
    {
        private StoreDBEntities2 db = new StoreDBEntities2();

        // GET: Stores
        public ActionResult Index()
        {
            return View(db.Stores.ToList());
        }

        // GET: Stores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // GET: Stores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Store_Name,Store_Address,Storekeeper")] Store store)
        {
            if (ModelState.IsValid)
            {
                var storelist = db.Stores.ToList();
                bool foundflag = false;
                foreach(var fstore in storelist) 
                {
                    if (fstore.Store_Address == store.Store_Address && fstore.Store_Name == store.Store_Name)
                    {
                        foundflag = true;
                    }
                }
                if (!foundflag)
                {
                    db.Stores.Add(store);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Repeted");
                }

            }

            return View(store);
        }

        // GET: Stores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Store_Name,Store_Address,Storekeeper")] Store store)
        {
            var cheekid = db.Stores.Find(store.ID);
                if (ModelState.IsValid && cheekid != null)
                {
                    cheekid.Storekeeper = store.Storekeeper;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            
            return View(store);
        }

        //// GET: Stores/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Store store = db.Stores.Find(id);
        //    if (store == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(store);
        //}

        //// POST: Stores/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Store store = db.Stores.Find(id);
        //    var categoriesQuantity = db.CategoriesQuantities.ToList();
        //    bool flag = false;
        //    foreach(var item in categoriesQuantity)
        //    {
        //        if (item.StoreID == store.ID)
        //        {
        //            flag = true;
        //        }
        //    }
        //    if (flag == false)
        //    {
        //        db.Stores.Remove(store);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("CannotDeleteOrEdit");
        //}
        public ActionResult CannotDeleteOrEdit()
        {
            return View();
        }
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
