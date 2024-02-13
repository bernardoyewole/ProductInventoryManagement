using ProductInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductInventoryManagement.Controllers
{
    public class ProductController : Controller
    {
        // GET: Inventory
        public ActionResult Index()
        {
            ProductBusinessLogic pbl = new ProductBusinessLogic();
            var products = pbl.GetProductsInfo();
            return View(products);
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        public ActionResult AddProductAction(ProductInventory productInventory)
        {
            ProductBusinessLogic pbl = new ProductBusinessLogic();
            if (pbl.AddProductInventory(productInventory))
            {
                return RedirectToAction("Index", "Product");
            } else
            {
                return View("AddProduct");
            }
        }

        public ActionResult DeleteProduct()
        {
            return View();
        }

        public ActionResult DeleteProductAction(FormCollection form)
        {
            ProductBusinessLogic pbl = new ProductBusinessLogic();
            pbl.DeleteProduct(form["productName"]);
            return RedirectToAction("Index", "Product");
        }
    }
}