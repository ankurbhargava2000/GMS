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
    
    public partial class USP_VendorWiseStock_Result
    {
        public int Id { get; set; }
        public string VendorName { get; set; }
        public Nullable<decimal> GivenForPrinting { get; set; }
        public Nullable<decimal> ReceivedAfterPrinting { get; set; }
        public decimal TotalNetQuantity { get; set; }
        public decimal TotalShrinkage { get; set; }
    }
}
