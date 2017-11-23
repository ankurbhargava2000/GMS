using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockManager.Models
{
    public class LoginVM
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public bool remember_me { get; set; }
        public string return_url { get; set; }
    }
}