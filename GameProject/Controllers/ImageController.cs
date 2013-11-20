using GameProject.Enums;
using GameProject.Filters;
using GameProject.Helpers;
using GameProject.Models;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Controllers
{
    public class ImageController : Controller
    {

        private DatabaseContext db = new DatabaseContext();

        public ActionResult Show(string imageName = "")
        {
            if (String.IsNullOrEmpty(imageName))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from a in db.Images
                        where a.FileName == imageName
                        select a;

            var img = query.FirstOrDefault();

            if (img == null)
            {
                return HttpNotFound();
            }

            return File(img.Data, img.Type);
        }
    }
}
