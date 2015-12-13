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
            string userId = GetUserId();

            var vm = new MyObjectsModel {
                MyObjects = _repos.LendObjectRepository.GetUserObjects(userId),
                MyLendings = _repos.LendingRepository.GetUserLendings(userId),
                MyBorrowings = _repos.LendingRepository.GetUserBorrowings(userId),
            };
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
            if (id == null || _repos.LendObjectRepository.GetUserObject(GetUserId(), id) == null)
            {
                return HttpNotFound();
            }

            var model = new LendModel
            {
                ObjectId = id
            };
            
            /*LendObject lendObject = _context.LendObject.Single(m => m.Id == id);
            if (lendObject == null)
            {
                return HttpNotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "User", lendObject.UserId);*/
            return View("Lend",model);
        }

        [HttpPost]
        public IActionResult LendToUser(LendToUserModel model)
        {
            if (string.IsNullOrEmpty(model.ObjectId))
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {

                var lrepo = _repos.LendingRepository;
                var lorepo = _repos.LendObjectRepository;
                var urepo = _repos.UserRepository;

                var lending = new Lending();

                if (lorepo.GetUserObject(GetUserId(),model.ObjectId) == null)
                {
                    return HttpNotFound();
                }

                if (!string.IsNullOrEmpty(model.LendToUser))
                {
                    User otherUser = urepo.FindByUserName(model.LendToUser);
                    if (otherUser != null)
                    {
                        lending.OtherUser = otherUser.Id;
                        lending.LendObjectId = model.ObjectId;
                        lending.LentAt = DateTime.Now;
                        lending.ExpectedReturn = DateTime.Now.AddDays(7);
                        lrepo.LendUserObject(GetUserId(), lending);
                        RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("LendToUser", "User " + model.LendToUser + " does'nt exist!");

                        return View("Lend",new LendModel(model) { Borrower = LendModel.BorrowerType.User });
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult LendToContact(LendToContactModel model)
        {
            if (string.IsNullOrEmpty(model.ObjectId))
            {
                return HttpNotFound();
            }
            ModelState.AddModelError(string.Empty, "Not implemented!");
            return View("Lend", new LendModel(model)  { Borrower = LendModel.BorrowerType.Contact });
        }

        private string GetUserId()
        {
            return HttpContext.User.GetUserId();
        }
    }
}
