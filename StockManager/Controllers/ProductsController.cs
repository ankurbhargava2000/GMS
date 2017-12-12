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
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var products = db.Products.Include(p => p.ProductType).Include(p => p.MeasuringUnit)
                .Where(x => x.CompanyId == CompanyId)
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
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            Product product = db.Products.Where(x => x.CompanyId == companyId && x.Id == id).FirstOrDefault();
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
            ViewBag.Unit = new SelectList(db.MeasuringUnits, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            ViewBag.MeasuringUnitId = new SelectList(db.MeasuringUnits, "Id", "Name", product.Unit);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            Product product = db.Products.Where(x => x.CompanyId == companyId && x.Id == id).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            ViewBag.Unit = new SelectList(db.MeasuringUnits, "Id", "Name", product.Unit);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeId = new SelectList(db.ProductTypes, "Id", "ProductType1", product.ProductTypeId);
            ViewBag.Unit = new SelectList(db.MeasuringUnits, "Id", "Name",product.Unit);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            Product product = db.Products.Where(x => x.CompanyId == companyId && x.Id == id).FirstOrDefault();
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
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            Product product = db.Products.Where(x => x.CompanyId == companyId && x.Id == id).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UpdateOpeningStock()
        {
            var model = new OpeningStockVM();
            int companyId = Convert.ToInt32(Session["CompanyID"]);
            model.Products = db.Products
                                .Where(x => x.IsActive == true && x.CompanyId == companyId)
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
            if ( ModelState.IsValid )
            {
                try
                {
                    var CompanyId = Convert.ToInt32(Session["CompanyID"]);
                    var year_id = Convert.ToInt32(Session["FinancialYearID"]);

                    var t = db.Transactions
                    .Where(x => x.financial_year == year_id)
                    .ToList();

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
                                Type = "opening stock",
                                financial_year = year_id,
                                CompanyId = CompanyId,
                                Created = DateTime.Now,
                                Updated = DateTime.Now,
                                Quantity = item.Quantity
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
                catch
                {

                }
            }

            var model = new OpeningStockVM();

            model.Products = db.Products
                                .Where(x => x.IsActive == true)
                                .ToList();

            var yid = Convert.ToInt32(Session["FinancialYearID"]);
            model.FinancialYear = db.FinancialYears
                .Where(x => x.Id == yid)
                .FirstOrDefault();

            model.Transactions = db.Transactions
                .Where(x => x.financial_year == yid)
                .ToList();

            return View(model);

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
