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
    public class PrinterChalansController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: PrinterChalans
        public ActionResult Index()
        {
            var printerChalans = db.PrinterChalans.Include(p => p.Vendor);
            return View(printerChalans.ToList());
        }

        // GET: PrinterChalans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrinterChalan printerChalan = db.PrinterChalans.Find(id);
            if (printerChalan == null)
            {
                return HttpNotFound();
            }
            return View(printerChalan);
        }

        // GET: PrinterChalans/Create
        public ActionResult Create()
        {
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName");
            return View();
        }

        // POST: PrinterChalans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VendorId,ChalanDate,Created,Updated,Description,IsGivenToPrinting")] PrinterChalan printerChalan)
        {
            if (ModelState.IsValid)
            {
                db.PrinterChalans.Add(printerChalan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", printerChalan.VendorId);
            return View(printerChalan);
        }

        // GET: PrinterChalans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrinterChalan printerChalan = db.PrinterChalans.Find(id);
            if (printerChalan == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", printerChalan.VendorId);
            return View(printerChalan);
        }

        // POST: PrinterChalans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VendorId,ChalanDate,Created,Updated,Description,IsGivenToPrinting")] PrinterChalan printerChalan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(printerChalan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", printerChalan.VendorId);
            return View(printerChalan);
        }

        // GET: PrinterChalans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrinterChalan printerChalan = db.PrinterChalans.Find(id);
            if (printerChalan == null)
            {
                return HttpNotFound();
            }
            return View(printerChalan);
        }

        // POST: PrinterChalans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrinterChalan printerChalan = db.PrinterChalans.Find(id);
            db.PrinterChalans.Remove(printerChalan);
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
