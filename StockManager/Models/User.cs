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
    
    public partial class User
    {
        public User()
        {
            this.InvoiceMasters = new HashSet<InvoiceMaster>();
        }
    
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public int TenantId { get; set; }
        public string password_reset_token { get; set; }
    
        public virtual ICollection<InvoiceMaster> InvoiceMasters { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
