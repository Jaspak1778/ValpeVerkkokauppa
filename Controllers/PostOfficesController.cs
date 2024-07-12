using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ValpeVerkkokauppa.Models;

namespace ValpeVerkkokauppa.Controllers
{
    public class PostOfficesController : Controller
    {
        private VerkkokauppaEntities db = new VerkkokauppaEntities();

        // GET: PostOffices
        public ActionResult Index()
        {
            return View(db.PostOffice.ToList());
        }

        // GET: PostOffices/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostOffice postOffice = db.PostOffice.Find(id);
            if (postOffice == null)
            {
                return HttpNotFound();
            }
            return View(postOffice);
        }

        // GET: PostOffices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostOffices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Zipcode,PostOffice1")] PostOffice postOffice)
        {
            if (ModelState.IsValid)
            {
                db.PostOffice.Add(postOffice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(postOffice);
        }

        // GET: PostOffices/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostOffice postOffice = db.PostOffice.Find(id);
            if (postOffice == null)
            {
                return HttpNotFound();
            }
            return View(postOffice);
        }

        // POST: PostOffices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Zipcode,PostOffice1")] PostOffice postOffice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postOffice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(postOffice);
        }

        // GET: PostOffices/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostOffice postOffice = db.PostOffice.Find(id);
            if (postOffice == null)
            {
                return HttpNotFound();
            }
            return View(postOffice);
        }

        // POST: PostOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PostOffice postOffice = db.PostOffice.Find(id);
            db.PostOffice.Remove(postOffice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
