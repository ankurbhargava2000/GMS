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
    public class PurchaseOrdersController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: PurchaseOrders
        public ActionResult Index()
        {
            var purchaseOrders = db.PurchaseOrders.Include(p => p.Vendor).Include(p => p.PurchaseDetails);
            return View(purchaseOrders.ToList());
        }

        // GET: PurchaseOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Create
        public ActionResult Create()
        {
            var purchaseOrders = db.PurchaseOrders.Include(p => p.Vendor).Include(p => p.PurchaseDetails);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            return View();
        }

        [HttpPost]
        public JsonResult Create(PurchaseOrder purchaseOrder)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime dtDate = DateTime.Now;
                    purchaseOrder.Created = dtDate;
                    purchaseOrder.Updated = dtDate;

                    db.PurchaseOrders.Add(purchaseOrder);
                    db.SaveChanges();
                    int scope_id = purchaseOrder.Id;
                    transaction.Commit();
                    return Json(Convert.ToString(scope_id));
                }
                catch
                {
                    transaction.Rollback();
                    ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", purchaseOrder.VendorId);
                    ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
                }
            }
            return Json("0");
        }
        // GET: PurchaseOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", purchaseOrder.VendorId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5     
        [HttpPost]
        public ActionResult Edit(PurchaseOrder purchaseOrder)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var objPurchaseDetails in purchaseOrder.PurchaseDetails)
                    {
                        if (objPurchaseDetails.Id == 0)
                        {
                            db.Entry(objPurchaseDetails).State = EntityState.Added;
                            db.SaveChanges();
                        }
                    }

                    while (purchaseOrder.PurchaseDetails.Where(x => x.Id == 0).Count() > 0)
                        purchaseOrder.PurchaseDetails.Remove(purchaseOrder.PurchaseDetails.Where(x => x.Id == 0).ToList()[0]);

                    DateTime dtDate = DateTime.Now;
                    purchaseOrder.Updated = dtDate;
                    db.Entry(purchaseOrder).State = EntityState.Modified;
                    db.SaveChanges();                 

                    transaction.Commit();
                    return Json(Convert.ToString(purchaseOrder.Id));
                }
                catch
                {
                    transaction.Rollback();
                    ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", purchaseOrder.VendorId);
                    ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
                }
            }
            return Json("0");
        }

        // GET: PurchaseOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (db.PurchaseDetails != null)
                    {
                        foreach (PurchaseDetail obj in db.PurchaseDetails.Where(x => x.PurchaseId == id))
                        {
                            db.PurchaseDetails.Remove(obj);
                        }
                    }
                    db.PurchaseOrders.Remove(purchaseOrder);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                }
            }
            return RedirectToAction("Index");
        }

        public JsonResult DeleteProduct(int? id)
        {
            if (id == null)
            {
                return Json("Error in deleting product.");
            }
            try
            {
                if (id != null && id != 0)
                {
                    PurchaseDetail purchaseDetail = (PurchaseDetail)db.PurchaseDetails.Where(x => x.Id == id).FirstOrDefault();
                    db.PurchaseDetails.Remove(purchaseDetail);
                    db.SaveChanges();
                    return Json("product deleted Successfully.");
                }
            }
            catch
            {
            }
            return Json("Error in deleting product.");
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
