using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using DAL;
using Model;
using DAL.Interfaces;

namespace LendWeb.Controllers
{
    public class LendObjectsController : Controller
    {
        private IUserRepository _userrepo;

        public LendObjectsController(IUserRepository userrepo)
        {
            _userrepo = userrepo;
        }
        /*
        // GET: LendObjects
        public IActionResult Index()
        {
            //var applicationDbContext = _context.LendObject.Include(l => l.User);
            return View(applicationDbContext.ToList());
        }

        // GET: LendObjects/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            LendObject lendObject = _context.LendObject.Single(m => m.Id == id);
            if (lendObject == null)
            {
                return HttpNotFound();
            }

            return View(lendObject);
        }

        // GET: LendObjects/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users.ToList(), "Id", "User");
            return View();
        }

        // POST: LendObjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LendObject lendObject)
        {
            if (ModelState.IsValid)
            {
                _context.LendObject.Add(lendObject);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users.ToList(), "Id", "User", lendObject.UserId);
            return View(lendObject);
        }

        // GET: LendObjects/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            LendObject lendObject = _context.LendObject.Single(m => m.Id == id);
            if (lendObject == null)
            {
                return HttpNotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "User", lendObject.UserId);
            return View(lendObject);
        }

        // POST: LendObjects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LendObject lendObject)
        {
            if (ModelState.IsValid)
            {
                _context.Update(lendObject);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "User", lendObject.UserId);
            return View(lendObject);
        }

        // GET: LendObjects/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            LendObject lendObject = _context.LendObject.Single(m => m.Id == id);
            if (lendObject == null)
            {
                return HttpNotFound();
            }

            return View(lendObject);
        }

        // POST: LendObjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            LendObject lendObject = _context.LendObject.Single(m => m.Id == id);
            _context.LendObject.Remove(lendObject);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }*/
    }
}
