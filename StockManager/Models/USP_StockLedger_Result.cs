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
    
    public partial class USP_StockLedger_Result
    {
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Particular { get; set; }
        public string Doc_Type { get; set; }
        public Nullable<int> Doc_No { get; set; }
        public int Doc_ID { get; set; }
        public Nullable<decimal> In_QTY { get; set; }
        public decimal Out_QTY { get; set; }
    }
}