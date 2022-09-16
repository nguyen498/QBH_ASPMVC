using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class ProductController : Controller
    {
        NorthwindDataContext da = new NorthwindDataContext();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            List<Product> products = da.Products.Select(p => p).ToList();
            return View(products);
        }

        public ActionResult Details(int id)
        {
            Product prod = da.Products.Where(p => p.ProductID == id).FirstOrDefault();

            return View(prod);
        }

        public ActionResult Create ()
        {
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        [HttpPost]
        public ActionResult Create (Product product, FormCollection form)
        {
            product.CategoryID = int.Parse(form["LSP"]);
            product.SupplierID = int.Parse(form["NCC"]);

            da.Products.InsertOnSubmit(product);
            da.SubmitChanges();

            return RedirectToAction("list");
        }

        public ActionResult Delete (int id)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductID == id); 

            return View(p);
        }

        [HttpPost]
        public ActionResult Delete (int id, FormCollection collection)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductID == id);
            da.Products.DeleteOnSubmit(p);
            da.SubmitChanges();

            return RedirectToAction("list");
        }

        public ActionResult Edit(int id)
        {

            Product p = da.Products.FirstOrDefault(s => s.ProductID == id);
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");

            return View();
        }
        [HttpPost]
        public ActionResult Edit(Product product, FormCollection collection)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductID == product.ProductID);
            p.UnitPrice = product.UnitPrice;
            p.QuantityPerUnit = product.QuantityPerUnit;
            p.UnitsInStock = product.UnitsInStock;
            p.UnitsOnOrder = product.UnitsOnOrder;
            p.ReorderLevel = product.ReorderLevel;
            p.Discontinued = product.Discontinued;
            p.SupplierID = int.Parse(collection["NCC"]);
            p.CategoryID = int.Parse(collection["LSP"]);
            UpdateModel(p);
            da.Products.InsertOnSubmit(p);
            da.SubmitChanges();

            return RedirectToAction("ListProducts");
        }
    }
}