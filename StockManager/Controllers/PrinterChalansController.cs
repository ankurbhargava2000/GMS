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
    public class PrinterChalansController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: PrinterChalans
        public ActionResult Index( int? page)
        {
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            var tenant_id = Convert.ToInt32(Session["TenantID"]);
            var printerChalans = db.PrinterChalans
                .Where(x => x.financial_year == year_id && x.tenant_id == tenant_id)
                .Include(p => p.Vendor).Include(p => p.PrinterChalanDetails).OrderBy(x => x.ChalanDate);
            
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
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName");
            ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2), "Id", "VendorName");
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");

            var last = db.PrinterChalans.OrderByDescending(o => o.chalan_number).FirstOrDefault();

            if (last == null)
            {
                ViewBag.chalan_number = 1;
            }
            else
            {
                ViewBag.chalan_number = last.chalan_number + 1;
            }

            return View();
        }

        // POST: PrinterChalans/Create       
        [HttpPost]
        public JsonResult Create(PrinterChalan printerChalan)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);
                    var tenant_id = Convert.ToInt32(Session["TenantID"]);
                    var creaded_by = Convert.ToInt32(Session["UserID"]);
                    DateTime dtDate = DateTime.Now;
                    printerChalan.Created = dtDate;
                    printerChalan.Updated = dtDate;
                    printerChalan.created_by = creaded_by;
                    printerChalan.financial_year = year_id;
                    printerChalan.tenant_id = tenant_id;

                    db.PrinterChalans.Add(printerChalan);
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
            PrinterChalan printerChalan = db.PrinterChalans.Find(id);
            if (printerChalan == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "ProductName", printerChalan.VendorId);
            ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 2), "Id", "VendorName", printerChalan.VendorId);
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");
            return View(printerChalan);
        }

        // POST: PrinterChalans/Edit/5        
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
            PrinterChalan printerChalan = db.PrinterChalans.Find(id);
            db.PrinterChalans.Remove(printerChalan);
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
                    PrinterChalanDetail printerChalanDetail = (PrinterChalanDetail)db.PrinterChalanDetails.Where(x => x.Id == id).FirstOrDefault();
                    db.PrinterChalanDetails.Remove(printerChalanDetail);
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
