using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using StockManager.Models;
using Rotativa;

namespace StockManager.Controllers
{
    [CheckAuth]
    public class InvoicesController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: InvoiceMasters
        public ActionResult Index()
        {
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var invoiceMasters = db.InvoiceMasters
                .Where(x => x.financial_year == year_id && x.CompanyId == CompanyId)
                .Include(i => i.FinancialYear)
                .Include(i => i.User)
                .Include(i => i.Customer)
                .Include(i => i.Company)
                .ToList();

            return View(invoiceMasters);
        }

        // GET: InvoiceMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InvoiceMaster invoiceMaster = db.InvoiceMasters.Where(x => x.invoice_no == id).FirstOrDefault();
            if (invoiceMaster == null)
            {
                return HttpNotFound();
            }
            return View(invoiceMaster);
        }

        // GET: InvoiceMasters/Create
        public ActionResult Create()
        {
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            ViewBag.customer_id = new SelectList(db.Customers.Where(x => x.CompanyId == CompanyId), "Id", "CustomerName");
            ViewBag.product_id = db.Products.Where(x => x.CompanyId == CompanyId).ToList();

            var last = db.InvoiceMasters.OrderByDescending(o => o.invoice_no).FirstOrDefault();

            if (last == null)
            {
                ViewBag.invoice_no = 1;
            }
            else
            {
                ViewBag.invoice_no = last.invoice_no + 1;
            }



            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceMaster invoice)
        {

            if (ModelState.IsValid)
            {
                invoice.created_at = DateTime.Now;
                db.InvoiceMasters.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            ViewBag.customer_id = new SelectList(db.Customers.Where(x => x.CompanyId == CompanyId), "Id", "CustomerName");
            ViewBag.product_id = db.Products.Where(x => x.CompanyId == CompanyId).ToList();

            return View();

        }

        // GET: InvoiceMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InvoiceMaster invoiceMaster = db.InvoiceMasters.Where(x => x.invoice_no == id).FirstOrDefault();

            if (invoiceMaster == null)
            {
                return HttpNotFound();
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            ViewBag.invoice_no = invoiceMaster.invoice_no;
            ViewBag.customer_id = new SelectList(db.Customers.Where(x => x.CompanyId == CompanyId), "Id", "CustomerName");
            ViewBag.product_id = db.Products.Where(x => x.CompanyId == CompanyId).ToList();

            var year_id = Session["FinancialYearID"];
            var year = db.FinancialYears.Find(year_id);

            ViewBag.StartYear = year.StartDate.ToString("dd-MMM-yyyy");
            ViewBag.EndYear = year.EndDate.ToString("dd-MMM-yyyy");

            return View(invoiceMaster);

        }

        // POST: InvoiceMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InvoiceMaster invoiceMaster, List<InvoiceDetail> nInvoiceDetails, string deleted_items)
        {

            var ditems = deleted_items.Split(',').Where(s => s != String.Empty).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    invoiceMaster.created_at = DateTime.Now;

                    //db.Entry(invoiceMaster.InvoiceDetails).State = EntityState.Unchanged;
                    db.Entry(invoiceMaster).State = EntityState.Modified;
                    foreach (var item in invoiceMaster.InvoiceDetails)
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }

                    db.SaveChanges();

                    if (nInvoiceDetails != null)
                    {
                        foreach (var item in nInvoiceDetails)
                        {
                            item.created_at = DateTime.Now;
                            item.invoice_id = invoiceMaster.Id;
                            db.InvoiceDetails.Add(item);
                        }
                        db.SaveChanges();
                    }

                    if (ditems.Count() > 0)
                    {
                        foreach (var item in ditems)
                        {
                            var a = Convert.ToInt32(item);
                            var id = db.InvoiceDetails.Where(x => x.Id == a).FirstOrDefault();
                            if (id != null)
                            {
                                db.InvoiceDetails.Remove(id);
                            }
                        }
                        db.SaveChanges();
                    }

                }
                catch
                {

                }

                return RedirectToAction("Index");
            }
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            ViewBag.invoice_no = invoiceMaster.invoice_no;
            ViewBag.customer_id = new SelectList(db.Customers.Where(x => x.CompanyId == CompanyId), "Id", "CustomerName");
            ViewBag.product_id = db.Products.Where(x => x.CompanyId == CompanyId).ToList();

            return View(invoiceMaster);

        }

        // GET: InvoiceMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InvoiceMaster invoiceMaster = db.InvoiceMasters.Where(x => x.invoice_no == id).FirstOrDefault();

            if (invoiceMaster == null)
            {
                return HttpNotFound();
            }
            return View(invoiceMaster);
        }

        // POST: InvoiceMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvoiceMaster invoiceMaster = db.InvoiceMasters.Where(x => x.invoice_no == id).FirstOrDefault();
            db.InvoiceMasters.Remove(invoiceMaster);
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
        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        public ActionResult Print()
        {
            var year_id = Convert.ToInt32(Session["FinancialYearID"]);
            var CompanyId = Convert.ToInt32(Session["CompanyID"]);
            var invoiceMasters = db.InvoiceMasters
                .Where(x => x.financial_year == year_id && x.CompanyId == CompanyId)
                .Include(i => i.FinancialYear)
                .Include(i => i.User)
                .Include(i => i.Customer)
                .Include(i => i.Company)
                .ToList();

            return View(invoiceMasters);
        }
        [ActionName("print-single")]
        public ActionResult SinglePrint(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InvoiceMaster invoiceMaster = db.InvoiceMasters.Where(x => x.invoice_no == id).FirstOrDefault();
            if (invoiceMaster == null)
            {
                return HttpNotFound();
            }

            return new ViewAsPdf("SinglePrint", invoiceMaster);

        }

        public JsonResult IsIdExists(int invoice_no)
        {
            return Json(!db.InvoiceMasters.Any(x => x.invoice_no == invoice_no), JsonRequestBehavior.AllowGet);
        }


    }
}
