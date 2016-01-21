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
using LendWeb.ViewModels.Users;

namespace LendWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly ILendingService _lService;
        private readonly IUsersService _uService;

        private readonly IApplicationEnvironment _hostingEnvironment;


        public UsersController(UserManager<User> um, SignInManager<User> sm, ILendingService lService,
            IUsersService uService, IApplicationEnvironment hostingEnvironment)
        {
            _userManager = um;
            _signInManager = sm;
            _lService = lService;
            _uService = uService;
            _hostingEnvironment = hostingEnvironment;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new SearchPageModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(SearchPageModel model)
        {
            if (!string.IsNullOrEmpty(model.Search))
            {
                model.Users = _uService.SearchUsersByName(model.Search);
            }
            return View(model);
        }

        public IActionResult Profile(string id)
        {
            if(string.IsNullOrEmpty(id) || !_uService.UserWithIdExists(id))
            {
                return HttpNotFound();
            }

            UserPageModel model = new UserPageModel { User = _uService.FindUserById(id)};
            return View(model);
        }

        public IActionResult Object(string id, string objectId)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(objectId))
            {
                UserDTO user = _uService.FindUserById(id);
                if (user != null)
                {
                    var obj = _lService.GetUserObject(id, objectId);
                    if (obj != null)
                    {
                        var model = new ObjectPageModel { User = user, Object = obj };
                        return View(model);
                    }
                }
                    
            }
            return HttpNotFound();
        }
    }
}

