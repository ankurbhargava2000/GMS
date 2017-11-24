//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vendor
    {
        public Vendor()
        {
            this.InvoiceMasters = new HashSet<InvoiceMaster>();
            this.PrinterChalans = new HashSet<PrinterChalan>();
            this.PrintJobWorkReceiveds = new HashSet<PrintJobWorkReceived>();
            this.PurchaseOrders = new HashSet<PurchaseOrder>();
            this.TailorChalans = new HashSet<TailorChalan>();
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int Id { get; set; }
        public Nullable<int> VendorTypeId { get; set; }
        public string VendorName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string email { get; set; }
        public string pan_number { get; set; }
        public string gst_number { get; set; }
        public string mobile { get; set; }
    
        public virtual ICollection<InvoiceMaster> InvoiceMasters { get; set; }
        public virtual ICollection<PrinterChalan> PrinterChalans { get; set; }
        public virtual ICollection<PrintJobWorkReceived> PrintJobWorkReceiveds { get; set; }
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual ICollection<TailorChalan> TailorChalans { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual VendorType VendorType { get; set; }
    }
}
