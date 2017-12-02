﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class StockManagerEntities : DbContext
    {
        public StockManagerEntities()
            : base("name=StockManagerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<FinancialYear> FinancialYears { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual DbSet<InvoiceMaster> InvoiceMasters { get; set; }
        public virtual DbSet<MeasuringUnit> MeasuringUnits { get; set; }
        public virtual DbSet<PrinterChalan> PrinterChalans { get; set; }
        public virtual DbSet<PrinterChalanDetail> PrinterChalanDetails { get; set; }
        public virtual DbSet<PrintJobWorkReceived> PrintJobWorkReceiveds { get; set; }
        public virtual DbSet<PrintJobWorkReceivedDetail> PrintJobWorkReceivedDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<TailorChalan> TailorChalans { get; set; }
        public virtual DbSet<TailorChalanDetail> TailorChalanDetails { get; set; }
        public virtual DbSet<TailorChalanSend> TailorChalanSends { get; set; }
        public virtual DbSet<TailorChalanSendDetail> TailorChalanSendDetails { get; set; }
        public virtual DbSet<TailorMaterialDetail> TailorMaterialDetails { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<VendorType> VendorTypes { get; set; }
    
        public virtual ObjectResult<USP_ProductWiseStock_Result> USP_ProductWiseStock(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_ProductWiseStock_Result>("USP_ProductWiseStock", userIdParameter);
        }
    
        public virtual ObjectResult<USP_VendorWiseStock_Result> USP_VendorWiseStock()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_VendorWiseStock_Result>("USP_VendorWiseStock");
        }
    }
}
