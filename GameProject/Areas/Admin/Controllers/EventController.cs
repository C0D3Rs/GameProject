using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameProject.Models.Entities;
using GameProject.Models;
using GameProject.Enums;
using GameProject.Helpers;
using System.Data.Entity.Infrastructure;
using GameProject.Areas.Admin.ViewModels;
using GameProject.Filters;
using PagedList;

namespace GameProject.Areas.Admin.Controllers
{
    [AuthorizationFilter(UserRole.Admin)]
    public class EventController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(int? page)
        {
            var query = from i in db.Events
                        orderby i.Id descending
                        select i;

            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, 10));
        }

        /*
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from i in db.Events
                        where i.Id == id
                        select i;

            var locationEvent = query.FirstOrDefault();

            if (locationEvent == null)
            {
                return HttpNotFound();
            }

            var query2 = from i in db.Locations
                         where i.ID == locationEvent.LocationId
                         select i;

            var query3 = from a in db.Monsters
                         from b in db.Images
                         where a.ImageId == b.ID
                         select new DetailsMonsterViewModel
                         {
                             Monster = a,
                             Image = b
                         };

            var monsters = query3.ToList();

            var eventLocation = query2.FirstOrDefault();

            CreateEventViewModel model = new CreateEventViewModel();

            model.Location = eventLocation;
            model.Event = locationEvent;
            model.Monsters = monsters;

            return View(model); 
        }
        */
        public ActionResult Create(int? locationID)
        {
            if (locationID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var query = from a in db.Locations
                        where a.ID == locationID
                        select a;

            var one_location = query.FirstOrDefault();

            if (one_location == null)
            {
                return HttpNotFound();
            }

            Event one_event = new Event();

            one_event.LocationId = one_location.ID;

            var query2 = from a in db.Monsters
                         from b in db.Images
                         where a.ImageId == b.ID
                         select new DetailsMonsterViewModel
                         {
                            Monster = a,
                            Image = b
                         };
                        
            var monsters = query2.ToList();


            CreateEventViewModel model = new CreateEventViewModel();
            
            model.Location = one_location;
            model.Event = one_event;
            model.Monsters = monsters;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event one_event)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.Events.Add(one_event);
                    db.SaveChanges();

                    FlashMessageHelper.SetMessage(this, FlashMessageType.Success, "Zapisanie nowych danych przebiegło pomyślnie.");
                    return RedirectToAction("Index");
                }
                FlashMessageHelper.SetMessage(this, FlashMessageType.Info, "Nie można zapisać nowych danych. Należy poprawić zaistniałe błędy.");
            }
            catch (Exception)
            {
                FlashMessageHelper.SetMessage(this, FlashMessageType.Danger, "Wystąpił nieoczekiwany błąd związany z zapisem nowych danych.");
            }

            var query = from a in db.Locations
                        where a.ID == one_event.LocationId
                        select a;

            var fail_event = query.FirstOrDefault();


            CreateEventViewModel setItemViewModel = new CreateEventViewModel()
            {
                Location = fail_event,
                Event = one_event
            };

            return View(setItemViewModel);
        }
    }
}
