using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PioneerStore.Models;

namespace PioneerStore.Controllers
{
    public class Sales_BillsController : ApiController
    {
        private StoreDBEntities2 db = new StoreDBEntities2();

        // GET: api/Sales_Bills
        public IHttpActionResult Get()
        {
            return Ok("It works!");
        }

        // GET: api/Sales_Bills/5
        [ResponseType(typeof(Sales_Bills))]
        public IHttpActionResult GetSales_Bills(int id)
        {
            Sales_Bills sales_Bills = db.Sales_Bills.Find(id);
            if (sales_Bills == null)
            {
                return NotFound();
            }

            return Ok(sales_Bills);
        }

        // PUT: api/Sales_Bills/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSales_Bills(int id, Sales_Bills sales_Bills)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sales_Bills.ID)
            {
                return BadRequest();
            }

            db.Entry(sales_Bills).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Sales_BillsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Sales_Bills
        [ResponseType(typeof(Sales_Bills))]
        public IHttpActionResult PostSales_Bills(Sales_Bills sales_Bills)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sales_Bills.Add(sales_Bills);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sales_Bills.ID }, sales_Bills);
        }

        // DELETE: api/Sales_Bills/5
        [ResponseType(typeof(Sales_Bills))]
        public IHttpActionResult DeleteSales_Bills(int id)
        {
            Sales_Bills sales_Bills = db.Sales_Bills.Find(id);
            if (sales_Bills == null)
            {
                return NotFound();
            }

            db.Sales_Bills.Remove(sales_Bills);
            db.SaveChanges();

            return Ok(sales_Bills);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Sales_BillsExists(int id)
        {
            return db.Sales_Bills.Count(e => e.ID == id) > 0;
        }
    }
}