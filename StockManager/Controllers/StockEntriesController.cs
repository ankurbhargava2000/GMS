using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockManager.Models;

namespace StockManager.Controllers
{
    [CheckAuth]
    public class StockEntriesController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: StockEntries
        public ActionResult Index()
        {
            var stockEntries = db.StockEntries.Include(s => s.Product).Include(s => s.Vendor);
            return View(stockEntries.ToList());
        }

        // GET: StockEntries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockEntry stockEntry = db.StockEntries.Find(id);
            if (stockEntry == null)
            {
                return HttpNotFound();
            }
            return View(stockEntry);
        }

        // GET: StockEntries/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName");
            return View();
        }

        // POST: StockEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,VendorId,Quantity,IsReceived,IsActive,DateCreated,DateUpdated")] StockEntry stockEntry)
        {
            if (ModelState.IsValid)
            {
                stockEntry.DateCreated = DateTime.Now;
                db.StockEntries.Add(stockEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", stockEntry.ProductId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", stockEntry.VendorId);
            return View(stockEntry);
        }

        // GET: StockEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockEntry stockEntry = db.StockEntries.Find(id);
            if (stockEntry == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", stockEntry.ProductId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", stockEntry.VendorId);
            return View(stockEntry);
        }

        // POST: StockEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,VendorId,Quantity,IsReceived,IsActive,DateCreated,DateUpdated")] StockEntry stockEntry)
        {
            if (ModelState.IsValid)
            {
                stockEntry.DateUpdated = DateTime.Now;
                db.Entry(stockEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", stockEntry.ProductId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", stockEntry.VendorId);
            return View(stockEntry);
        }

        // GET: StockEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockEntry stockEntry = db.StockEntries.Find(id);
            if (stockEntry == null)
            {
                return HttpNotFound();
            }
            return View(stockEntry);
        }

        // POST: StockEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockEntry stockEntry = db.StockEntries.Find(id);
            db.StockEntries.Remove(stockEntry);
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
