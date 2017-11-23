using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockManager.Models;
using PagedList;
namespace StockManager.Controllers
{
    [CheckAuth]
    public class PrintJobWorkReceivedController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: PrinterChalans
        public ActionResult Index( int? page)
        {
            var printerChalans = db.PrintJobWorkReceiveds.Include(p => p.Vendor).Include(p => p.PrintJobWorkReceivedDetails).OrderBy(x => x.ChalanDate);
            
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(printerChalans.ToPagedList(pageNumber, pageSize));
        }

        // GET: PrinterChalans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrintJobWorkReceived printerChalan = db.PrintJobWorkReceiveds.Find(id);
            if (printerChalan == null)
            {
                return HttpNotFound();
            }
            return View(printerChalan);
        }

        // GET: PrinterChalans/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2), "Id", "VendorName");            
            return View();
        }

        // POST: PrinterChalans/Create       
        [HttpPost]
        public JsonResult Create(PrintJobWorkReceived printerChalan)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime dtDate = DateTime.Now;
                    printerChalan.Created = dtDate;
                    printerChalan.Updated = dtDate;

                    db.PrintJobWorkReceiveds.Add(printerChalan);
                    db.SaveChanges();
                    int scope_id = printerChalan.Id;
                    transaction.Commit();
                    return Json(Convert.ToString(scope_id));
                }
                catch
                {
                    transaction.Rollback();
                    ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2), "Id", "VendorName", printerChalan.VendorId);
                    ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
                }
            }
            return Json("0");
        }

        // GET: PrinterChalans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrintJobWorkReceived printerChalan = db.PrintJobWorkReceiveds.Find(id);
            if (printerChalan == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", printerChalan.VendorId);
            ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2), "Id", "VendorName", printerChalan.VendorId);
            return View(printerChalan);
        }

        // POST: PrinterChalans/Edit/5        
        [HttpPost]
        public JsonResult Edit(PrintJobWorkReceived printerChalan)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var objPurchaseDetails in printerChalan.PrintJobWorkReceivedDetails)
                    {
                        if (objPurchaseDetails.Id == 0)
                        {
                            db.Entry(objPurchaseDetails).State = EntityState.Added;
                            db.SaveChanges();
                        }
                    }

                    while (printerChalan.PrintJobWorkReceivedDetails.Where(x => x.Id == 0).Count() > 0)
                        printerChalan.PrintJobWorkReceivedDetails.Remove(printerChalan.PrintJobWorkReceivedDetails.Where(x => x.Id == 0).ToList()[0]);

                    DateTime dtDate = DateTime.Now;
                    printerChalan.Updated = dtDate;
                    db.Entry(printerChalan).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Commit();
                    return Json(Convert.ToString(printerChalan.Id));
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2), "Id", "VendorName", printerChalan.VendorId);
                    ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
                }
            }
            return Json("0");
        }

        // GET: PrinterChalans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrintJobWorkReceived printerChalan = db.PrintJobWorkReceiveds.Find(id);
            db.PrintJobWorkReceiveds.Remove(printerChalan);
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
                    PrintJobWorkReceivedDetail printerChalanDetail = (PrintJobWorkReceivedDetail)db.PrintJobWorkReceivedDetails.Where(x => x.Id == id).FirstOrDefault();
                    db.PrintJobWorkReceivedDetails.Remove(printerChalanDetail);
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
