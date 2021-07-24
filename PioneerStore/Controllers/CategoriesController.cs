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
    public class CategoriesController : Controller
    {
        private StoreDBEntities2 db = new StoreDBEntities2();

        // GET: Categories
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.Unit).Include(c => c.Unit1);
            return View(categories.ToList());
        }

        // GET: Categories/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(category);
        //}

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.MainUint = new SelectList(db.Units.Where(mainunit =>mainunit.UnitType==1), "ID", "UnitName");
            ViewBag.SubUint = new SelectList(db.Units.Where(subunit => subunit.UnitType == 2), "ID", "UnitName");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,SillingPrice,PurechcastPrice,Code,MainUint,SubUint,AddedTax")] Category category)
        {
            var cheeckMain = db.Units.Find(category.MainUint);
            var cheeckSub = db.Units.Find(category.SubUint);
            if (ModelState.IsValid && cheeckMain !=null && cheeckSub !=null&& category.MainUint != 0 && category.SubUint != 0 && ((category.AddedTax >= 1 && category.AddedTax <= 100) || category.AddedTax == 0) && category.SillingPrice > 0 && category.PurechcastPrice > 0)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MainUint = new SelectList(db.Units, "ID", "UnitName", category.MainUint);
            ViewBag.SubUint = new SelectList(db.Units, "ID", "UnitName", category.SubUint);
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.MainUint = new SelectList(db.Units.Where(mainunit => mainunit.UnitType == 1), "ID", "UnitName");
            ViewBag.SubUint = new SelectList(db.Units.Where(subunit => subunit.UnitType == 2), "ID", "UnitName");
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,SillingPrice,PurechcastPrice,Code,MainUint,SubUint,AddedTax")] Category category)
        {
            var cheeckMain = db.Units.Find(category.MainUint);
            var cheeckSub = db.Units.Find(category.SubUint);
            var cheeckid = db.Categories.Find(category.ID);
            if (ModelState.IsValid && cheeckMain != null && cheeckSub != null && category.MainUint != 0 && category.SubUint != 0 && ((category.AddedTax >= 1 && category.AddedTax <= 100) || category.AddedTax ==  0) && category.SillingPrice > 0 && category.PurechcastPrice > 0 &&cheeckid!=null)
            {
                cheeckid.Name = category.Name;
                cheeckid.SillingPrice = category.SillingPrice;
                cheeckid.PurechcastPrice = category.PurechcastPrice;
                cheeckid.Code = category.Code;
                cheeckid.MainUint = category.MainUint;
                cheeckid.SubUint = category.SubUint;
                cheeckid.AddedTax = category.AddedTax;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MainUint = new SelectList(db.Units, "ID", "UnitName", category.MainUint);
            ViewBag.SubUint = new SelectList(db.Units, "ID", "UnitName", category.SubUint);
            return View(category);
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
