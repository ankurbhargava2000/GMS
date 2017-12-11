using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManager.Models
{
    public class PrinterModel
    {
        public int No { get; set; }
        public List<PrinterItem> Items = new List<PrinterItem>();
        public Company Company { get; set; }
        public string Customer { get; set; }
        public DateTime CreatedOn { get; set; }

        public bool ShowTotal = false;
        public double Gross { get; set; }
        public double Discount { get; set; }
        public double NetTotal { get; set; }

    }

    public class PrinterItem
    {
        public int No { get; set; }
        public string Particular { get; set; }
        public int Pcs { get; set; }
        public double Rate { get; set; }
    }

}