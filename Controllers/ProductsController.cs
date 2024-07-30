using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ValpeVerkkokauppa.Models;

namespace ValpeVerkkokauppa.Controllers
{
    public class ProductsController : Controller
    {
        private VerkkokauppaEntities db = new VerkkokauppaEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        public ActionResult GetImage(int id)
        {
            var product = db.Products.Find(id);
            if (product != null && product.Image != null)
            {
                return File(product.Image, "image/jpeg");
            }
            return null; // Or return a default image
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Category, "Category_ID", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,Name,Price,Description,Discount,UnitsInStock")] Products products, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(Image.InputStream))
                    {
                        products.Image = binaryReader.ReadBytes(Image.ContentLength);
                    }
                }

                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Category, "Category_ID", "Name", products.CategoryID);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Category, "Category_ID", "Name", products.CategoryID);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,Name,Price,Description,Discount,UnitsInStock")] Products products, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = db.Products.Find(products.ProductID);
                if (existingProduct == null)
                {
                    return HttpNotFound();
                }

                // Päivitetään tuotteen tiedot
                existingProduct.CategoryID = products.CategoryID;
                existingProduct.Name = products.Name;
                existingProduct.Price = products.Price;
                existingProduct.Description = products.Description;
                existingProduct.Discount = products.Discount;
                existingProduct.UnitsInStock = products.UnitsInStock;

                // Päivitetään tuotteen kuva jos käyttäjä on lähettänyt uuden kuvan
                if (Image != null && Image.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(Image.InputStream))
                    {
                        existingProduct.Image = binaryReader.ReadBytes(Image.ContentLength);
                    }
                }

                db.Entry(existingProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Category, "Category_ID", "Name", products.CategoryID);
            return View(products);
        }


        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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