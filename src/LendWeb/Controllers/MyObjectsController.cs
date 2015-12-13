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
            return View("Lend");
        }

        [HttpPost]
        public IActionResult Lend(string id, LendModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return HttpNotFound();
            }

            var lrepo = _repos.LendingRepository;
            var lorepo = _repos.LendObjectRepository;
            var urepo = _repos.UserRepository;

            var lending = new Lending
            {
                //LendObjectId = new MongoDB.Bson.ObjectId()
            };

            //lrepo.LendUserObject(GetUserId(), lending)
            if(!lorepo.GetUserObjects(GetUserId()).Any(x => x.Id == id))
            {
                return HttpNotFound();
            }

            User otherUser = urepo.FindByUserName(model.LendToUser);
            if (!string.IsNullOrEmpty(model.LendToUser))
            {
                if (otherUser != null)
                {
                    lending.OtherUser = otherUser.Id;
                    lending.LendObjectId = id;
                    lending.LentAt = DateTime.Now;
                    lending.ExpectedReturn = DateTime.Now.AddDays(7);
                    lrepo.LendUserObject(GetUserId(), lending);
                }
                else
                {
                    return View(model);
                }
            }
            else
            {

            }

            /*LendObject lendObject = _context.LendObject.Single(m => m.Id == id);
            if (lendObject == null)
            {
                return HttpNotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "User", lendObject.UserId);*/
            return RedirectToAction("Index");
        }

        private string GetUserId()
        {
            return HttpContext.User.GetUserId();
        }
    }
}
