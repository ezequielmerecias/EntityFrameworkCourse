﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WizLib_DataAccess;
using WizLib_Model.Models;
using WizLib_Model.ViewModels;

namespace WizLib.Controllers
{
    public class BookController : Controller
    {

        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            //Eager Loading 
            List<Book> objList = _db.Books.Include(q => q.Publisher).ToList();
            //List<Book> objList = _db.Books.ToList();
            //foreach(var obj in objList)
            //{
            //    //Incorrect
            //    //obj.Publisher = _db.Publishers.FirstOrDefault(q => q.Publisher_Id == obj.Publisher_Id);

            //    //Explicit loading More efficient
            //    _db.Entry(obj).Reference(q => q.Publisher).Load();
            //}
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM obj = new BookVM();
            obj.PublisherList = _db.Publishers.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Publisher_Id.ToString()
            });
            if (id == null)
            {
                return View(obj);
            }
            //this for edit
            obj.Book = _db.Books.FirstOrDefault(q => q.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM obj)
        {
            if (obj.Book.Book_Id == 0)
            {
                //this is create
                _db.Books.Add(obj.Book);
            }
            else
            {
                //this is update
                _db.Books.Update(obj.Book);
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var objFromDb = _db.Books.FirstOrDefault(q => q.Book_Id == id);
            _db.Books.Remove(objFromDb);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            BookVM obj = new BookVM();

            if (id == null)
            {
                return View(obj);
            }
            obj.Book = _db.Books.Include(q => q.BookDetail).FirstOrDefault(q => q.Book_Id == id);

            //this for edit
            //obj.Book = _db.Books.FirstOrDefault(q => q.Book_Id == id);
            //obj.Book.BookDetail = _db.BookDetails.FirstOrDefault(q => q.BookDetail_Id == obj.Book.BookDetail_Id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM obj)
        {
            if (obj.Book.BookDetail.BookDetail_Id == 0)
            {
                //this is create
                _db.BookDetails.Add(obj.Book.BookDetail);
                _db.SaveChanges();

                var BookFromDb = _db.Books.FirstOrDefault(q => q.Book_Id == obj.Book.Book_Id);
                BookFromDb.BookDetail_Id = obj.Book.BookDetail.BookDetail_Id;
                _db.SaveChanges();
            }
            else
            {
                //this is update
                _db.BookDetails.Update(obj.Book.BookDetail);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult PlayGround()
        {
            //var bookTemp = _db.Books.FirstOrDefault();
            //bookTemp.Price = 100;

            //var bookCollection = _db.Books;
            //double totalPrice = 0;

            //foreach (var book in bookCollection)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookList = _db.Books.ToList();
            //foreach (var book in bookList)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookCollection2 = _db.Books;
            //var bookCount1 = bookCollection2.Count();

            //var bookCount2 = _db.Books.Count();

            IEnumerable<Book> BookList1 = _db.Books;
            var FilteredBook1 = BookList1.Where(q => q.Price > 500).ToList();

            IQueryable<Book> BookList2 = _db.Books;
            var filteredBook2 = BookList2.Where(q => q.Price > 500).ToList();

            var Category = _db.Categories.FirstOrDefault();
            _db.Entry(Category).State = EntityState.Modified;
            _db.SaveChanges();

            //Updating Related Data
            var bookTemp1 = _db.Books.Include(q => q.BookDetail).FirstOrDefault(q => q.Book_Id == 3);
            bookTemp1.BookDetail.NumberOfChapters = 2222;
            _db.Books.Update(bookTemp1);
            _db.SaveChanges();

            //Más eficiente usar Attach para no actualizar los padres en entidades derivadas
            var bookTemp2 = _db.Books.Include(q => q.BookDetail).FirstOrDefault(q => q.Book_Id == 3);
            bookTemp2.BookDetail.Weight = 3333;
            _db.Books.Attach(bookTemp2);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
