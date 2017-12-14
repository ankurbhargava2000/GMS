using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManager.Models
{
    [MetadataType(typeof(CustomerMetaData))]
    public partial class Customer { }

    class CustomerMetaData
    {
        
        [Required(ErrorMessage = "This field is required")]
        public string CustomerName { get; set; }

    }

}