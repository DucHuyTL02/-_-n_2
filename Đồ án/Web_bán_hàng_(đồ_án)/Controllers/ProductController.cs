using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web_bán_hàng__đồ_án_.Models;
using Web_bán_hàng__đồ_án_.Models.ViewModel;

namespace Web_bán_hàng__đồ_án_.Controllers
{
    public class ProductController : Controller
    {
        LTWEntities csdl = new LTWEntities();


        public ActionResult ProductDetails(int? id, int? quantity, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pro = csdl.Products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }
            var product = csdl.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();
            ProductDetailsVM model = new ProductDetailsVM();
            int pageNumber = page ?? 1;
            int pageSize = model.PageSize;
            model.product = pro;
            model.RelatedProducts = product.OrderByDescending(p => p.ProductID).Take(8).ToPagedList(pageNumber, pageSize);
            model.TopProducts = product.OrderBy(p => p.OrderDetails.Count()).Take(10).ToPagedList(pageNumber, pageSize);
            if (quantity.HasValue)
            {
                model.quantity = quantity.Value;
            }
            return View(model);
        }
    
        public ActionResult ProductList()
        {
            var products = csdl.Products.ToList();
            return View(products);
        }
        public ActionResult Xacnhan()
        {
            return View();
        }

        public ActionResult AddToCart(int id, int quantity = 1)
        {
            // Lấy giỏ hàng từ session
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>(); // Tạo giỏ hàng nếu chưa có
            }

            // Kiểm tra sản phẩm đã có trong giỏ chưa
            var existingItem = cart.FirstOrDefault(x => x.Product.ProductID == id);
            if (existingItem != null)
            {
                // Nếu đã có, tăng số lượng
                existingItem.Quantity += quantity;
            }
            else
            {
                // Nếu chưa có, thêm sản phẩm mới
                var product = csdl.Products.Find(id); // Tìm sản phẩm trong database
                if (product != null)
                {
                    cart.Add(new CartItem
                    {
                        Product = product,
                        Quantity = quantity
                    });
                }
            }

            // Cập nhật lại session
            Session["Cart"] = cart;

            // Chuyển hướng về trang giỏ hàng
            return RedirectToAction("Giohang", "Product");
        }









        public ActionResult Giohang()
        {
            // Lấy giỏ hàng từ session
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
            {
                ViewBag.TotalAmount = 0; // Tổng tiền là 0 khi giỏ hàng trống
                return View(new List<CartItem>()); // Trả về view với giỏ hàng trống
            }

            // Tính tổng tiền
            ViewBag.TotalAmount = cart.Sum(item => item.Product.ProductPrice * item.Quantity);

            return View(cart);
        }


        public ActionResult Thanhtoan()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Giohang");
            }

            // Xử lý thanh toán tại đây (ví dụ: lưu vào database)

            // Sau khi thanh toán xong, xóa giỏ hàng
            Session["Cart"] = null;

            return RedirectToAction("OrderSuccess");
        }

        public ActionResult OrderSuccess()
        {
            return View(); // Tạo view thông báo đặt hàng thành công
        }





    [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            return View();
        }

        [HttpPost]
        public ActionResult RemoveItem(int id)
        {
            return View();
        }
        public ActionResult All(int id)
        {
            var product = csdl.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult tragop(int productId)
        {
            var product = csdl.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                return HttpNotFound(); 
            }

            decimal productPrice = product.ProductPrice;

            var installmentOptions = new List<object>
    {
        new {
            Period = "1 Tháng",
            InterestRate = 0.00m, 
            TotalPayment = productPrice,
            MonthlyPayment = productPrice
        },
        new {
            Period = "3 Tháng",
            InterestRate = 0.15m, 
            TotalPayment = productPrice * 1.15m,
            MonthlyPayment = (productPrice * 1.15m) / 3
        },
        new {
            Period = "6 Tháng",
            InterestRate = 0.30m, 
            TotalPayment = productPrice * 1.30m,
            MonthlyPayment = (productPrice * 1.30m) / 6
        }
    };

            ViewBag.Product = product;
            ViewBag.InstallmentOptions = installmentOptions;

            return PartialView("_tragop"); 
        }




    }
}