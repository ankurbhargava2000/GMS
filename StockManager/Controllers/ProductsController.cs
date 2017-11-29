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
    public class ProductsController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: Products
        public ActionResult Index(int? page)
        {
            var tenant_id = Convert.ToInt32(Session["TenantID"]);
            var products = db.Products.Include(p => p.ProductType)
                .Where(x => x.tenant_id == tenant_id)
                .ToList();
            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductTypeId,ProductName,Description,IsActive,tenant_id")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductTypeId,ProductName,Description,IsActive,tenant_id")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateOpeningStock()
        {
            var model = new OpeningStockVM();

            model.Products = db.Products
                                .Where(x => x.IsActive == true)
                                .ToList();

            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            model.FinancialYear = db.FinancialYears
                .Where(x => x.Id == year_id)
                .FirstOrDefault();

            model.Transactions = db.Transactions
                .Where(x => x.financial_year == year_id)
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOpeningStock(List<Transaction> Transactions)
        {

            var tenant_id = Convert.ToInt32(Session["TenantID"]);
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);

            var t = db.Transactions
            .Where(x => x.financial_year == year_id)
            .ToList();

            var vendor = db.Vendors.FirstOrDefault();

            foreach (var item in Transactions)
            {

                var product = t
                .Where(x => x.ProductId == item.ProductId)
                .Where(x => x.financial_year == year_id)
                .FirstOrDefault();

                if (product == null)
                {
                    db.Transactions.Add(new Transaction()
                    {
                        ProductId = item.ProductId,
                        VendorId = vendor.Id,
                        Type = "opening stock",
                        financial_year = year_id,
                        TenantId = tenant_id,
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    });
                }
                else
                {
                    product.Quantity = item.Quantity;
                    product.Updated = DateTime.Now;
                    db.Entry(product).State = EntityState.Modified;
                }

            }

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
