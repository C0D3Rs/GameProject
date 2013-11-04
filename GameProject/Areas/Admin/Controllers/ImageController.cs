using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Areas.Admin.Controllers
{
    public class ImageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}