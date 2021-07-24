using PioneerStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PioneerStore.Controllers
{
    public class ValuesController : ApiController
    {
        StoreDBEntities2 db = new StoreDBEntities2();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        
        public IHttpActionResult Post([FromBody]SalesBills SalesBill)
        {
            SalesBill.Bill.BillDate = DateTime.Now;
            Random r = new Random();
            SalesBill.Bill.BillNumber = r.Next(1, 1000000000);
            //geting the data in value then cheek validation for every singl item.
            var bill = SalesBill.Bill;
            decimal finalTotal = 0;
            //var cheekbilldate = false;
            var items = SalesBill.Items;
            bool cheeckItemStoreID = false;
            bool cheeckItemID = false;
            bool cheeckItemQuantity = false;
            bool cheeckItemPrice = false;
            bool cheeckItemAddedTax = false;
            bool cheeckItemTotal = false;
            foreach (var item in items)
            {
                var itemobject = db.Categories.Find(item.ItemID);
                if (item.StoreID > 0)
                {
                    var storeobject = db.Stores.Find(item.StoreID);
                    if (storeobject != null) { cheeckItemStoreID = true; }
                }
                if (item.ItemID > 0)
                {
                    if (itemobject != null) { cheeckItemID = true; }
                }
                var itemQuantityInStore = db.CategoriesQuantities.Where(i => i.ItemID == item.ItemID && i.StoreID == item.StoreID);
                foreach (var quantityitem in itemQuantityInStore)
                {
                    if (item.Quantity > 0 && quantityitem.Quantity >= item.Quantity)
                    {
                        cheeckItemQuantity = true;
                    }
                }

                if (item.Price > 0) { cheeckItemPrice = true; }

                item.AddedTax = item.Price * item.Quantity * (Convert.ToDecimal(itemobject.AddedTax) / 100);
                item.Total = (item.Price * item.Quantity) + item.AddedTax;
                if (item.AddedTax >= 0) { cheeckItemAddedTax = true; }
                if (item.Total > 0) { cheeckItemTotal = true; }
                finalTotal = finalTotal + item.Total;
            }
            bill.Total = finalTotal;
            bill.Remain = finalTotal - bill.Payed;
            bool cheeckItems = false;
            if (cheeckItemStoreID && cheeckItemID && cheeckItemQuantity && cheeckItemPrice && cheeckItemAddedTax && cheeckItemTotal) { cheeckItems = true; }
            //Checking if the bill number is repeated or not
            if (bill.BillNumber > 0 && bill.BillDate != null && bill.ClientID >= 1 && bill.Total >= 0 && bill.Payed >= 0 && bill.Remain >= 0 && cheeckItems)
            {
                var bills = db.Sales_Bills.ToList();
                bool foundflag = false;
                foreach (var b in bills)
                {
                    if (b.BillNumber == bill.BillNumber)
                    {
                        foundflag = true;
                    }
                }
                if (foundflag == false) { db.Sales_Bills.Add(bill); }
                db.SaveChanges();
                List<Sales_Bills> IDS = db.Sales_Bills.ToList();
                int sid = 0;
                foreach (var id in IDS)
                {
                    sid = id.ID;
                }

                if (items == null)
                {
                    items = new List<Sales_Bills_Details>();
                }
                //List<Purchases_Bills> IDS = db.Purchases_Bills.ToList();
                if (foundflag == false)
                {
                    foreach (var item in items)
                    {
                        item.BillID = sid;
                        var updateItemQuantity = db.CategoriesQuantities.Where(i => i.ItemID == item.ItemID && i.StoreID == item.StoreID);
                        foreach (var i in updateItemQuantity)
                        {
                            i.Quantity = i.Quantity - item.Quantity;
                            db.Entry(i).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        db.Sales_Bills_Details.Add(item);
                    }

                    db.SaveChanges();
                }
                return Json(foundflag);
            }
            else
            {
                return Json(true);
            }
        }
            // PUT api/< controller >/ 5
            public void Put(int id, [FromBody]string value)
            {
            }

            //  DELETE api/< controller >/ 5
            public void Delete(int id)
            {
            }
         
    }
}