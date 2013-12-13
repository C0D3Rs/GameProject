using GameProject.ViewModels;
using GameProject.Enums;
using GameProject.Filters;
using GameProject.Helpers;
using GameProject.Models;
using GameProject.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GameProject.Controllers
{
    [AuthorizationFilter(UserRole.Normal, Order = 1)]
    [CharacterCreatorFilter(Order = 2)]
    [CharacterResourcesFilter(Order = 3)]
    public class LocationController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var query = from r1 in db.Locations
                        from r2 in db.Images
                        where r1.ImageId == r2.ID &&
                              r2.Category == ImageCategory.Location
                        select new DetailsLocationViewModel
                        {    Location = r1,
                             Image = r2
                        };

            return View(query.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var query = from i in db.Locations
                        where i.ID == id
                        select i;
            var location = query.FirstOrDefault();

            if (location == null)
            {
                return HttpNotFound();
            }

            DetailsLocationViewModel model = new DetailsLocationViewModel();
            model.Location = location;

            var query2 = from i in db.Images
                        where i.ID == location.ImageId
                        select i;

            var image = query2.FirstOrDefault();

            if (image != null)
            {
                model.Image = image;
            }
            else
            {
                model.Image = null;
            }

            return View(model);
        }
    }
}
