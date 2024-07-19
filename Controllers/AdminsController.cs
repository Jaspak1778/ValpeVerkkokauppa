using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValpeVerkkokauppa.ViewModels;
using ValpeVerkkokauppa.Models;

namespace ValpeVerkkokauppa.Controllers
{
    public class AdminsController : Controller
    {
        private VerkkokauppaEntities db = new VerkkokauppaEntities();

        // GET: Admins/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = db.Admin.FirstOrDefault(a => a.Email == model.Email && a.Password == model.Password);
                if (admin != null)
                {
                    Session["AdminID"] = admin.AdminID.ToString();
                    Session["AdminName"] = admin.Name.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }
            return View(model);
        }

        // Logout function
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
