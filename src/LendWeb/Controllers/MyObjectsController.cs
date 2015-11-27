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

namespace LendWeb.Controllers
{
    [Authorize]
    public class MyObjectsController : Controller
    {
        private IUserRepository _userrepo;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public MyObjectsController(IUserRepository userrepo, UserManager<User> um, SignInManager<User> sm)
        {
            _userrepo = userrepo;
            _userManager = um;
            _signInManager = sm;
        }

        public async Task<IActionResult> Index()
        {
            return View(_userrepo.GetUserObjects(HttpContext.User.GetUserId()));
        }
    }
}
