using Newtonsoft.Json;
using StockManager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            try
            {
                var vm = new DashboardVM();

                var companyID = Convert.ToInt32(Session["CompanyID"]);
                var yearID = Convert.ToInt32(Session["FinancialYearID"]);
                
                vm.totalPurchases = db.PurchaseOrders
                    .Where(x => x.CompanyId == companyID)
                    .Where(x => x.financial_year == yearID)
                    .ToList().Sum(x => x.NetAmount);

                vm.totalSales = db.InvoiceMasters
                    .Where(x => x.CompanyId == companyID)
                    .Where(x => x.financial_year == yearID)
                    .ToList().Sum(x => x.net_amount);

                vm.totalExpenses = 0;

                vm.topProducts = GetTopProducts(yearID, companyID);
                vm.topCustomers = GetTopCustomers(yearID, companyID);
                vm.topVendors = GetTopVendors(yearID, companyID);
                vm.latestPurchase = GetLatestPurchaseOrders(yearID, companyID);
                vm.SalesAmount = GetSalesAmount(yearID, companyID);
                vm.SalesCount = GetSalesCount(yearID, companyID);

                return View(vm);
            }
            catch(Exception e)
            {
                return HttpNotFound(e.InnerException.Message);
            }            
        }

        private ChartModel GetSalesCount(int? yearID, int? companyID)
        {
            var FiscalYear = db.FinancialYears.Find(yearID);

            if (FiscalYear != null)
            {
                var data = db.InvoiceMasters
                     .Where(x => x.financial_year == yearID)
                    .Where(x => x.CompanyId == companyID)
                    .GroupBy(d => d.created_on)
                    .Select(m => new ChartModel()
                    {
                        Label = m.FirstOrDefault().created_on,
                        Data = m.Count().ToString()
                    })
                    .ToList();

                var start = new DateTime(FiscalYear.StartDate.Ticks);
                var end = new DateTime(FiscalYear.EndDate.Ticks);

                var diff = Enumerable.Range(0, 13).Select(a => start.AddMonths(a))
                           .TakeWhile(a => a <= end)
                           .Select(a => a);

                List<string> labels = new List<string>();
                List<string> points = new List<string>();

                foreach (var item in diff)
                {
                    var t = data.Where(x => x.Label.Value.Month == item.Month).FirstOrDefault();

                    if (t != null)
                    {
                        labels.Add(t.Label.Value.ToString("MMM"));
                        points.Add(t.Data);
                    }
                    else
                    {
                        labels.Add(item.ToString("MMM"));
                        points.Add("0");
                    }

                }

                return new ChartModel()
                {
                    LabelJson = JsonConvert.SerializeObject(labels),
                    PointJson = JsonConvert.SerializeObject(points)
                };

            }

            return new ChartModel()
            {
                LabelJson = "",
                PointJson = ""
            };

        }
        private ChartModel GetSalesAmount(int? yearID, int? companyID)
        {

            var FiscalYear = db.FinancialYears.Find(yearID);

            if (FiscalYear != null)
            {

                var data = db.InvoiceMasters
                     .Where(x => x.financial_year == yearID)
                    .Where(x => x.CompanyId == companyID)
                    .GroupBy(d => d.created_on)
                    .Select(m => new ChartModel()
                    {
                        Label = m.FirstOrDefault().created_on,
                        Data = m.Sum(x => x.net_amount).ToString()
                    })
                    .ToList();

                var start = new DateTime(FiscalYear.StartDate.Ticks);
                var end = new DateTime(FiscalYear.EndDate.Ticks);

                var diff = Enumerable.Range(0, 13).Select(a => start.AddMonths(a))
                           .TakeWhile(a => a <= end)
                           .Select(a => a);

                List<string> labels = new List<string>();
                List<string> points = new List<string>();

                foreach (var item in diff)
                {
                    var t = data.Where(x => x.Label.Value.Month == item.Month).FirstOrDefault();

                    if (t != null)
                    {
                        labels.Add(t.Label.Value.ToString("MMM"));
                        points.Add(t.Data);
                    }
                    else
                    {
                        labels.Add(item.ToString("MMM"));
                        points.Add("0");
                    }

                }

                return new ChartModel()
                {
                    LabelJson = JsonConvert.SerializeObject(labels),
                    PointJson = JsonConvert.SerializeObject(points)
                };

            }

            return new ChartModel()
            {
                LabelJson = "",
                PointJson = ""
            };

        }
        private List<TopModel> GetTopProducts(int? yearID, int? companyID, int count = 10)
        {
            return db.InvoiceDetails
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
                .Take(count)
                .ToList();
        }
        private List<TopModel> GetTopCustomers(int? yearID, int? companyID, int count = 10)
        {
            return db.InvoiceMasters
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
        }
        private List<TopModel> GetTopVendors(int? yearID, int? companyID, int count = 10)
        {
            return db.PurchaseOrders
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
                .Take(count)
                .ToList();
        }
        private List<PurchaseOrder> GetLatestPurchaseOrders(int? yearID, int? companyID, int count = 10)
        {
            return db.PurchaseOrders
                .Where(x => x.financial_year == yearID)
                .Where(x => x.CompanyId == companyID)
                .OrderByDescending(x => x.Id)
                .Take(count)
                .ToList();
        }

    }
}