using StockManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StockManager.Controllers
{
    public class TailorChalanController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: TailorChalan
        public ActionResult Index(int? id)
        {
            var tailorChalans = db.TailorChalans.Include(p => p.Vendor).Where(p => p.IsGivenToTailor == (id == 1 ? true : false)).Include(p => p.TailorChalanDetails).Include(p => p.TailorChalanDetails1);
            ViewBag.Send = id;
            return View(tailorChalans.ToList());
        }

        // GET: TailorChalan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TailorChalan tailorChalan = db.TailorChalans.Find(id);
            if (tailorChalan == null)
            {
                return HttpNotFound();
            }
            return View(tailorChalan);
        }

        // GET: TailorChalan/Create
        public ActionResult Create(int? id)
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName");
            ViewBag.GivenToTailor = id;
            return View();
        }

        // POST: TailorChalan/Create       
        [HttpPost]
        public JsonResult Create(TailorChalan tailorChalan)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime dtDate = DateTime.Now;
                    tailorChalan.Created = dtDate;
                    tailorChalan.Updated = dtDate;

                    db.TailorChalans.Add(tailorChalan);
                    db.SaveChanges();
                    int scope_id = tailorChalan.Id;
                    transaction.Commit();
                    return Json(Convert.ToString(scope_id));
                }
                catch
                {
                    transaction.Rollback();
                    ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", tailorChalan.VendorId);
                    ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
                }
            }
            return Json("0");
        }

        // GET: TailorChalan/Edit/5
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
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", printerChalan.VendorId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", printerChalan.VendorId);
            return View(printerChalan);
        }

        // POST: TailorChalan/Edit/5        
        [HttpPost]
        public JsonResult Edit(PrinterChalan printerChalan)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var objPurchaseDetails in printerChalan.PrinterChalanDetails)
                    {
                        if (objPurchaseDetails.Id == 0)
                        {
                            db.Entry(objPurchaseDetails).State = EntityState.Added;
                            db.SaveChanges();
                        }
                    }

                    while (printerChalan.PrinterChalanDetails.Where(x => x.Id == 0).Count() > 0)
                        printerChalan.PrinterChalanDetails.Remove(printerChalan.PrinterChalanDetails.Where(x => x.Id == 0).ToList()[0]);

                    DateTime dtDate = DateTime.Now;
                    printerChalan.Updated = dtDate;
                    db.Entry(printerChalan).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Commit();
                    return Json(Convert.ToString(printerChalan.Id));
                }
                catch
                {
                    transaction.Rollback();
                    ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", printerChalan.VendorId);
                    ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
                }
            }
            return Json("0");
        }

        // GET: TailorChalan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TailorChalan tailorChalan = db.TailorChalans.Find(id);
            db.TailorChalans.Remove(tailorChalan);
            db.SaveChanges();
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
                    TailorChalanDetail tailorChalanDetail = (TailorChalanDetail)db.TailorChalanDetails.Where(x => x.Id == id).FirstOrDefault();
                    db.TailorChalanDetails.Remove(tailorChalanDetail);
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
