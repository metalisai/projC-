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
using BLL.Interfaces;
using BLL.DTO;

namespace LendWeb.Controllers
{
    [Authorize]
    public class MyObjectsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly ILendingService _lService;
        private readonly IUsersService _uService;

        public MyObjectsController(UserManager<User> um, SignInManager<User> sm, ILendingService lService, IUsersService uService)
        {
            _userManager = um;
            _signInManager = sm;
            _lService = lService;
            _uService = uService;
        }

        public IActionResult Index()
        {
            string userId = GetUserId();

            var vm = new MyObjectsModel {
                MyObjects = _lService.GetUserLendObjects(userId),
                MyLendings = _lService.GetUserLendings(userId),
                MyBorrowings = _lService.GetUserBorrowings(userId),
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
                _lService.AddUserLendObject(GetUserId(), lendObject.AddObject);
                return RedirectToAction("Index");
            }
            return View(lendObject);
        }

        [HttpGet]
        public IActionResult Lend(string id)
        {
            if (id == null || _lService.GetUserObject(GetUserId(), id) == null)
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

        [HttpGet]
        public IActionResult MarkReturned(string id)
        {
            LendObjectDTO lobject;
            if (id != null)
            {
                lobject = _lService.GetUserObject(GetUserId(), id);
                if(lobject != null && lobject.Status != LendObjectDTO.LendObjectStatus.Available)
                {
                    _lService.UserObjectReturned(GetUserId(), id);
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }
            return HttpNotFound();
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

                if (_lService.GetUserObject(GetUserId(),model.ObjectId) == null)
                {
                    return HttpNotFound();
                }

                if (!string.IsNullOrEmpty(model.LendToUser))
                {
                    if (_uService.UserExists(model.LendToUser))
                    {
                        _lService.LendUserObject(GetUserId(), model.ObjectId, model.LendToUser);
                        return RedirectToAction("Index");
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
