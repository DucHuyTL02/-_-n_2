using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_bán_hàng__đồ_án_.Models;

namespace Web_bán_hàng__đồ_án_.Controllers
{
    public class AccountController : Controller
    {

        // GET: LoginRegister
       LTWEntities acc = new LTWEntities();
        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu Email đã tồn tại trong bảng Customer
                
                if (acc.Customers.Any(c => c.CustomerEmail== model.Email))
                {
                    ModelState.AddModelError("Emailcus", "Email đã tồn tại.");
                    return View(model);
                }

                // Kiểm tra nếu Username đã tồn tại trong bảng User
                if (acc.Users.Any(u => u.Username == model.UserName))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }

                // Tạo đối tượng User
                var user = new User
                {
                    Username = model.UserName,
                    Password = model.Password,  // Lưu mật khẩu chưa mã hóa (tốt nhất là mã hóa mật khẩu trước khi lưu)
                    UserRole = "Customer"  // Giả sử quyền mặc định là 1
                };

                // Thêm User vào bảng User
                acc.Users.Add(user);
                acc.SaveChanges();  // Lưu vào bảng User

                // Tạo đối tượng Customer
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerPhone = model.CustomerPhone,
                    CustomerEmail = model.CustomEmail,
                    Username = model.UserName  // Liên kết với User thông qua Username
                };

                // Thêm Customer vào bảng Customer
                acc.Customers.Add(customer);
                acc.SaveChanges();  // Lưu vào bảng Customer

                // Đăng ký thành công, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login");
            }

            return View(model);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = acc.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // Xử lý đăng nhập thành công
                Session["User"] = user.Username;
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
            return View();
        }

    }
}