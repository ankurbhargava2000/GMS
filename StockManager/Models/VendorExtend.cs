using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManager.Models
{
    [MetadataType(typeof(VendorMetaData))]
    public partial class Vendor { }

    class VendorMetaData
    {
        [Required(ErrorMessage = "This field is required")]
        public Nullable<int> VendorTypeId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string VendorName { get; set; }
        
    }

}