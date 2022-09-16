using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Cart
    {
        private NorthwindDataContext da = new NorthwindDataContext();

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal total { get { return UnitPrice * Quantity; } }

        public Cart(int ProductID)
        {
            this.ProductID = ProductID;
            Product p = da.Products.Single(s => s.ProductID == ProductID);
            ProductName = p.ProductName;
            UnitPrice = (decimal)p.UnitPrice;
            Quantity = 1;
        }
    }
}