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
    public class CompanyController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: Companies
        public ActionResult Index()
        {
            var TenantId = Convert.ToInt32(Session["TenantID"]);
            var Companies = db.Companies
                .Where(x => x.TenantId == TenantId)
                .ToList();
            return View(Companies);
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company vendor = db.Companies.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            //ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorType1");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Phone,Address,City,State,Country,Mobile,email,PanNo,GSTNo,TenantId")] Company vendor)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(vendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorType1", vendor.VendorTypeId);
            return View(vendor);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company vendor = db.Companies.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            //ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorType1", vendor.VendorTypeId);
            return View(vendor);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Phone,Address,City,State,Country,Mobile,email,PanNo,GSTNo,TenantId")] Company vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.VendorTypeId = new SelectList(db.VendorTypes, "Id", "VendorType1", vendor.VendorTypeId);
            return View(vendor);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company vendor = db.Companies.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            db.Companies.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company vendor = db.Companies.Find(id);
            db.Companies.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddUser(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var company_id = id;

            var company = db.Companies.Find(company_id);
            
            if (company == null)
                return HttpNotFound("Company Not Found");

            var company_user = db.UserCompanies
                .Where(x => x.CompanyId == company_id)
                .ToList();

            var default_user = company_user.Where(x => x.is_default == true).FirstOrDefault();

            var users = db.Users.ToList();
            var addUser = new List<AddUser>();

            foreach (var item in users)
            {
                var nu = new AddUser()
                {
                    UserId = item.UserId,
                    UserName = item.UserName,
                    IsDefault = false,
                    AddToCompany = false
                };

                if (default_user != null && default_user.UserId == item.UserId)
                {
                    nu.IsDefault = true;
                }

                if (company_user.Any(x => x.UserId == item.UserId))
                {
                    nu.AddToCompany = true;
                }

                addUser.Add(nu);

            }

            ViewBag.Company = company;

            return View(addUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(List<AddUser> users, int? defaultFor, int id)
        {
            try
            {
                var user_companies = db.UserCompanies.Where(x => x.CompanyId == id).ToList();

                var addToCompany = users
                    .Where(x => x.AddToCompany == true && !user_companies.Any(c => c.UserId == x.UserId))
                    .ToList();

                user_companies
                    .Where(x => users.Any(r => r.UserId == x.UserId && r.AddToCompany == false))
                    .ToList().ForEach(x => db.Entry(x).State = EntityState.Deleted);

                addToCompany.ForEach(x => db.UserCompanies.Add(new UserCompany { CompanyId = id, UserId = x.UserId }));
                
                db.SaveChanges();

                user_companies = db.UserCompanies.Where(x => x.CompanyId == id)
                    .ToList();

                foreach (var item in user_companies)
                {
                    if (item.UserId == defaultFor)
                    {
                        item.is_default = true;
                    }
                    else
                    {
                        item.is_default = false;
                    }

                    db.Entry(item).State = EntityState.Modified;

                }
                
                db.SaveChanges();

                return RedirectToAction("Index");

            }
            catch ( Exception e)
            {
                
            }

            return RedirectToAction("AddUser", new { id = id });

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
