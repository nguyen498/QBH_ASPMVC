using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLBH_HaTruongNguyen.Models;

namespace QLBH_HaTruongNguyen.Models
{
    public class Cart
    {

        private NorthwindDataContext da = new NorthwindDataContext();

        public int productID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get { return UnitPrice * Quantity; } }

        public Cart(int productID)
        {
            this.productID = productID;
            Product prod = da.Products.Single(n => n.ProductID == productID);
            ProductName = prod.ProductName;
            UnitPrice = (decimal) prod.UnitPrice;
            Quantity = 1;
        }

    }
}