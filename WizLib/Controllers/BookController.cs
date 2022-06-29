using Microsoft.AspNetCore.Mvc;
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
            //List<Book> objList = _db.Books.Include(q => q.Publisher).ToList();

            //List<Book> objList = _db.Books.ToList();
            //foreach(var obj in objList)
            //{
            //    //Incorrect
            //    //obj.Publisher = _db.Publishers.FirstOrDefault(q => q.Publisher_Id == obj.Publisher_Id);

            //    //Explicit loading More efficient
            //    _db.Entry(obj).Reference(q => q.Publisher).Load();
            //}

            //List<Book> objList = _db.Books.ToList();
            //foreach (var obj in objList)
            //{
            //    //Explicit loading More efficient
            //    _db.Entry(obj).Reference(q => q.Publisher).Load();
            //    _db.Entry(obj).Collection(q => q.BookAuthors).Load();
            //    foreach(var bookAuth in obj.BookAuthors)
            //    {
            //        _db.Entry(bookAuth).Reference(q => q.Author).Load();
            //    }
            //}

            //Eager Loading, simplifica lo de arriba
            List<Book> objList = _db.Books.Include(q => q.Publisher)
                                            .Include(q => q.BookAuthors)
                                            .ThenInclude(q => q.Author).ToList();


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

        public IActionResult ManageAuthors(int id)
        {
            BookAuthorVM obj = new BookAuthorVM
            {
                BookAuthorList = _db.BookAuthors.Include(q => q.Author).Include(q => q.Book)
                                .Where(q => q.Book_Id == id).ToList(),
                BookAuthor = new BookAuthor()
                {
                    Book_Id = id
                },
                Book = _db.Books.FirstOrDefault(q => q.Book_Id == id)
            };
            List<int> tempListOfAssignedAuthor = obj.BookAuthorList.Select(q => q.Author_Id).ToList();

            //NOT IN Clause in LINQ
            //get all the author whos id is not in tempListOfAssignedAuthor
            var tempList = _db.Authors.Where(q => !tempListOfAssignedAuthor.Contains(q.Author_Id)).ToList();

            obj.AuthorList = tempList.Select(q => new SelectListItem
            {
                Text = q.FullName,
                Value = q.Author_Id.ToString()
            });

            return View(obj);
        }

        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if(bookAuthorVM.BookAuthor.Book_Id != 0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _db.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });
        }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookId = bookAuthorVM.Book.Book_Id;
            BookAuthor bookAuthor = _db.BookAuthors
                .FirstOrDefault(q => q.Author_Id == authorId && q.Book_Id == bookId);

            _db.BookAuthors.Remove(bookAuthor);
            _db.SaveChanges();
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
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

            //IEnumerable<Book> BookList1 = _db.Books;
            //var FilteredBook1 = BookList1.Where(q => q.Price > 500).ToList();

            //IQueryable<Book> BookList2 = _db.Books;
            //var filteredBook2 = BookList2.Where(q => q.Price > 500).ToList();

            //var Category = _db.Categories.FirstOrDefault();
            //_db.Entry(Category).State = EntityState.Modified;
            //_db.SaveChanges();

            ////Updating Related Data
            //var bookTemp1 = _db.Books.Include(q => q.BookDetail).FirstOrDefault(q => q.Book_Id == 3);
            //bookTemp1.BookDetail.NumberOfChapters = 2222;
            //_db.Books.Update(bookTemp1);
            //_db.SaveChanges();

            ////Más eficiente usar Attach para no actualizar los padres en entidades derivadas
            //var bookTemp2 = _db.Books.Include(q => q.BookDetail).FirstOrDefault(q => q.Book_Id == 3);
            //bookTemp2.BookDetail.Weight = 3333;
            //_db.Books.Attach(bookTemp2);
            //_db.SaveChanges();

            //VIEWS
            var viewList = _db.BookDetailsFromViews.ToList();
            var viewList1 = _db.BookDetailsFromViews.FirstOrDefault();
            var viewList2 = _db.BookDetailsFromViews.Where(q => q.Price > 500);

            //RAW SQL

            var bookRaw = _db.Books.FromSqlRaw("SELECT * FROM dbo.Books").ToList();

            //SQL Injection Attack Prone
            int id = 1;
            var bookTemp = _db.Books.FromSqlInterpolated($"SELECT * FROM dbo.Books Where Book_Id={id}").ToList();

            var booksSproc = _db.Books.FromSqlInterpolated($"EXEC dbo.getAllBookDetails {id}").ToList();

            //.NET 5 superior
            var bookFilter1 = _db.Books.Include(e => e.BookAuthors.Where(q => q.Author_Id == 1)).ToList();
            var bookFilter2 = _db.Books.Include(e => e.BookAuthors.OrderByDescending(q => q.Author_Id).Take(1)).ToList();



            return RedirectToAction(nameof(Index));
        }
    }
}
