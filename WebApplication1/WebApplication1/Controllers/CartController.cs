using System;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        NorthwindDataContext da = new NorthwindDataContext();
       
        public List<Cart> ListCart()
        {
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if(carts == null)
            {
                carts = new List<Cart>();
                Session["Cart"] = carts;
            }

            return carts;
        }
       
        public ActionResult addCart(int id)
        {
            List<Cart> carts = ListCart();
            Cart cartItem = carts.Find(c => c.ProductID == id);
            if (cartItem == null)
            {
                cartItem = new Cart(id);
                carts.Add(cartItem); 
            }
            else
            {
                cartItem.Quantity++;
            }
            return RedirectToAction("list");
        }

        public decimal CartTotal(decimal cartTotal = 0)
        {
            List<Cart> carts = ListCart();
            cartTotal = carts.Sum(c => c.total);

            return cartTotal;
        }

        public int CartCount(int cartCount = 0)
        {
            List<Cart> carts = ListCart();
            cartCount = carts.Sum(c => c.Quantity);

            return cartCount;
        }

        public ActionResult List()
        {
            List<Cart> list = ListCart();
            ViewBag.CartCount = CartCount();
            ViewBag.CartTotal = CartTotal();

            return View(list);
        }

        public ActionResult OrderProduct(FormCollection  form)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    //Create Order
                    Order or = new Order();
                    or.OrderDate = DateTime.Now;
                    da.Orders.InsertOnSubmit(or);
                    da.SubmitChanges();
                    //For each cart item in carts => Create Order Item
                    List<Cart> carts = ListCart();
                    foreach(var item in carts)
                    {
                        Order_Detail od = new Order_Detail();
                        od.OrderID = or.OrderID;
                        od.ProductID = item.ProductID;
                        od.Quantity = short.Parse(item.Quantity.ToString());
                        od.UnitPrice = item.UnitPrice;
                        od.Discount = 0;

                        da.Order_Details.InsertOnSubmit(od);
                    }
                    da.SubmitChanges();
                    transactionScope.Complete();
                    Session["Cart"] = null;

                }
                catch (Exception)
                {
                    transactionScope.Dispose();
                    return RedirectToAction("list");
                }
            }

            return RedirectToAction("OrderDetailsList", "Cart");
        }

        public ActionResult OrderDetailsList()
        {
            var p = da.Order_Details.OrderByDescending(s => s.OrderID).Select(s => s).ToList();
            return View(p);
        }
    }
}