using StockManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace StockManager.Controllers
{
    [CheckAuth]
    public class TailorChalanSendController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: TailorChalan
        public ActionResult Index(int? page)
        {
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var tailorChalans = db.TailorChalanSends
                .Where(x => x.financial_year == year_id && x.CompanyId == CompanyId)
                .Include(p => p.Vendor)                
                .Include(p => p.TailorChalanSendDetails).OrderBy(x => x.ChalanDate);
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tailorChalans.ToPagedList(pageNumber, pageSize));
        }

        // GET: TailorChalan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TailorChalanSend tailorChalan = db.TailorChalanSends.Find(id);
            if (tailorChalan == null)
            {
                return HttpNotFound();
            }
            return View(tailorChalan);
        }

        // GET: TailorChalan/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true), "Id", "ProductName");
            ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3), "Id", "VendorName");
            
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");

            var last = db.TailorChalanSends.OrderByDescending(o => o.ChalanNo).FirstOrDefault();

            if (last == null)
            {
                ViewBag.ChalanNo = 1;
            }
            else
            {
                ViewBag.ChalanNo = last.ChalanNo + 1;
            }
            return View();
        }

        // POST: TailorChalan/Create       
        [HttpPost]
        public JsonResult Create(TailorChalanSend tailorChalan)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);
                    var CompanyId = Convert.ToInt32(Session["CompanyID"]);
                    var creaded_by = Convert.ToInt32(Session["UserID"]);
                    DateTime dtDate = DateTime.Now;
                    tailorChalan.Created = dtDate;
                    tailorChalan.Updated = dtDate;
                    tailorChalan.created_by = creaded_by;
                    tailorChalan.financial_year = year_id;
                    tailorChalan.CompanyId = CompanyId;

                    db.TailorChalanSends.Add(tailorChalan);
                    db.SaveChanges();
                    int scope_id = tailorChalan.Id;
                    transaction.Commit();
                    return Json(Convert.ToString(scope_id));
                }
                catch
                {
                    transaction.Rollback();
                    ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3), "Id", "VendorName", tailorChalan.VendorId);
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true), "Id", "ProductName");
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
            TailorChalanSend tailorChalan = db.TailorChalanSends.Find(id);
            if (tailorChalan == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true), "Id", "ProductName", tailorChalan.VendorId);
            ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3), "Id", "VendorName", tailorChalan.VendorId);
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");
            return View(tailorChalan);
        }

        // POST: TailorChalan/Edit/5        
        [HttpPost]
        public JsonResult Edit(TailorChalanSend tailorChalan)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {                    

                    foreach (var objTailorDetails in tailorChalan.TailorChalanSendDetails)
                    {
                        if (objTailorDetails.Id == 0)
                        {
                            db.Entry(objTailorDetails).State = EntityState.Added;
                            db.SaveChanges();
                        }
                    }

                    while (tailorChalan.TailorChalanSendDetails.Where(x => x.Id == 0).Count() > 0)
                        tailorChalan.TailorChalanSendDetails.Remove(tailorChalan.TailorChalanSendDetails.Where(x => x.Id == 0).ToList()[0]);

                    
                    DateTime dtDate = DateTime.Now;
                    tailorChalan.Updated = dtDate;
                    db.Entry(tailorChalan).State = EntityState.Modified;
                    db.SaveChanges();

                    transaction.Commit();
                    return Json(Convert.ToString(tailorChalan.Id));
                }
                catch
                {
                    transaction.Rollback();
                    ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3), "Id", "VendorName", tailorChalan.VendorId);
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true), "Id", "ProductName");
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
            TailorChalanSend tailorChalan = db.TailorChalanSends.Find(id);
            db.TailorChalanSends.Remove(tailorChalan);
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
                    TailorChalanSendDetail tailorChalanDetail = (TailorChalanSendDetail)db.TailorChalanSendDetails.Where(x => x.Id == id).FirstOrDefault();
                    db.TailorChalanSendDetails.Remove(tailorChalanDetail);
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
