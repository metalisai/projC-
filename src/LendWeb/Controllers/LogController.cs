using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using BLL.Interfaces;
using LendWeb.ViewModels.Logs;

namespace LendWeb.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        ILogService _logServ;

        public LogController(ILogService logserv)
        {
            _logServ = logserv;
        }

        public IActionResult Index()
        {
            var model = new LogsModel() { LogEntries = _logServ.GetUserActions(GetUserId()) };
            return View(model);
        }

        private string GetUserId()
        {
            return HttpContext.User.GetUserId();
        }
    }
}
