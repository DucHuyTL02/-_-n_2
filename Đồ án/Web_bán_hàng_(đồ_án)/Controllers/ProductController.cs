using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_bán_hàng__đồ_án_.Models;

namespace Web_bán_hàng__đồ_án_.Controllers
{
    public class ProductController : Controller
    {
        LTWEntities csdl = new LTWEntities();   
        // GET: Product
        public ActionResult Details(int? id)
        {
            var product = csdl.Products.FirstOrDefault(i => i.ProductPrice == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
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