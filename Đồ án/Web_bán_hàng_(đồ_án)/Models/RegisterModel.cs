using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_bán_hàng__đồ_án_.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string Namecus { get; set; }

        [Required]
        [EmailAddress]
        public string Emailcus { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}