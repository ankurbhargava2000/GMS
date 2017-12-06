using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockManager.Models
{
    public class AddUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool AddToCompany { get; set; }
        public bool IsDefault { get; set; }
    }
}