using System.Linq;
using System.Web.Mvc;
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
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admins/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = db.Admin.FirstOrDefault(a => a.Email == model.Email);
                if (admin != null && PasswordHelper.VerifyPassword(model.Password, admin.Password, admin.Salt))
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

        // Action to update salts and hash passwords for existing users
        public ActionResult UpdateAdminPasswords()
        {
            var admins = db.Admin.ToList();
            foreach (var admin in admins)
            {
                if (string.IsNullOrEmpty(admin.Salt))
                {
                    // Generate a new salt
                    var salt = PasswordHelper.GenerateSalt();
                    admin.Salt = salt;

                    // Hash the existing password with the new salt
                    var hashedPassword = PasswordHelper.HashPassword(admin.Password, salt);
                    admin.Password = hashedPassword;

                    // Update the admin record
                    db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
                }
            }
            db.SaveChanges();

            return Content("Admin passwords updated successfully.");
        }

        // GET: Admins/Logout
        public ActionResult Logout()
        {
            Session.Clear();
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