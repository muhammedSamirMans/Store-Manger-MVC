using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PioneerStore.Models;
using Rotativa;
namespace PioneerStore.Controllers
{
    public class SalesReportController : Controller
    {
        private StoreDBEntities2 db = new StoreDBEntities2();
        // GET: SalesReport
        public ActionResult Index(DateTime? date1,DateTime? date2)
        {
            List<ItemProfit> ItemsList = new List<ItemProfit>();
            List<Category> duplicateditems = new List<Category>();
            decimal totalProfit = 0;
            var reportList = db.Sales_Bills.Where(s=> EntityFunctions.TruncateTime(s.BillDate) >= date1 && EntityFunctions.TruncateTime(s.BillDate) <= date2);
            foreach(var item in reportList)
            {
                foreach(var detail in item.Sales_Bills_Details.ToList())
                {
                    decimal itemtotalprofit = (detail.Category.SillingPrice - detail.Category.PurechcastPrice) * detail.Quantity;
                    ItemProfit itemProfit= new ItemProfit();
                    itemProfit.item = detail.Category;
                    duplicateditems.Add(detail.Category);
                    itemProfit.profit = itemtotalprofit;
                    ItemsList.Add(itemProfit);
                    totalProfit = totalProfit + ((detail.Category.SillingPrice-detail.Category.PurechcastPrice)*detail.Quantity);
                }
            }
            ViewBag.TotalProfit = totalProfit;
            List<Category> nonduplicateditems = duplicateditems.Distinct().ToList();
            List<ItemProfit> finalItemsList = new List<ItemProfit>();
            foreach(var nondupitem in nonduplicateditems)
            {
                ItemProfit nonduplcatitemprofit = new ItemProfit();
                decimal totalitemprofit = 0;
                foreach (var item in ItemsList)
                {
                    if (nondupitem == item.item)
                    {
                        totalitemprofit = totalitemprofit + item.profit;
                    }
                }
                nonduplcatitemprofit.item = nondupitem;
                nonduplcatitemprofit.profit = totalitemprofit;
                finalItemsList.Add(nonduplcatitemprofit);
            }
            ViewBag.nonduplicateditems = nonduplicateditems;
            ViewBag.ItemsList = finalItemsList;
            return View(reportList);

        }

        //printing bill with rotativa
        public ActionResult IndexById(int? id)
        {
            var bill = db.Sales_Bills.Find(id);
            var billItems = db.Sales_Bills_Details.Where(s => s.BillID == id);
            SalesBills salesBills = new SalesBills();
            salesBills.Bill = bill;
            salesBills.Items = billItems.ToList();
            return View(salesBills);
        }

        public ActionResult PrintSalarySlip(int id)
        {
            var report = new ActionAsPdf("IndexById", new {id = id});
            return report;
        }
    }
}