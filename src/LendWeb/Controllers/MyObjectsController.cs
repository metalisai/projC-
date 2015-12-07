using DAL;
using DAL.Interfaces;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LendWeb.ViewModels.MyObjects;

namespace LendWeb.Controllers
{
    [Authorize]
    public class MyObjectsController : Controller
    {
        private IRepositoryProvider _repos;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public MyObjectsController(IRepositoryProvider repos, UserManager<User> um, SignInManager<User> sm)
        {
            _repos = repos;
            _userManager = um;
            _signInManager = sm;
        }

        public IActionResult Index()
        {
            var vm = new MyObjectsModel { MyObjects = _repos.LendObjectRepository.GetUserObjects(GetUserId()) } ;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MyObjectsModel lendObject)
        {
            if (ModelState.IsValid)
            {
                lendObject.AddObject.Added = DateTime.Now;
                _repos.LendObjectRepository.Add(GetUserId(), lendObject.AddObject);
                return RedirectToAction("Index");
            }
            return View(lendObject);
        }

        [HttpGet]
        public IActionResult Lend(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            /*LendObject lendObject = _context.LendObject.Single(m => m.Id == id);
            if (lendObject == null)
            {
                return HttpNotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "User", lendObject.UserId);*/
            var vm = new MyObjectsModel { MyObjects = _repos.LendObjectRepository.GetUserObjects(GetUserId()) };
            return View("Index",vm);
        }

        private string GetUserId()
        {
            return HttpContext.User.GetUserId();
        }
    }
}
