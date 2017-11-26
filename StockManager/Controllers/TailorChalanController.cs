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
    public class TailorChalanController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: TailorChalan
        public ActionResult Index(int? id, int? page)
        {
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            var tenant_id = Convert.ToInt32(Session["TenantID"]);
            var tailorChalans = db.TailorChalans
                .Where(x => x.financial_year == year_id && x.tenant_id == tenant_id)
                .Include(p => p.Vendor)                
                .Include(p => p.TailorChalanDetails).Include(p => p.TailorChalanDetails1).OrderBy(x => x.ChalanDate);
            ViewBag.Send = id;
            int pageSize = 3;
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
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == (id == 1 ? 1 : 0) && x.IsActive == true), "Id", "ProductName");
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName");
            ViewBag.GivenToTailor = id;
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");
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
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.IsActive == true), "Id", "ProductName");
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
            TailorChalan tailorChalan = db.TailorChalans.Find(id);
            if (tailorChalan == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.IsActive == true), "Id", "ProductName", tailorChalan.VendorId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", tailorChalan.VendorId);
            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");
            return View(tailorChalan);
        }

        // POST: TailorChalan/Edit/5        
        [HttpPost]
        public JsonResult Edit(TailorChalan tailorChalan)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var objTailorDetails in tailorChalan.TailorChalanDetails)
                    {
                        db.TailorMaterialDetails.RemoveRange(db.TailorMaterialDetails.Where(x => x.TailorChalanDetailsId == objTailorDetails.Id));
                        db.SaveChanges();
                        foreach (var objTailorMaterial in objTailorDetails.TailorMaterialDetails)
                        {
                            objTailorMaterial.TailorChalanDetailsId = objTailorDetails.Id;
                            db.Entry(objTailorMaterial).State = EntityState.Added;
                            db.SaveChanges();
                        }
                    }

                    foreach (var objTailorDetails in tailorChalan.TailorChalanDetails)
                    {
                        if (objTailorDetails.Id == 0)
                        {
                            db.Entry(objTailorDetails).State = EntityState.Added;
                            db.SaveChanges();
                        }
                    }

                    while (tailorChalan.TailorChalanDetails.Where(x => x.Id == 0).Count() > 0)
                        tailorChalan.TailorChalanDetails.Remove(tailorChalan.TailorChalanDetails.Where(x => x.Id == 0).ToList()[0]);

                    foreach (var x in tailorChalan.TailorChalanDetails)
                    {
                        x.TailorMaterialDetails = null;
                    }

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
                    ViewBag.VendorId = new SelectList(db.Vendors, "Id", "VendorName", tailorChalan.VendorId);
                    ViewBag.ProductId = new SelectList(db.Products.Where(x => x.IsActive == true), "Id", "ProductName");
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
