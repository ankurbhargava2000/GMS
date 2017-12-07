using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockManager.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User { }
    class UserMetaData
    {
        [MinLength(5)]
        [Required]
        [Remote("IsUserExists", "Users", ErrorMessage = "User Name already in use")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Remote("IsEmailExists", "Users", ErrorMessage = "Email already in use")]
        public string Email { get; set; }

        [MinLength(6)]
        [Required]
        public string Password { get; set; }

    }

    namespace LogedUser
    {
        public static class LogedUser
        {
            public static User user { get; set; }
        }

    }

}