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
    
    public partial class TailorChalanDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TailorChalanDetail()
        {
            this.TailorMaterialDetails = new HashSet<TailorMaterialDetail>();
        }
    
        public int Id { get; set; }
        public int ChalanId { get; set; }
        public int ProductId { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> LaborCost { get; set; }
        public string Description { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual TailorChalan TailorChalan { get; set; }
        public virtual TailorChalan TailorChalan1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TailorMaterialDetail> TailorMaterialDetails { get; set; }
    }
}
