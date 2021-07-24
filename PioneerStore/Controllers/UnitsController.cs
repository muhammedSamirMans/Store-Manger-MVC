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
    public class UnitsController : Controller
    {
        private StoreDBEntities2 db = new StoreDBEntities2();

        // GET: Units
        public ActionResult Index()
        {
            return View(db.Units.ToList());
        }

        // GET: Units/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Unit unit = db.Units.Find(id);
        //    if (unit == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(unit);
        //}

        // GET: Units/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UnitName,UnitType,Precent")] Unit unit)
        {
            if (ModelState.IsValid && (unit.UnitType==1||unit.UnitType==2))
            {
                if (unit.UnitType == 1)
                {
                    unit.Precent = 1;
                }
                db.Units.Add(unit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unit);
        }

        // GET: Units/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UnitName,UnitType,Precent")] Unit unit)
        {
            var cheekunit = db.Units.Find(unit.ID);
            if (ModelState.IsValid &&cheekunit!=null && (unit.UnitType == 1 || unit.UnitType == 2)&&unit.Precent>0)
            {
                List<Category> categorys = db.Categories.ToList();
                bool foundedFalg = false;
                foreach (var cat in categorys)
                {
                    if (cat.Unit.ID == unit.ID || cat.Unit1.ID == unit.ID)
                    {
                        foundedFalg = true;
                    }
                }
                if (foundedFalg == false)
                {
                    if (unit.UnitType == 1) { unit.Precent = 1; }
                    cheekunit.UnitName = unit.UnitName;
                    cheekunit.UnitType = unit.UnitType;
                    cheekunit.Precent = unit.Precent;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("ConnotDeleteOrEdit");
                
            }
            return View(unit);
        }

        //// GET: Units/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Unit unit = db.Units.Find(id);
        //    if (unit == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(unit);
        //}

        //// POST: Units/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Unit unit = db.Units.Find(id);
        //    //Search if this uints givin to Category?
        //    List<Category> categorys = db.Categories.ToList();
        //    bool foundedFalg = false;
        //    foreach(var cat in categorys)
        //    {
        //        if(cat.Unit.ID==unit.ID || cat.Unit1.ID == unit.ID)
        //        {
        //            foundedFalg = true;
        //        }
        //    }
        //    if (foundedFalg == false)
        //    {
        //        db.Units.Remove(unit);
        //        db.SaveChanges();

        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("ConnotDeleteOrEdit");
        //}

        public ActionResult ConnotDeleteOrEdit()
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
