using QLBH_HaTruongNguyen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH_HaTruongNguyen.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        NorthwindDataContext da = new NorthwindDataContext();

        public ActionResult ListProducts()
        {
            List <Product> products = da.Products.Select(s => s).ToList();
            return View(products);
        }

        public ActionResult Details(int id)
        {
            Product product = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            //Product p = da.Products.FirstOrDefault(s => s.ProductID == id);
            return View(product);
        }

        public ActionResult Create()
        {
            ViewData["NCC"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product p, FormCollection collection)
        {
            p.SupplierID = int.Parse(collection["NCC"]);
            p.CategoryID = int.Parse(collection["LSP"]);
            da.Products.InsertOnSubmit(p);
            da.SubmitChanges();

            return RedirectToAction("ListProducts");
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
        public ActionResult Delete(int id, FormCollection collection)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductID == id);
            ViewData["ncc"] = new SelectList(da.Suppliers, "SupplierID", "CompanyName");
            ViewData["LSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            return View(p);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductID == id);
            da.Products.DeleteOnSubmit(p);
            da.SubmitChanges();
            return RedirectToAction("ListProducts");
        }
    }

}