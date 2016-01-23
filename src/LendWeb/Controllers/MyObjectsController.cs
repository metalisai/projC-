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
using Microsoft.AspNet.Http;
using System.IO;
using Microsoft.Data.Entity.Design.Internal;
using Microsoft.Extensions.PlatformAbstractions;

namespace LendWeb.Controllers
{
    [Authorize]
    public class MyObjectsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly ILendingService _lService;
        private readonly IUsersService _uService;

        private readonly IApplicationEnvironment _hostingEnvironment;


        public MyObjectsController(UserManager<User> um, SignInManager<User> sm, ILendingService lService, 
            IUsersService uService, IApplicationEnvironment hostingEnvironment)
        {
            _userManager = um;
            _signInManager = sm;
            _lService = lService;
            _uService = uService;
            _hostingEnvironment = hostingEnvironment;

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
            // TODO: check name for null or empty
            if (ModelState.IsValid)
            {
                lendObject.AddObject.Added = DateTime.Now;
                var nid = _lService.AddUserLendObject(GetUserId(), lendObject.AddObject);
                return RedirectToAction("Show",new { id = nid });
            }
            return View(lendObject);
        }

        [HttpGet]
        public IActionResult Show(string id)
        {
            LendObjectDTO lObject;

            // make sure the request is valid
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                lObject = _lService.GetUserObject(GetUserId(), id);
                if(lObject == null)
                {
                    return HttpNotFound();
                }
            }

            var model = new ShowModel
            {
                ShowObject = lObject
            };

            return View("Show", model);
        }

        public async Task<ActionResult> PostFile(string id, IList<IFormFile> files)
        {
            // TODO: check if lendobject exists
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
 
            // loop files (we only have one anyway...)
            foreach (var f in files)
            {
                // TODO: move to configuration
                string filePath = _hostingEnvironment.ApplicationBasePath + "\\wwwroot\\uploads\\";

                // make directory for uploads if doesn't exist
                if(!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                // is it a png?
                if (f.ContentType == "image/png")
                {
                    string fileName = Guid.NewGuid() + ".png";
                    await f.SaveAsAsync(Path.Combine(filePath, fileName));
                    _lService.AddImageToLendObject(GetUserId(), id, fileName);
                }
                // TODO: show error to player if wasn't png or something went wrong!
            }
            return RedirectToAction("Show",new { id = id });
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
            
            return View("Lend",model);
        }

        [HttpGet]
        public IActionResult MarkReturned(string id)
        {
            LendObjectDTO lobject;
            if (id != null) // id specified?
            {
                lobject = _lService.GetUserObject(GetUserId(), id);
                // object exists and is avaiable?
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
        [ValidateAntiForgeryToken]
        public IActionResult AddObjectProperty(ShowModel model)
        {
            object propertyToAdd;
            // specify type
            switch(model.AddPropertyType)
            {
                case LendObject.LoProperty.LoPropertyType.Date:
                    propertyToAdd = model.AddProperty.Date;
                    break;
                case LendObject.LoProperty.LoPropertyType.Text:
                    propertyToAdd = model.AddProperty.Text;
                    break;
                case LendObject.LoProperty.LoPropertyType.Integer:
                    propertyToAdd = model.AddProperty.Number;
                    break;
                default:
                    // TODO: more graceful error handling
                    return new HttpStatusCodeResult(500);
            }
            _lService.AddPropertyToLendObject(GetUserId(), model.ShowObject.Id, new LendObject.LoProperty {
                PropertyName = model.AddProperty.Name,
                Property = propertyToAdd});

            return RedirectToAction("Show", new { id = model.ShowObject.Id });
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
                // the object doesnt exist
                if (_lService.GetUserObject(GetUserId(),model.ObjectId) == null)
                {
                    return HttpNotFound();
                }
                // other user is specified?
                if (!string.IsNullOrEmpty(model.LendToUser))
                {
                    // other user exists?
                    if (_uService.UserWithNameExists(model.LendToUser))
                    {
                        _lService.LendUserObject(GetUserId(), model.ObjectId, model.LendToUser);
                        return RedirectToAction("Index");
                    }
                    else // doesn't exist
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
            // object not specified or doesn't exist
            if (string.IsNullOrEmpty(model.ObjectId) || _lService.GetUserObject(GetUserId(), model.ObjectId) == null)
            {
                return HttpNotFound();
            }
            // contact not specified
            if (string.IsNullOrEmpty(model.LendToName))
            {
                ModelState.AddModelError(string.Empty, "Name field must be filled!");
                return View("Lend", new LendModel(model) { Borrower = LendModel.BorrowerType.Contact });
            }
            // contact email not specified
            if (string.IsNullOrEmpty(model.LendToEmail))
            {
                ModelState.AddModelError(string.Empty, "Email field must be filled!");
                return View("Lend", new LendModel(model) { Borrower = LendModel.BorrowerType.Contact });
            }

            _lService.LendUserObjectToContact(GetUserId(), model.ObjectId, model.LendToName, model.LendToEmail);
            return RedirectToAction("Index");
        }

        private string GetUserId()
        {
            return HttpContext.User.GetUserId();
        }
    }
}
