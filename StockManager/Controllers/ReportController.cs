using iTextSharp.text;
using iTextSharp.text.pdf;
using StockManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StockManager.Controllers
{
    [CheckAuth]
    public class ReportController : Controller
    {
        private StockManagerEntities db = new StockManagerEntities();
        public ActionResult VendorWiseStock()
        {
            var company = Convert.ToInt32(Session["CompanyID"]);
            var fYear = Convert.ToInt32(Session["FinancialYearID"]);
            FinancialYear f = db.FinancialYears.Where(x => x.Id == fYear).FirstOrDefault();
            var result = db.USP_VendorWiseStock(17,company, f.StartDate, f.EndDate).ToList();
            return View(result);
        }

        public ActionResult ProductWiseStock()
        {
            var company = Convert.ToInt32(Session["CompanyID"]);
            var fYear = Convert.ToInt32(Session["FinancialYearID"]);
            FinancialYear f = db.FinancialYears.Where(x => x.Id == fYear).FirstOrDefault();
            var result = db.USP_ProductWiseStock(company,fYear, f.StartDate, f.EndDate).ToList();
            return View(result);
        }

        public ActionResult StockLedger(int productId)
        {
            var company = Convert.ToInt32(Session["CompanyID"]);
            var fYear = Convert.ToInt32(Session["FinancialYearID"]);

            var result = db.USP_StockLedger(productId,company, fYear).ToList();
            ViewBag.Product = db.Products.Where(x => x.Id == productId).FirstOrDefault().ProductName;
            return View(result);
        }

        public FileResult PrintProduct()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("Product Wise Stock Report" + dTime.ToString("yyyy-MM-dd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(4);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table

            //file will created in this path
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF 
            doc.Add(ProductReport(tableLayout));

            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", strPDFFileName);
        }

        public FileResult PrintVendor()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created 
            string strPDFFileName = string.Format("Vendor Wise Stock Report" + dTime.ToString("yyyy-MM-dd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns
            PdfPTable tableLayout = new PdfPTable(5);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table

            //file will created in this path
            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF 
            doc.Add(VendorReport(tableLayout));

            // Closing the document
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable ProductReport(PdfPTable tableLayout)
        {
            float[] headers = { 50, 24, 45, 35 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top
            var company = Convert.ToInt32(Session["CompanyID"]);
            var fYear = Convert.ToInt32(Session["FinancialYearID"]);
            FinancialYear f = db.FinancialYears.Where(x => x.Id == fYear).FirstOrDefault();
            List<USP_ProductWiseStock_Result> product = db.USP_ProductWiseStock(company,fYear,f.StartDate,f.EndDate).ToList<USP_ProductWiseStock_Result>();
            tableLayout.AddCell(new PdfPCell(new Phrase("Product Wise Stock Report", new Font(Font.HELVETICA, 8, 1, new iTextSharp.text.Color(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });

            ////Add header
            AddCellToHeader(tableLayout, "Product Name");
            AddCellToHeader(tableLayout, "Total Purchase");
            AddCellToHeader(tableLayout, "Total Received");
            AddCellToHeader(tableLayout, "Total Quantity Given For Printing");
            ////Add body
            foreach (var emp in product)
            {
                AddCellToBody(tableLayout, emp.ProductName);
                AddCellToBody(tableLayout, Convert.ToString(emp.TotalPurchase));
                AddCellToBody(tableLayout, Convert.ToString(emp.TotalReceived));
                AddCellToBody(tableLayout, Convert.ToString(emp.TotalProductGivenForPrinting));
            }
            return tableLayout;
        }

        protected PdfPTable VendorReport(PdfPTable tableLayout)
        {
            float[] headers = { 50, 24, 45, 35, 50 };  //Header Widths
            tableLayout.SetWidths(headers);        //Set the pdf headers
            tableLayout.WidthPercentage = 100;       //Set the PDF File witdh percentage
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top
            var company = Convert.ToInt32(Session["CompanyID"]);
            var fYear = Convert.ToInt32(Session["FinancialYearID"]);
            FinancialYear f = db.FinancialYears.Where(x => x.Id == fYear).FirstOrDefault();
            List<USP_VendorWiseStock_Result> product = db.USP_VendorWiseStock(17, company,f.StartDate,f.EndDate).ToList<USP_VendorWiseStock_Result>();
            tableLayout.AddCell(new PdfPCell(new Phrase("Vendor Wise Stock Report", new Font(Font.HELVETICA, 8, 1, new iTextSharp.text.Color(0, 0, 0)))) { Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER });

            ////Add header
            AddCellToHeader(tableLayout, "Vendor Name");
            AddCellToHeader(tableLayout, "Given For Printing");
            AddCellToHeader(tableLayout, "Received After Printing");
            AddCellToHeader(tableLayout, "Total Quantity Left");
            AddCellToHeader(tableLayout, "Total Shrinkage");
            ////Add body
            foreach (var emp in product)
            {
                //AddCellToBody(tableLayout, emp.VendorName);
                //AddCellToBody(tableLayout, Convert.ToString(emp.GivenForPrinting));
                //AddCellToBody(tableLayout, Convert.ToString(emp.ReceivedAfterPrinting));
                //AddCellToBody(tableLayout, Convert.ToString(emp.TotalNetQuantity));
                //AddCellToBody(tableLayout, Convert.ToString(emp.TotalShrinkage));
            }
            return tableLayout;
        }

        // Method to add single cell to the Header
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.HELVETICA, 8, 1, iTextSharp.text.Color.YELLOW))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.Color(128, 0, 0) });
        }

        // Method to add single cell to the body
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.HELVETICA, 8, 1, iTextSharp.text.Color.BLACK))) { HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.Color(255, 255, 255) });
        }
    }
}
