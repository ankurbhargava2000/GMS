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
    public class PurchaseDetailsController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: PurchaseDetails
        public ActionResult Index(int? id)
        {
            var purchaseDetails = db.PurchaseDetails.Where(x => x.PurchaseId == id).Include(p => p.Product).Include(p => p.PurchaseOrder);
            ViewData["PurchaseId"] = id;            
            return View(purchaseDetails.ToList());
        }

        // GET: PurchaseDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseDetail purchaseDetail = db.PurchaseDetails.Find(id);
            if (purchaseDetail == null)
            {
                return HttpNotFound();
            }
            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Create/
        public ActionResult Create(int PurchaseId)
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            return View();
        }

        // POST: PurchaseDetails/Create       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PurchaseId,ProductId,Quantity,Rate,Description")] PurchaseDetail purchaseDetail)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseDetails.Add(purchaseDetail);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = purchaseDetail.PurchaseId });
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", purchaseDetail.ProductId);
            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseDetail purchaseDetail = db.PurchaseDetails.Find(id);
            if (purchaseDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", purchaseDetail.ProductId);
            return View(purchaseDetail);
        }

        // POST: PurchaseDetails/Edit/5    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PurchaseId,ProductId,Quantity,Rate,Description")] PurchaseDetail purchaseDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = purchaseDetail.PurchaseId });
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", purchaseDetail.ProductId);
            return View(purchaseDetail);
        }

        // GET: PurchaseDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseDetail purchaseDetail = db.PurchaseDetails.Find(id);
            if (purchaseDetail == null)
            {
                return HttpNotFound();
            }
            if (purchaseDetail != null)
            {
                db.PurchaseDetails.Remove(purchaseDetail);
                db.SaveChanges();                
            }
            return RedirectToAction("Index", new { id = purchaseDetail.PurchaseId });
        }

        // POST: PurchaseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseDetail purchaseDetail = db.PurchaseDetails.Find(id);
            db.PurchaseDetails.Remove(purchaseDetail);
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
