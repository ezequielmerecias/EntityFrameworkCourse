using Microsoft.AspNetCore.Mvc;
using WizLib_DataAccess;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class AuthorController : Controller
    {

        private readonly ApplicationDbContext _db;

        public AuthorController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Author> objList = _db.Authors.ToList();
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            Author obj = new Author();
            if (id == null)
            {
                return View(obj);
            }
            //this for edit
            obj = _db.Authors.FirstOrDefault(q => q.Author_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Author obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.Author_Id == 0)
                {
                    //this is create
                    _db.Authors.Add(obj);
                }
                else
                {
                    //this is update
                    _db.Authors.Update(obj);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(int id)
        {
            var objFromDb = _db.Authors.FirstOrDefault(q => q.Author_Id == id);
            _db.Authors.Remove(objFromDb);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
