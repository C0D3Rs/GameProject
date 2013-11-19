using GameProject.Enums;
using GameProject.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Areas.Admin.Controllers
{
    [AuthorizationFilter(UserRole.Admin)]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
