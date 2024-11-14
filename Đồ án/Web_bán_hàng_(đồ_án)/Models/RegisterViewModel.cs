    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_bán_hàng__đồ_án_.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="Tên Khách Hàng")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Tên Đăng Nhập")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        [Compare("Password",ErrorMessage ="Mật khẩu không khớp nhau")]
        public string ConFirmPassword { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string CustomEmail { get; set; }
        [Required]
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }

    }
}