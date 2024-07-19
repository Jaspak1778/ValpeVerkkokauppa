using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ValpeVerkkokauppa.Models;
using ValpeVerkkokauppa.Helpers;
using ValpeVerkkokauppa.ViewModels;
using System.Data.Entity;

namespace ValpeVerkkokauppa.Controllers
{
    public class AdminsController : Controller
    {
        private VerkkokauppaEntities db = new VerkkokauppaEntities();

        // GET: Admins/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Admins/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var admin = db.Admin.FirstOrDefault(a => a.Email == model.Email);
                if (admin != null && PasswordHelper.VerifyPassword(model.Password, admin.Password, admin.Salt))
                {
                    Session["AdminID"] = admin.AdminID.ToString();
                    Session["AdminName"] = admin.Name.ToString();

                    FormsAuthentication.SetAuthCookie(model.Email, false);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                }
            }
            return View(model);
        }

        // GET: Admins/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // GET: Admins/Index
        public ActionResult Index()
        {
            var admins = db.Admin.ToList();
            return View(admins);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Email,Phonenumber,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                var salt = PasswordHelper.GenerateSalt();
                admin.Salt = salt;
                admin.Password = PasswordHelper.HashPassword(admin.Password, salt);
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }


        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminID,Name,Email,Phonenumber")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }


        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admin.Find(id);
            db.Admin.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
