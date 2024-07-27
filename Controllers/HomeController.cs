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
        private readonly VerkkokauppaEntities1 db = new VerkkokauppaEntities1();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
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