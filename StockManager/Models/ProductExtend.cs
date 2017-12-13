using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManager.Models
{
    [MetadataType(typeof(ProductMetaData))]
    public partial class Product { }

    class ProductMetaData
    {
        [Required(ErrorMessage = "This field is required")]
        public Nullable<int> ProductTypeId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public Nullable<int> Unit { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public Nullable<decimal> SellingPrice { get; set; }

    }

}