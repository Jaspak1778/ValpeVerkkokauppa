using System.Linq;
using System.Web.Mvc;
using ValpeVerkkokauppa.Models;
using ValpeVerkkokauppa.Helpers; // Make sure to include the Helpers namespace
using ValpeVerkkokauppa.ViewModels; // Assuming the LoginViewModel is in this namespace

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
    }
}
