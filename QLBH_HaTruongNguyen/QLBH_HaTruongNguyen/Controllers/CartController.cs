using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_HaTruongNguyen.Models;
using System.Transactions;

namespace QLBH_HaTruongNguyen.Controllers
{
    public class CartController : Controller
    {
        private NorthwindDataContext dt = new NorthwindDataContext();
        // GET: Cart
        public List<Cart> GetListCarts()
        {
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if(carts == null)
            {
                carts = new List<Cart>();
                Session["Cart"] = carts;
            }
            return carts;
        }

        private int Count()
        {
            int n = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if(carts != null)
            {
                n = carts.Sum(s => s.Quantity);
            }
            return n;
        }

        private decimal  Total()
        {
            decimal n = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts != null)
            {
                n = carts.Sum(s => s.Total);
            }
            return n;
        }

        public ActionResult AddCart (int id)
        {
            List<Cart> carts = GetListCarts();
            Cart c = carts.Find(s => s.productID == id);
            if(c == null)
            {
                c = new Cart(id);
                carts.Add(c);
            }
            else
            {
                c.Quantity++;
            }
            return RedirectToAction("ListCarts");
        }

        public ActionResult ListCarts()
        {
            List<Cart> carts = GetListCarts();

            if (carts.Count == 0)
            {
                return RedirectToAction("ListProducts", "Product");
            }
            ViewBag.CountProduct = Count();
            ViewBag.Total = Total();

            return View(carts);
        }

        public ActionResult Delete(int id)
        {
            List<Cart> carts = GetListCarts(); // Lấy DS GH
            Cart c = carts.Find(s => s.productID == id);
            if (c != null)
            {
                carts.RemoveAll(s => s.productID == id);
            }
            if (carts.Count == 0)
            {
                return RedirectToAction("ListProducts", "Product");
            }

            return RedirectToAction("ListCarts");
        }
         public ActionResult OrderProduct (FormCollection fCollection)
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    Order order = new Order();
                    order.OrderDate = DateTime.Now;
                    dt.Orders.InsertOnSubmit(order);
                    dt.SubmitChanges();

                    List<Cart> carts = GetListCarts();
                    foreach (var item in carts)
                    {
                        Order_Detail od = new Models.Order_Detail();
                        od.OrderID = order.OrderID;
                        od.ProductID = item.productID;
                        od.Quantity =short.Parse( item.Quantity.ToString());
                        od.UnitPrice = item.UnitPrice;
                        od.Discount = 0;

                        dt.Order_Details.InsertOnSubmit(od);
                    }

                    dt.SubmitChanges();
                    tranScope.Complete();
                    Session["Cart"] = null;
                }
                catch (Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("ListCarts");
                }
            }
            return RedirectToAction("OrderDetailList", "Cart");
        }
        public ActionResult OrderDetailList()
        {
            var p = dt.Order_Details.OrderByDescending(s => s.OrderID).ToList();
            return View(p);
        } 
    }
}