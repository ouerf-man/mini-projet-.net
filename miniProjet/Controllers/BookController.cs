using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using miniProjet.Models;

namespace Books.Controllers
{
    public class BookController : Controller
    {
        private BooksEntities3 db = new BooksEntities3();

        // GET: Book
        public ActionResult Index()
        {
            return View(db.Book.ToList().OrderBy(m => m.ReleaseDate));
        }

        // POST: Books (Filter by Title)
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string title = form["txtTitle"];

            return View(db.Book.Where(t => t.Title.Contains(title)).OrderBy(m => m.ReleaseDate));
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book Book = db.Book.Find(id);
            if (Book == null)
            {
                return HttpNotFound();
            }
            return View(Book);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ReleaseDate,Director")] Book Book)
        {

            // Validate the Id
            if (!PkValidate(Book))
                ModelState.AddModelError("Id", "There is another Book with the same id");

            // Validate the Book
            if (!BookValidate(Book))
                ModelState.AddModelError("Title", "There is another Book with the same title and director");

            if (ModelState.IsValid)
            {
                db.Book.Add(Book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Book);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book Book = db.Book.Find(id);
            if (Book == null)
            {
                return HttpNotFound();
            }
            return View(Book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,ReleaseDate,Director")] Book Book)
        {
            // Validate the Book
            if (!BookValidate(Book))
            {
                ModelState.AddModelError("Title", "There is another Book with the same title and director");
            }

            if (ModelState.IsValid)
            {
                db.Entry(Book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Book);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book Book = db.Book.Find(id);
            if (Book == null)
            {
                return HttpNotFound();
            }
            return View(Book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book Book = db.Book.Find(id);
            db.Book.Remove(Book);
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

        // Function to validate title and director of the Book
        private bool BookValidate(Book Book)
        {
            // Count Book with the same title and director and different Id
            int book = db.Book.Where(t => t.Title.ToLower() == Book.Title.ToLower())
                                  .Where(d => d.Director.ToLower() == Book.Director.ToLower())
                                  .Where(i => i.Id != Book.Id)
                                  .Count();
            // Return true if there is no duplicate Book
            return book == 0;
        }

        // Function to validate PK
        private bool PkValidate(Book Book)
        {
            // Count Book with the same Id
            int book = db.Book.Where(i => i.Id == Book.Id)
                                  .Count();
            // Return true if there is no duplicate Id
            return book == 0;
        }
    }
}