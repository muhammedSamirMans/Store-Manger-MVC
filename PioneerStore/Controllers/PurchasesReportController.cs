using PioneerStore.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PioneerStore.Controllers
{
    public class PurchasesReportController : Controller
    {
        // GET: PurchasesReport
        private StoreDBEntities2 db = new StoreDBEntities2();
        public ActionResult Index(DateTime? date1, DateTime? date2)
        {
            var reportList = db.Purchases_Bills.Where(s => EntityFunctions.TruncateTime(s.BillDate) >= date1 && EntityFunctions.TruncateTime(s.BillDate) <= date2);
            return View(reportList);
        }
        //printing bill with rotativa
        public ActionResult IndexById(int? id)
        {
            var bill = db.Purchases_Bills.Find(id);
            var billItems = db.Purchases_Bills_Details.Where(s => s.BillID == id);
            PurechasesBill purchasesBills = new PurechasesBill();
            purchasesBills.Bill = bill;
            purchasesBills.Items = billItems.ToList();
            return View(purchasesBills);
        }
        public ActionResult PrintSalarySlip(int id)
        {
            var report = new ActionAsPdf("IndexById", new { id = id });
            return report;
        }
    }
}