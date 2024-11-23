using System;
using Web_bán_hàng__đồ_án_.Models; // Thay bằng namespace của Model dự án của bạn

namespace Web_bán_hàng__đồ_án_.Models.ViewModel
{
    public class CartItem
    {
        public Product Product { get; set; } 
        public int Quantity { get; set; }
        public decimal TotalPrice => Product.ProductPrice * Quantity;

    }
}
