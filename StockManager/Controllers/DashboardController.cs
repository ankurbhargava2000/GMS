using StockManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StockManager.Controllers
{
    public class DashboardController : Controller
    {

        private StockManagerEntities db = new StockManagerEntities();

        [CheckAuth]
        public ActionResult Index()
        {
            var vm = new DashboardVM();

            vm.totalPurchases = db.PurchaseOrders.ToList().Sum(x => x.NetAmount);
            vm.totalSales = db.InvoiceMasters.ToList().Sum(x => x.net_amount);
            vm.totalExpenses = 0;

            var companyID = Convert.ToInt32(Session["CompanyID"]);
            var yearID = Convert.ToInt32(Session["FinancialYearID"]);

            // Top 10 Selling Products for current FiscalYear and Company
            vm.topProducts = db.InvoiceDetails
                .Where(x => x.InvoiceMaster.financial_year == yearID)
                .Where(x => x.InvoiceMaster.CompanyId == companyID)
                .GroupBy(d => d.product_id)
                .Select(tp => new TopModel()
                {
                    Name = tp.FirstOrDefault().Product.ProductName,
                    Amount = tp.Sum(x => (x.sale_rate * x.quantity) - x.discount),
                    Count = tp.Count()
                })
                .OrderByDescending(x => x.Amount)
                .Take(10)
                .ToList();

            // Top 10 Customers for current FiscalYear and Company
            vm.topCustomers = db.InvoiceMasters
                .Where(x => x.financial_year == yearID)
                .Where(x => x.CompanyId == companyID)
                .GroupBy(d => d.customer_id)
                .Select(tp => new TopModel()
                {
                    Name = tp.FirstOrDefault().Customer.CustomerName,
                    Amount = tp.Sum(x => x.net_amount),
                    Count = tp.Count()
                })
                .OrderByDescending(x => x.Amount)
                .Take(10)
                .ToList();

            // Top 10 Vendors for current FiscalYear and Company
            vm.topVendors = db.PurchaseOrders
                .Where(x => x.financial_year == yearID)
                .Where(x => x.CompanyId == companyID)
                .GroupBy(d => d.VendorId)
                .Select(tp => new TopModel()
                {
                    Name = tp.FirstOrDefault().Vendor.VendorName,
                    Amount = (tp.Sum(x => x.NetAmount.HasValue ? (double)x.NetAmount.Value : 0)),
                    Count = tp.Count()
                })
                .OrderByDescending(x => x.Amount)
                .Take(10)
                .ToList();

            // Latest Purchase for current FiscalYear and Company
            vm.latestPurchase = db.PurchaseOrders
                .Where(x => x.financial_year == yearID)
                .Where(x => x.CompanyId == companyID)
                .OrderByDescending(x => x.Id)
                .Take(10)
                .ToList();

            return View(vm);

        }
    }
}