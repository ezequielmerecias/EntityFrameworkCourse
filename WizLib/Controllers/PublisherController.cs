using Microsoft.AspNetCore.Mvc;
using WizLib_DataAccess;
using WizLib_Model.Models;

namespace WizLib.Controllers
{
    public class PublisherController : Controller
    {

        private readonly ApplicationDbContext _db;

        public PublisherController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Publisher> objList = _db.Publishers.ToList();
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {
            Publisher obj = new Publisher();
            if (id == null)
            {
                return View(obj);
            }
            //this for edit
            obj = _db.Publishers.FirstOrDefault(q => q.Publisher_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Publisher obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.Publisher_Id == 0)
                {
                    //this is create
                    _db.Publishers.Add(obj);
                }
                else
                {
                    //this is update
                    _db.Publishers.Update(obj);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public IActionResult Delete(int id)
        {
            var objFromDb = _db.Publishers.FirstOrDefault(q => q.Publisher_Id == id);
            _db.Publishers.Remove(objFromDb);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


    }
}
