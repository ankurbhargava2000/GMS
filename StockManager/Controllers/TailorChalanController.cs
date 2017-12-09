using StockManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Validation;

namespace StockManager.Controllers
{
    [CheckAuth]
    public class TailorChalanController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: TailorChalan
        public ActionResult Index(int? id, int? page)
        {
            //var tailorChalans = db.TailorChalans.Include(p => p.Vendor).Where(p => p.IsGivenToTailor == (id == 1 ? true : false)).Include(p => p.TailorChalanDetails).Include(p => p.TailorChalanDetails1).OrderBy(x => x.ChalanDate);
            var tailorChalans = db.TailorChalans.Include(p => p.Vendor).Include(p => p.TailorChalanDetails).Include(p => p.TailorChalanDetails1).OrderBy(x => x.ChalanDate);
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
            
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true), "Id", "ProductName");
            
            ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3), "Id", "VendorName");            
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
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);
                    var CompanyId = Convert.ToInt32(Session["CompanyID"]);
                    var creaded_by = Convert.ToInt32(Session["UserID"]);
                    DateTime dtDate = DateTime.Now;
                    tailorChalan.Created = dtDate;
                    tailorChalan.Updated = dtDate;
                    tailorChalan.created_by = creaded_by;
                    tailorChalan.financial_year = year_id;
                    tailorChalan.CompanyId = CompanyId;

                    db.TailorChalans.Add(tailorChalan);
                    db.SaveChanges();
                    int scope_id = tailorChalan.Id;
                    transaction.Commit();
                    return Json(Convert.ToString(scope_id));
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {                       
                        foreach (var ve in eve.ValidationErrors)
                        {
                        }
                    }
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
            TailorChalan tailorChalan = db.TailorChalans.Find(id);
            if (tailorChalan == null)
            {
                return HttpNotFound();
            }            
            ViewBag.ProductId = new SelectList(db.Products.Where(x => x.ProductTypeId == 1 && x.IsActive == true), "Id", "ProductName");
            ViewBag.VendorId = new SelectList(db.Vendors.Where(x => x.VendorTypeId == 3), "Id", "VendorName");
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
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);
                    var CompanyId = Convert.ToInt32(Session["CompanyID"]);
                    var creaded_by = Convert.ToInt32(Session["UserID"]); 
                    DateTime dtDate = DateTime.Now;
                    tailorChalan.Updated = dtDate;
                    tailorChalan.created_by = creaded_by;
                    tailorChalan.financial_year = year_id;
                    tailorChalan.CompanyId = CompanyId;

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
