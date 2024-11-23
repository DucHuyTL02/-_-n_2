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


        public ActionResult ProductDetails(int? id)
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
            List<Product> products = csdl.Products.Take(4).ToList();
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

        public ActionResult Thanhtoan()
        {
            return View();
        }







        public ActionResult Giohang()
        {
            List<Product> giohang = Session["Giohang"] as List<Product>;
            if (giohang == null)
            {
                giohang = new List<Product>(); 
                Session["Giohang"] = giohang;
            }

            return View(giohang); 
        }



        private const string CartSessionKey = "Giohang";
            public ActionResult Giohang(int? productId)
            {
                var product = csdl.Products.FirstOrDefault(p => p.ProductID == productId);
                if (product == null)
                {
                    return View(); 
                }

                var cart = Session[CartSessionKey] as List<Product> ?? new List<Product>();

                var cartItem = cart.FirstOrDefault(c => c.ProductID == productId);
                if (cartItem == null)
                {
                    cart.Add(new Product
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice,
                        StockQuantityPro = 1,
                        ProductImage = product.ProductImage
                    });
                }
                else
                {
                    cartItem.StockQuantityPro++;
                }

                Session[CartSessionKey] = cart;

                return RedirectToAction("Giohang", "Product");
            }

            public ActionResult Giohang1()
            {
                var cart = Session[CartSessionKey] as List<Product> ?? new List<Product>();
                return View(cart);
            }

        [HttpPost]




        public ActionResult Thanhtoan(string PaymentMethod)
        {
            var cart = Session[CartSessionKey] as List<Product>;
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Giohang", "Product"); 
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(c => c.StockQuantityPro * c.ProductPrice),
                ShippingMethod = PaymentMethod,
                PaymentStatus = "Đã giao",
            };

            csdl.Orders.Add(order);
            csdl.SaveChanges();

            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.StockQuantityPro,
                    UnitPrice = item.ProductPrice
                };
                csdl.OrderDetails.Add(orderDetail);
            }
            csdl.SaveChanges();

            Session.Remove(CartSessionKey);

            if (PaymentMethod == "bank")
            {
                return RedirectToAction("Thanhtoan", "Product", new { orderId = order.OrderID });
            }
            else
            {
                return RedirectToAction("Xacnhandon", "Product", new { orderId = order.OrderID });
            }
        }




        public ActionResult Xacnhandon(int orderId)
        {
            var order = csdl.Orders.FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                return HttpNotFound("Không tìm thấy đơn hàng."); 
            }

            if (order.PaymentStatus == "bank")
            {
                order.PaymentStatus = "Đã Giao"; 
            }
            else
            {
                order.PaymentStatus = "Chưa Giao"; 
            }

            csdl.SaveChanges();

            return View(order);
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