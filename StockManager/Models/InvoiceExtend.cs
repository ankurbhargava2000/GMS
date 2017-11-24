using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManager.Models
{
    [MetadataType(typeof(InvoiceMetaData))]
    public partial class InvoiceMaster
    {
    }

    public class InvoiceMetaData
    {
        [Remote("IsIdExists", "Invoices", ErrorMessage = "Invoice already exist with that #")]
        public int invoice_no { get; set; }
    }

}