using StockManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManager.Controllers
{
    public class ReportController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();
        public ActionResult VendorWiseStock()
        {
            var result = db.USP_VendorWiseStock().ToList();
            return View(result);
        }

        public ActionResult ProductWiseStock()
        {
            var result = db.USP_ProductWiseStock().ToList();
            return View(result);
        }
    }
}
