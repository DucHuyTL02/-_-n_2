﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web_bán_hàng__đồ_án_
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "ProductDetails",
            url: "Product/ProductDetails/{id}",
            defaults: new { controller = "Product", action = "ProductDetails", id = UrlParameter.Optional }
        );
            routes.MapRoute(
           name: "ProductList",
           url: "Product/List",
           defaults: new { controller = "Product", action = "ProductList" }
       );

        }
    }
}
