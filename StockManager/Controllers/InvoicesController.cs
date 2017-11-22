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
    public class InvoicesController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();

        // GET: InvoiceMasters
        public ActionResult Index(int? page)
        {
            var invoiceMasters = db.InvoiceMasters.Include(i => i.FinancialYear).Include(i => i.User).Include(i => i.Vendor).Include(i => i.Tenant).OrderBy(x => x.created_on);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(invoiceMasters.ToPagedList(pageNumber, pageSize));
        }

        // GET: InvoiceMasters/Details/5
        public ActionResult Details(string id)
        {
            if (String.IsNullOrEmpty(id))
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
            ViewBag.customer_id = new SelectList(db.Vendors, "Id", "VendorName");
            ViewBag.product_id = db.Products.ToList();

            var last = db.InvoiceMasters.OrderByDescending(o => o.Id).FirstOrDefault();

            ViewBag.invoice_no = (last.Id+1);
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceMaster invoice)
        {

            if (ModelState.IsValid)
            {                
                db.InvoiceMasters.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.Vendors, "Id", "VendorName");
            ViewBag.product_id = db.Products.ToList();

            return View();

        }

        // GET: InvoiceMasters/Edit/5
        public ActionResult Edit(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InvoiceMaster invoiceMaster = db.InvoiceMasters.Where(x => x.invoice_no == id).FirstOrDefault();

            if (invoiceMaster == null)
            {
                return HttpNotFound();
            }

            ViewBag.invoice_no = invoiceMaster.invoice_no;
            ViewBag.customer_id = new SelectList(db.Vendors, "Id", "VendorName");
            ViewBag.product_id = db.Products.ToList();

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
                catch (Exception e)
                {
                    
                }
                
                return RedirectToAction("Index");
            }

            ViewBag.invoice_no = invoiceMaster.invoice_no;
            ViewBag.customer_id = new SelectList(db.Vendors, "Id", "VendorName");
            ViewBag.product_id = db.Products.ToList();

            return View(invoiceMaster);

        }

        // GET: InvoiceMasters/Delete/5
        public ActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
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
        public ActionResult DeleteConfirmed(string id)
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

    }
}
