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
    public class AccGroupController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: accGroup
        public ActionResult Index()
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var accGroup = db.acc_group
                .Where(x => x.company_id == CompanyId && x.is_base_group==false)
                .ToList();
            return View(accGroup);
        }

        // GET: accGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            acc_group vendor = db.acc_group.Where(x => x.company_id == companyId && x.id == id && x.is_base_group == false).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: accGroup/Create
        public ActionResult Create()
        {
            ViewBag.base_group = new SelectList(db.acc_group.Where(x => x.is_base_group == true), "id", "name");
            return View();
        }

        // POST: accGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,description,base_group")] acc_group vendor)
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            if (ModelState.IsValid)
            {
                db.acc_group.Add(vendor);
                vendor.company_id = CompanyId;
                vendor.is_base_group = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.base_group = new SelectList(db.acc_group.Where(x => x.is_base_group == true), "id", "name");
            return View(vendor);
        }

        // GET: accGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            acc_group vendor = db.acc_group.Where(x => x.company_id == companyId && x.id == id && x.is_base_group == false).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            ViewBag.base_group = new SelectList(db.acc_group.Where(x => x.is_base_group == true && x.company_id == null), "id", "name", vendor.base_group);
            return View(vendor);
        }

        // POST: accGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,description,base_group")] acc_group vendor)
        {
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                vendor.company_id = companyId;
                vendor.is_base_group = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.base_group = new SelectList(db.acc_group.Where(x => x.is_base_group == true && x.company_id == null), "id", "name");
            return View(vendor);
        }

        // GET: accGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            acc_group vendor = db.acc_group.Where(x => x.company_id == companyId && x.id == id && x.is_base_group == false).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            db.acc_group.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: accGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var companyId = Convert.ToInt32(Session["CompanyID"]);
            acc_group vendor = db.acc_group.Where(x => x.company_id == companyId && x.id == id && x.is_base_group == false).FirstOrDefault();
            db.acc_group.Remove(vendor);
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
