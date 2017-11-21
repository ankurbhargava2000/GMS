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
    
    public partial class InvoiceMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvoiceMaster()
        {
            this.InvoiceDetails = new HashSet<InvoiceDetail>();
        }
    
        public int Id { get; set; }
        public string invoice_no { get; set; }
        public Nullable<int> customer_id { get; set; }
        public double gross_amount { get; set; }
        public Nullable<double> discount { get; set; }
        public double net_amount { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<int> financial_year { get; set; }
        public Nullable<int> tenant_id { get; set; }
    
        public virtual FinancialYear FinancialYear { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual User User { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
