using System;
using System.Collections.Generic;
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
        // GET: Product
        public ActionResult ProductDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pro =csdl.Products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }
            var product = csdl.Products.Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID).AsQueryable();
            ProductDetailsVM model = new ProductDetailsVM();
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
            return View();
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
    }
}