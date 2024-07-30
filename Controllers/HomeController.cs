using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ValpeVerkkokauppa.Models;
using ValpeVerkkokauppa.CustomModel;
using System.Net.NetworkInformation;

namespace ValpeVerkkokauppa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ValpeEntities db = new ValpeEntities();

        // GET: Products
        public ActionResult Index()
        {
            var viewModel = new ProductCategoryViewModel
            {
                Products = db.Products.Include(p => p.Category).ToList(),
                Categories = db.Category.ToList()
            };

            return View(viewModel);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}